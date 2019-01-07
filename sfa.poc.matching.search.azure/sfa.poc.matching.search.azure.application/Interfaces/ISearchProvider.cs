using System.Collections.Generic;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Entities;

namespace sfa.poc.matching.search.azure.application.Interfaces
{
    public interface ISearchProvider
    {
        Task<IEnumerable<Course>> FindCourses(string searchText);

        Task<IEnumerable<Location>> FindLocations(decimal latitude, decimal longitude, decimal distance);

        Task RebuildIndexes();

    }
}
