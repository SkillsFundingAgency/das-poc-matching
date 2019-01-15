using System.Collections.Generic;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Entities;
using sfa.poc.matching.search.azure.application.Search;

namespace sfa.poc.matching.search.azure.application.Interfaces
{
    public interface ISearchService
    {
        Task Index(IndexingOptions options);

        Task<IEnumerable<Course>> SearchCourses(string searchText);

        Task<IEnumerable<Location>> SearchLocations(decimal latitude, decimal longitude, decimal distance);

        Task<IEnumerable<CombinedIndexedItem>> SearchCombinedIndex(string searchText, decimal latitude, decimal longitude, decimal distance);
    }
}
