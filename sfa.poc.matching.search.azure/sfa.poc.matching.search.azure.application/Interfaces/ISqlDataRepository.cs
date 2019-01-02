using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Entities;

namespace sfa.poc.matching.search.azure.application.Interfaces
{
    public interface ISqlDataRepository
    {
        Task<IEnumerable<Course>> GetPageOfCourses(int pageNumber, int pageSize);

        Task<IEnumerable<Location>> GetPageOfLocations(int pageNumber, int pageSize);
    }
}
