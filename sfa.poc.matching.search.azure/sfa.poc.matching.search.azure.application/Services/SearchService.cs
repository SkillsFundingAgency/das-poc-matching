using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Entities;
using sfa.poc.matching.search.azure.application.Interfaces;

namespace sfa.poc.matching.search.azure.application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchProvider _searchProvider;

        public SearchService(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        public async Task Index()
        {
            await _searchProvider.RebuildIndexes();
        }
        
        public async Task<IEnumerable<Course>> SearchCourses(string searchText)
        {
            return await _searchProvider.FindCourses(searchText);
        }

        public async Task<IEnumerable<Location>> SearchLocations(decimal latitude, decimal longitude, decimal distance)
        {
            return await _searchProvider.FindLocations(latitude, longitude, distance);
        }
    }
}
