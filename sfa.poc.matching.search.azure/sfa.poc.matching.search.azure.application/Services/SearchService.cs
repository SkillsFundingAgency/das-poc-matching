using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using sfa.poc.matching.search.azure.application.Configuration;
using sfa.poc.matching.search.azure.application.Entities;
using sfa.poc.matching.search.azure.application.Interfaces;

namespace sfa.poc.matching.search.azure.application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchConfiguration _configuration;
        private readonly ISqlDataRepository _sqlDataRepository;
        private readonly ISearchProvider _searchProvider;

        public SearchService(ISearchConfiguration configuration, ISearchProvider searchProvider, ISqlDataRepository sqlDataRepository)
        {
            _configuration = configuration;
            _searchProvider = searchProvider;
            _sqlDataRepository = sqlDataRepository;
        }

        public async Task Index()
        {
            var serviceClient = CreateSearchServiceClient(_configuration.AzureSearchConfiguration);

            await DeleteIndexIfExists("courses", serviceClient);

            await CreateCoursesIndex(serviceClient);

            var indexClient = serviceClient.Indexes.GetClient("courses");

            await UploadCourses(indexClient);
        }

        private static async Task DeleteIndexIfExists(string indexName, SearchServiceClient serviceClient)
        {
            if (await serviceClient.Indexes.ExistsAsync(indexName))
            {
                await serviceClient.Indexes.DeleteAsync(indexName);
            }
        }

        private static async Task CreateCoursesIndex(SearchServiceClient serviceClient)
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
                    new Field("Id", DataType.String) { IsKey = true},
                    new Field("Name", DataType.String) { IsSearchable = true, IsFilterable = true},
                    new Field("Description", DataType.String) { IsFilterable = true, IsFacetable = true }
                }
            };

            await serviceClient.Indexes.CreateAsync(indexDefinition);
        }

        public async Task UploadCourses(ISearchIndexClient indexClient)
        {
            //TODO: Rewrite course paging as in iterator method and do foreach on it

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
                        //TODO: Add a logger and use it insead of console
                        Console.WriteLine("Failed to index some of the documents: {0}",
                            string.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
                    }
                }

                page++;
            } while (courses.Any());
        }

        public async Task<IEnumerable<Course>> Search(string searchText)
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
                searchText += "~2";
            }

            var parameters = new SearchParameters()
            {
                Select = new[] { "Id", "Name" },
                QueryType = QueryType.Full,
                SearchMode = SearchMode.All
            };

            //var searchRequestOptions = new SearchRequestOptions();

            var searchResults = await indexClient.Documents.SearchAsync<IndexedCourse>(searchText, parameters);

            var courses = new List<Course>();
            if (searchResults != null)
            {
                foreach (var result in searchResults.Results)
                {
                    Console.WriteLine($"Found {result.Document.Id} - {result.Document.Name}");
                    courses.Add(result.Document.ToCourse());
                }
            }

            return courses;
        }

        private static SearchServiceClient CreateSearchServiceClient(AzureSearchConfiguration configuration)
        {
            return new SearchServiceClient(configuration.SearchServiceName, new SearchCredentials(configuration.SearchServiceAdminApiKey));
        }

        private static SearchIndexClient CreateSearchIndexClient(string indexName, AzureSearchConfiguration configuration)
        {
            return new SearchIndexClient(configuration.SearchServiceName, indexName, new SearchCredentials(configuration.SearchServiceQueryApiKey));
        }
    }
}
