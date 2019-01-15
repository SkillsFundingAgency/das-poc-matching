using System.Collections.Generic;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Entities;
using sfa.poc.matching.search.azure.application.Search;

namespace sfa.poc.matching.search.azure.application.Interfaces
{
    public interface ISearchProvider
    {
        Task RebuildIndexes(IndexingOptions options);

        Task<IEnumerable<Course>> SearchCourses(string searchText);

        Task<IEnumerable<Location>> SearchLocations(decimal latitude, decimal longitude, decimal searchRadius);
        
        Task<IEnumerable<CombinedIndexedItem>> SearchCombinedIndex(string searchText, decimal latitude, decimal longitude, decimal searchRadius);
    }
}
