using System.Collections.Generic;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Entities;

namespace sfa.poc.matching.search.azure.application.Interfaces
{
    public interface ISearchProvider
    {
        Task<IEnumerable<Course>> FindCourses(IEnumerable<string> keywords);

        Task<IEnumerable<Location>> FindLocations(IEnumerable<string> keywords);
    }
}
