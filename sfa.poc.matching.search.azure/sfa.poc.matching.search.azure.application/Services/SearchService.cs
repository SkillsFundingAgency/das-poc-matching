using System.Collections.Generic;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Entities;
using sfa.poc.matching.search.azure.application.Interfaces;
using sfa.poc.matching.search.azure.application.Search;

namespace sfa.poc.matching.search.azure.application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchProvider _searchProvider;

        public SearchService(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        public async Task Index(IndexingOptions options = IndexingOptions.Default)
        {
            await _searchProvider.RebuildIndexes(options);
        }
        
        public async Task<IEnumerable<Course>> SearchCourses(string searchText)
        {
            return await _searchProvider.SearchCourses(searchText);
        }

        public async Task<IEnumerable<Location>> SearchLocations(decimal latitude, decimal longitude, decimal distance)
        {
            return await _searchProvider.SearchLocations(latitude, longitude, distance);
        }

        public async Task<IEnumerable<CombinedIndexedItem>> SearchCombinedIndex(string searchText, decimal latitude, decimal longitude, decimal distance)
        {
            return await _searchProvider.SearchCombinedIndex(searchText, latitude, longitude, distance);
        }
    }
}
