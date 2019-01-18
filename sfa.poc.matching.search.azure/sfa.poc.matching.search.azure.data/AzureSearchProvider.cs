using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Rest.Azure;
using sfa.poc.matching.search.azure.application.Configuration;
using sfa.poc.matching.search.azure.application.Entities;
using sfa.poc.matching.search.azure.application.Helpers;
using sfa.poc.matching.search.azure.application.Interfaces;
using sfa.poc.matching.search.azure.application.Search;

namespace sfa.poc.matching.search.azure.data
{
    public class AzureSearchProvider : ISearchProvider
    {
        private readonly ISearchConfiguration _configuration;
        private readonly ISqlDataRepository _sqlDataRepository;

        public AzureSearchProvider(ISearchConfiguration configuration, ISqlDataRepository sqlDataRepository)
        {
            _configuration = configuration;
            _sqlDataRepository = sqlDataRepository;
        }

        public async Task RebuildIndexes(IndexingOptions options)
        {
            var serviceClient = CreateSearchServiceClient(_configuration.AzureSearchConfiguration);

            //await RebuildCourseIndex(serviceClient);

            //await RebuildLocationIndex(serviceClient);

            await RebuildCombinedIndex(serviceClient, options);
        }

        private SearchServiceClient CreateSearchServiceClient(AzureSearchConfiguration configuration)
        {
            return new SearchServiceClient(configuration.Name, new SearchCredentials(configuration.AdminApiKey));
        }

        private SearchIndexClient CreateSearchIndexClient(string indexName, AzureSearchConfiguration configuration)
        {
            return new SearchIndexClient(configuration.Name, indexName, new SearchCredentials(configuration.QueryApiKey));
        }

        private async Task DeleteIndexIfExists(string indexName, ISearchServiceClient serviceClient)
        {
            if (await serviceClient.Indexes.ExistsAsync(indexName))
            {
                await serviceClient.Indexes.DeleteAsync(indexName);
            }
        }

        #region Course index methods

        public async Task RebuildCourseIndex(ISearchServiceClient serviceClient)
        {
            var indexName = SearchConstants.CoursesIndexName;

            await DeleteIndexIfExists(indexName, serviceClient);
            await CreateCoursesIndex(serviceClient);

            await CreateCourseSynonymMap(serviceClient);

            await EnableSynonymsInCoursesIndex(serviceClient);

            await UploadCourses(serviceClient.Indexes.GetClient(indexName));
        }

        private async Task CreateCoursesIndex(ISearchServiceClient serviceClient)
        {
            // https://docs.microsoft.com/en-us/rest/api/searchservice/create-index
            var indexDefinition = new Index
            {
                Name = SearchConstants.CoursesIndexName,
                Fields = new[]
                {
                    new Field("Id", DataType.String) { IsKey = true },
                    new Field("Name", DataType.String) { IsSearchable = true, IsFilterable = true },
                    new Field("Description", DataType.String) { IsSearchable = true, IsFilterable = true, IsFacetable = true }
                }
            };

            await serviceClient.Indexes.CreateAsync(indexDefinition);
        }

        private async Task UploadCourses(ISearchIndexClient indexClient)
        {
            var page = 1;
            var pageSize = 500;
            IList<Course> courses;
            do
            {
                courses = (await _sqlDataRepository.GetPageOfCourses(page, pageSize)).ToList();
                if (courses.Any())
                {
                    var coursesForIndex = courses.Select(IndexedCourse.FromCourse);
                    var batch = IndexBatch.Upload(coursesForIndex);

                    try
                    {
                        await indexClient.Documents.IndexAsync(batch);
                    }
                    catch (IndexBatchException e)
                    {
                        // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                        // the batch. Depending on your application, you can take compensating actions like delaying and
                        // retrying. For this simple demo, we just log the failed document keys and continue.
                        //TODO: Add a logger and use it instead of console
                        Console.WriteLine("Failed to index some of the documents: {0}",
                            string.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
                    }
                }

                page++;
            } while (courses.Any());
        }

        private Index AddSynonymMapsToCoursesIndexFields(Index index)
        {
            index.Fields.First(f => f.Name == "Name").SynonymMaps = new[] { SearchConstants.CourseSynonymMapName };
            index.Fields.First(f => f.Name == "Description").SynonymMaps = new[] { SearchConstants.CourseSynonymMapName };
            return index;
        }

        private async Task EnableSynonymsInCoursesIndex(ISearchServiceClient serviceClient)
        {
            const int maxTries = 3;

            for (var i = 0; i < maxTries; ++i)
            {
                try
                {
                    var index = await serviceClient.Indexes.GetAsync(SearchConstants.CoursesIndexName);
                    index = AddSynonymMapsToCoursesIndexFields(index);

                    // The IfNotChanged condition ensures that the index is updated only if the ETags match.
                    await serviceClient.Indexes.CreateOrUpdateAsync(index, accessCondition: AccessCondition.IfNotChanged(index));

                    break;
                }
                catch (CloudException e) when (e.IsAccessConditionFailed())
                {
                    Console.WriteLine($"Index update failed : {e.Message}. Attempt({i}/{maxTries}).\n");
                }
            }
        }

        public async Task<IEnumerable<Course>> SearchCourses(string searchText)
        {
            var indexClient = CreateSearchIndexClient(SearchConstants.CoursesIndexName, _configuration.AzureSearchConfiguration);

            var searchTokens = searchText
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                //.Select(s => $"{s}~2");
                .Select(s => s)
                .ToList();

            if (searchTokens.Count == 1)
            {
                //Add fuzzy search for a single word
                //searchText += "~2";
            }

            var parameters = new SearchParameters()
            {
                SearchFields = new[] { "Name", "Description" },
                Select = new[] { "Id", "Name" },
                //QueryType = QueryType.Full,
                //SearchMode = SearchMode.All
            };

            //var searchRequestOptions = new SearchRequestOptions();

            var searchResults = await indexClient.Documents.SearchAsync<IndexedCourse>(searchText, parameters);

            var courses = new List<Course>();
            if (searchResults != null)
            {
                foreach (var result in searchResults.Results)
                {
                    //Console.WriteLine($"Found {result.Document.Id} - {result.Document.Name}");
                    courses.Add(result.Document.ToCourse());
                }
            }

            return courses;
        }

        #endregion

        #region Location index methods

        public async Task RebuildLocationIndex(ISearchServiceClient serviceClient)
        {
            var indexName = SearchConstants.LocationsIndexName;
            await DeleteIndexIfExists(indexName, serviceClient);
            await CreateLocationsIndex(serviceClient);
            await UploadLocations(serviceClient.Indexes.GetClient(indexName));
        }

        private async Task CreateLocationsIndex(ISearchServiceClient serviceClient)
        {
            var indexDefinition = new Index
            {
                Name = SearchConstants.LocationsIndexName,
                Fields = new[]
                {
                    new Field("Id", DataType.String) { IsKey = true },
                    new Field("Postcode", DataType.String) { IsSearchable = true },
                    new Field("Location", DataType.GeographyPoint) { IsFilterable = true, IsSortable = true },
                    new Field("Country", DataType.String) { IsSearchable = true, IsFilterable = true },
                    new Field("Region", DataType.String) { IsFilterable = true, IsFacetable = true },
                    new Field("AdminDistrict", DataType.String) { IsFilterable = true, IsFacetable = true },
                    new Field("AdminDistrictCode", DataType.String) { IsFilterable = true, IsFacetable = true },
                    new Field("AdminCounty", DataType.String) { IsFilterable = true, IsFacetable = true },
                }
            };

            await serviceClient.Indexes.CreateAsync(indexDefinition);
        }

        private async Task UploadLocations(ISearchIndexClient indexClient)
        {
            var page = 1;
            var pageSize = 500;
            IList<Location> locations;
            do
            {
                locations = (await _sqlDataRepository.GetPageOfLocations(page, pageSize)).ToList();
                if (locations.Any())
                {
                    var locationsForIndex = locations.Select(IndexedLocation.FromLocation);
                    var batch = IndexBatch.Upload(locationsForIndex);

                    try
                    {
                        await indexClient.Documents.IndexAsync(batch);
                    }
                    catch (IndexBatchException e)
                    {
                        // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                        // the batch. Depending on your application, you can take compensating actions like delaying and
                        // retrying. For this simple demo, we just log the failed document keys and continue.
                        //TODO: Add a logger and use it instead of console
                        Console.WriteLine("Failed to index some of the documents: {0}",
                            string.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
                    }
                }

                page++;
            } while (locations.Any());
        }

        public async Task<IEnumerable<Location>> SearchLocations(decimal latitude, decimal longitude, decimal searchRadius)
        {
            //Radius is in miles, but we need it in km
            var radius = Convert.ToDouble(searchRadius) * SearchConstants.MilesToMeters / 1000;

            var latString = latitude.ToString(CultureInfo.InvariantCulture);
            var lngString = longitude.ToString(CultureInfo.InvariantCulture);

            Console.WriteLine($"Searching for locations within {radius} miles ({radius}km) of ({latitude}, {longitude})");

            var indexClient = CreateSearchIndexClient(SearchConstants.LocationsIndexName, _configuration.AzureSearchConfiguration);

            var searchParams = new SearchParameters
            {
                Filter = $"geo.distance(Location, geography'POINT({lngString} {latString})') lt {radius}"
            };

            //var searchResults = await indexClient.Documents.SearchAsync<IndexedCourse>(searchText, parameters);
            //For initial test just search for anything
            var keyword = "*";

            var searchResults = await indexClient.Documents.SearchAsync<IndexedLocation>(keyword, searchParams);

            var locations = new List<Location>();
            if (searchResults != null)
            {
                foreach (var result in searchResults.Results)
                {
                    locations.Add(result.Document.ToLocation());
                }
            }

            return locations;

            //$filter = geo.distance(location, geography'POINT(-122.131577 47.678581)') le 10

            // https://stackoverflow.com/questions/48566493/spatial-search-in-azure-search-net-client-api
            /*
            // center of circle to search in
            var lat = ...;
            var lng = ...;
            // radius of circle to search in
            var radius = ...;

            // Make sure to use invariant culture to avoid using invalid decimal separators
            var latString = lat.ToString(CultureInfo.InvariantCulture);
            var lngString = lng.ToString(CultureInfo.InvariantCulture);
            var radiusString = radius.ToString(CultureInfo.InvariantCulture);

            var searchParams = new SearchParameters();
            searchParams.Filter = $"geo.distance(location, geography'POINT({lngString} {latString})') lt {radius}";

            var searchResults = index.Documents.Search<Expert>(keyword, searchParams);
            var items = searchResults.Results.ToList();
             */
        }

        #endregion

        #region Combined index methods

        public async Task RebuildCombinedIndex(ISearchServiceClient serviceClient, IndexingOptions options)
        {
            var indexName = SearchConstants.CombinedIndexName;
            await DeleteIndexIfExists(indexName, serviceClient);

            //await CreateCustomAnalyzers(serviceClient);

            await CreateCombinedIndex(serviceClient);

            if (options.HasFlag(IndexingOptions.UseSynonyms))
            {
                await CreateCourseSynonymMap(serviceClient);
                await EnableSynonymsInCombinedIndex(serviceClient);
            }

            await UploadCombinedIndexItems(serviceClient.Indexes.GetClient(indexName));
        }

        private async Task CreateCombinedIndex(ISearchServiceClient serviceClient)
        {
            var indexDefinition = new Index
            {
                Name = SearchConstants.CombinedIndexName,
                Fields = FieldBuilder.BuildForType<CombinedIndexedItem>(),
                Analyzers = new List<Analyzer>
                {
                    new CustomAnalyzer
                    {
                        Name = SearchConstants.AdvancedAnalyzerName,
                        TokenFilters = new List<TokenFilterName>
                        {
                            TokenFilterName.Lowercase,
                            TokenFilterName.AsciiFolding,
                            //TokenFilterName.Phonetic,
                            //TokenFilterName.EdgeNGram
                        },
                        Tokenizer = TokenizerName.Create(SearchConstants.NGramTokenizerName),
                        //Tokenizer = TokenizerName.EdgeNGram,
                    },
                    new CustomAnalyzer
                    {
                        Name = SearchConstants.AdvancedAnalyzer_2_Name,
                        Tokenizer = TokenizerName.EdgeNGram,
                        TokenFilters = new List<TokenFilterName>
                        {
                            TokenFilterName.Lowercase,
                            "myNGramTokenFilter"
                        }
                    }
                },
                Tokenizers = new List<Tokenizer>
                {
                    new NGramTokenizer(SearchConstants.NGramTokenizerName)
                    {
                        MinGram = 4,
                        MaxGram = 30,
                        TokenChars = new List<TokenCharacterKind>
                        {
                            TokenCharacterKind.Letter,
                            TokenCharacterKind.Digit,
                        }
                    }
                },
                TokenFilters = new List<TokenFilter>
                {
                    new NGramTokenFilterV2
                    {
                        Name = "myNGramTokenFilter",
                        MinGram = 1,
                        MaxGram = 100
                    }
                }
            };

            await serviceClient.Indexes.CreateAsync(indexDefinition);
        }

        private async Task CreateCourseSynonymMap(ISearchServiceClient serviceClient)
        {
            // https://docs.microsoft.com/en-us/azure/search/search-synonyms-tutorial-sdk
            var synonymMap = new SynonymMap
            {
                Name = SearchConstants.CourseSynonymMapName,
                //ETag = "",
                Format = "solr",
                Synonyms = "plumber, plumbing\n" +
                           "electric, electrical, electrician\n" +
                           "electric, electrics, electricity\n" +
                           "electrician, electrical"
            };

            await serviceClient.SynonymMaps.CreateOrUpdateAsync(synonymMap);
        }

        private async Task EnableSynonymsInCombinedIndex(ISearchServiceClient serviceClient)
        {
            const int maxTries = 3;

            for (var i = 0; i < maxTries; ++i)
            {
                try
                {
                    var index = await serviceClient.Indexes.GetAsync(SearchConstants.CombinedIndexName);
                    index.Fields.First(f => f.Name == "courseName").SynonymMaps = new[] { SearchConstants.CourseSynonymMapName };
                    index.Fields.First(f => f.Name == "courseDescription").SynonymMaps = new[] { SearchConstants.CourseSynonymMapName };

                    // The IfNotChanged condition ensures that the index is updated only if the ETags match.
                    await serviceClient.Indexes.CreateOrUpdateAsync(index, accessCondition: AccessCondition.IfNotChanged(index));

                    break;
                }
                catch (CloudException e) when (e.IsAccessConditionFailed())
                {
                    Console.WriteLine($"Index update failed : {e.Message}. Attempt({i}/{maxTries}).\n");
                }
            }
        }

        private async Task UploadCombinedIndexItems(ISearchIndexClient indexClient)
        {
            var page = 1;
            var pageSize = 1000;
            IList<CombinedIndexedItem> items;
            do
            {
                items = (await _sqlDataRepository.GetPageOfCombinedItems(page, pageSize)).ToList();
                if (items.Any())
                {
                    var batch = IndexBatch.Upload(items);

                    Console.WriteLine($"Uploading page {page} with {items.Count} items to index {SearchConstants.CombinedIndexName}");
                    //foreach (var action in batch.Actions)
                    //{
                    //    Console.WriteLine($"    Uploading {action.Document.Id} - {action.Document.CourseName}");
                    //}
                    try
                    {
                        var status = await indexClient.Documents.IndexAsync(batch);
                        Console.WriteLine($"Uploaded {items.Count} documents to index '{indexClient.IndexName}'. Page {page}.");
                        //foreach (var statusResult in status.Results)
                        //{
                        //    Console.WriteLine($"    {statusResult.Key}, {statusResult.Succeeded} {statusResult.StatusCode}, {statusResult.ErrorMessage}");
                        //}
                    }
                    catch (IndexBatchException e)
                    {
                        // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                        // the batch. Depending on your application, you can take compensating actions like delaying and
                        // retrying. For this simple demo, we just log the failed document keys and continue.
                        //TODO: Add a logger and use it instead of console
                        Console.WriteLine("Failed to index some of the documents: {0}",
                            string.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
                    }
                }

                page++;
            } while (items.Any());
        }

        public async Task<IEnumerable<CombinedIndexedItem>> SearchCombinedIndex(string searchText, decimal latitude, decimal longitude, decimal searchRadius)
        {
            var indexClient = CreateSearchIndexClient(SearchConstants.CombinedIndexName, _configuration.AzureSearchConfiguration);

            searchText = !string.IsNullOrWhiteSpace(searchText)
                ? searchText
                : "*";
            Console.WriteLine($"Searching for '{searchText}'");

            var searchParameters = new SearchParameters
            {
                SearchMode = SearchMode.Any,
                //QueryType = QueryType.Full,
                SearchFields = new []
                {
                    "courseName",
                    //"courseDescription",
                    //"providerName"
                },
                OrderBy = new []
                {
                    "courseName desc",
                    //$"geo.distance(location, geography'POINT({longitude} {latitude})')"
                }
            };

            var startLatitude = 0d;
            var startLongitude = 0d;

            if (latitude != 0 && longitude != 0)
            {
                startLatitude = Convert.ToDouble(latitude);
                startLongitude = Convert.ToDouble(longitude);
                var radius = Convert.ToDouble(searchRadius) * SearchConstants.MilesToMeters / 1000;

                Console.WriteLine($"Searching for locations within {radius} miles ({radius}km) of ({latitude}, {longitude})");
                searchParameters.Filter =
                    $"geo.distance(location, geography'POINT({longitude} {latitude})') lt {radius}";

                searchParameters.OrderBy = new []
                {
                    $"geo.distance(location, geography'POINT({longitude} {latitude})')"
                };
            }

            var searchResults = await indexClient.Documents.SearchAsync<CombinedIndexedItem>(searchText, searchParameters);

            Debug.Print($"{searchResults.Results} results. Scores:");
            foreach (var sr in searchResults.Results)
            {
                Debug.Print($"    {sr.Score}: '{sr.Document.CourseName}'");
            }

            var calculator = new DistanceCalculator();

            var results = searchResults.Results.Select(r =>
                {
                    r.Document.SearchScore = r.Score;
                    if (latitude != 0 && longitude != 0)
                    {
                        r.Document.Distance = calculator.DistanceFromLatLong(
                            startLatitude,
                            startLongitude,
                            r.Document.Location.Latitude,
                            r.Document.Location.Longitude);
                        }
                    return r.Document;
                })
                .ToList();
            //SetDistancesInSearchResults(latitude, longitude, results);

            return results;
        }

        private void SetDistancesInSearchResults(decimal latitude, decimal longitude, IEnumerable<CombinedIndexedItem> results)
        {
            if (latitude != 0 && longitude != 0)
            {
                var calculator = new DistanceCalculator();
                foreach (var item in results)
                {
                    item.Distance = calculator.DistanceFromLatLong(
                        Convert.ToDouble(latitude),
                        Convert.ToDouble(longitude),
                        item.Location.Latitude,
                        item.Location.Longitude);
                }
            }

        }

        #endregion
    }
}
