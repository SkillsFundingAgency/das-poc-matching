﻿using System.Collections.Generic;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Entities;

namespace sfa.poc.matching.search.azure.application.Interfaces
{
    public interface ISearchService
    {
        Task Index();

        Task<IEnumerable<Course>> Search(string searchText);
    }
}