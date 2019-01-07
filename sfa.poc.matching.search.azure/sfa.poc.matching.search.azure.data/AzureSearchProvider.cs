using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Rest.Azure;
using sfa.poc.matching.search.azure.application.Configuration;
using sfa.poc.matching.search.azure.application.Entities;
using sfa.poc.matching.search.azure.application.Interfaces;

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

        public async Task<IEnumerable<Course>> FindCourses(string searchText)
        {
            var indexClient = CreateSearchIndexClient("courses", _configuration.AzureSearchConfiguration);

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

        public const double MilesToMetres = 1609.34;

        public async Task<IEnumerable<Location>> FindLocations(decimal latitude, decimal longitude, decimal distance)
        {
            //Distance is in miles, but we need it in km
            var radius = Convert.ToDouble(distance) * MilesToMetres / 1000;

            var latString = latitude.ToString(CultureInfo.InvariantCulture);
            var lngString = longitude.ToString(CultureInfo.InvariantCulture);
            var radiusString = radius.ToString(CultureInfo.InvariantCulture);

            Console.WriteLine($"Searching for locations within {distance} miles ({radius}km) of ({latitude}, {longitude})");

            var indexClient = CreateSearchIndexClient("locations", _configuration.AzureSearchConfiguration);

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

        public async Task RebuildIndexes()
        {
            var serviceClient = CreateSearchServiceClient(_configuration.AzureSearchConfiguration);

            await DeleteIndexIfExists("courses", serviceClient);
            await CreateCoursesIndex(serviceClient);

            await UploadSynonyms(serviceClient);
            await EnableSynonymsInCoursesIndex(serviceClient);

            var coursesIndexClient = serviceClient.Indexes.GetClient("courses");
            await UploadCourses(coursesIndexClient);

            await DeleteIndexIfExists("locations", serviceClient);
            await CreateLocationsIndex(serviceClient);

            var locationsIndexClient = serviceClient.Indexes.GetClient("locations");

            await UploadLocations(locationsIndexClient);
        }

        private async Task EnableSynonymsInCoursesIndex(ISearchServiceClient serviceClient)
        {
            int MaxNumTries = 3;

            for (int i = 0; i < MaxNumTries; ++i)
            {
                try
                {
                    var index = await serviceClient.Indexes.GetAsync("courses");
                    index = AddSynonymMapsToFields(index);

                    // The IfNotChanged condition ensures that the index is updated only if the ETags match.
                    await serviceClient.Indexes.CreateOrUpdateAsync(index, accessCondition: AccessCondition.IfNotChanged(index));

                    break;
                }
                catch (CloudException e) when (e.IsAccessConditionFailed())
                {
                    Console.WriteLine($"Index update failed : {e.Message}. Attempt({i}/{MaxNumTries}).\n");
                }
            }
        }

        private static Index AddSynonymMapsToFields(Index index)
        {
            index.Fields.First(f => f.Name == "Name").SynonymMaps = new[] { "desc-synonymmap" };
            index.Fields.First(f => f.Name == "Description").SynonymMaps = new[] { "desc-synonymmap" };
            return index;
        }

        private async Task UploadSynonyms(ISearchServiceClient serviceClient)
        {
            // https://docs.microsoft.com/en-us/azure/search/search-synonyms-tutorial-sdk
            var synonymMap = new SynonymMap
            {
                Name = "desc-synonymmap",
                Format = "solr",
                Synonyms = "plumber, plumbing\n" +
                           "electric, electrical, electrician\n" +
                           "electrician, electrical"
            };

            await serviceClient.SynonymMaps.CreateOrUpdateAsync(synonymMap);
        }

        private static SearchServiceClient CreateSearchServiceClient(AzureSearchConfiguration configuration)
        {
            return new SearchServiceClient(configuration.SearchServiceName, new SearchCredentials(configuration.SearchServiceAdminApiKey));
        }

        private static SearchIndexClient CreateSearchIndexClient(string indexName, AzureSearchConfiguration configuration)
        {
            return new SearchIndexClient(configuration.SearchServiceName, indexName, new SearchCredentials(configuration.SearchServiceQueryApiKey));
        }

        private static async Task DeleteIndexIfExists(string indexName, SearchServiceClient serviceClient)
        {
            if (await serviceClient.Indexes.ExistsAsync(indexName))
            {
                await serviceClient.Indexes.DeleteAsync(indexName);
            }
        }

        private static async Task CreateCoursesIndex(ISearchServiceClient serviceClient)
        {
            var indexName = "courses";
            //This is the way to do it if we have annotated our Course class:
            /*
            var indexDefinition = new Index()
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<Course>()
            };
            */

            // https://docs.microsoft.com/en-us/rest/api/searchservice/create-index
            var indexDefinition = new Index()
            {
                Name = indexName,
                Fields = new[]
                {
                    new Field("Id", DataType.String) { IsKey = true },
                    new Field("Name", DataType.String) { IsSearchable = true, IsFilterable = true },
                    new Field("Description", DataType.String) { IsSearchable = true, IsFilterable = true, IsFacetable = true }
                }
            };

            await serviceClient.Indexes.CreateAsync(indexDefinition);
        }

        private static async Task CreateLocationsIndex(ISearchServiceClient serviceClient)
        {
            var indexName = "locations";

            var indexDefinition = new Index()
            {
                Name = indexName,
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

        private async Task UploadCourses(ISearchIndexClient indexClient)
        {
            var page = 1;
            var pageSize = 100;
            IList<Course> courses = null;
            do
            {
                courses = (await _sqlDataRepository.GetPageOfCourses(page, pageSize)).ToList();
                if (courses.Any())
                {
                    var coursesForIndex = courses.Select(IndexedCourse.FromCourse);
                    var batch = IndexBatch.Upload(coursesForIndex);

                    try
                    {
                        var status = await indexClient.Documents.IndexAsync(batch);
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

        private async Task UploadLocations(ISearchIndexClient indexClient)
        {
            var page = 1;
            var pageSize = 100;
            IList<Location> locations = null;
            do
            {
                locations = (await _sqlDataRepository.GetPageOfLocations(page, pageSize)).ToList();
                if (locations.Any())
                {
                    var locationsForIndex = locations.Select(IndexedLocation.FromLocation);
                    var batch = IndexBatch.Upload(locationsForIndex);

                    try
                    {
                        var status = await indexClient.Documents.IndexAsync(batch);
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
    }
}
