using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Configuration;
using sfa.poc.matching.search.azure.application.Entities;
using sfa.poc.matching.search.azure.application.Interfaces;

namespace sfa.poc.matching.search.azure.data
{
    public class AzureSearchProvider : ISearchProvider
    {
        private readonly ISearchConfiguration _configuration;

        public AzureSearchProvider(ISearchConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Course>> FindCourses(IEnumerable<string> keywords)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Location>> FindLocations(IEnumerable<string> keywords)
        {
            throw new NotImplementedException();
        }

    }
}
