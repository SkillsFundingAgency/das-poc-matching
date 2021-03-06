﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sfa.poc.matching.search.azure.application.Entities;
using sfa.poc.matching.search.azure.application.Interfaces;
using sfa.poc.matching.search.azure.application.Search;

namespace sfa.poc.matching.search.azure.data
{
    public class SqlSearchProvider : SqlRepositoryBase, ISearchProvider
    {
        public SqlSearchProvider(string connectionString)
            : base(connectionString)
        {
        }

        public Task RebuildIndexes(IndexingOptions options)
        {
            return Task.FromResult(0);
        }

        public Task<IEnumerable<Course>> SearchCourses(string searchText)
        {
            throw new NotImplementedException();

            //TODO: Change to use a searchText string for all searches.
            //TODO: Provide a combined search for courses/locations and providers

            //var searchTokens = searchText
            //    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            //    .Select(s => s)
            //    .ToList();
            /*
             This should build and perform a search similar to:
                declare @searchWord1 nvarchar(100) = 'web';
                declare @searchWord2 nvarchar(100) = 'design';

                SELECT * 
                FROM [dbo].[Courses] a
                WHERE (
		                   a.[Name] LIKE '%' + @searchWord1 + '%'
		                OR a.[Description] LIKE '%' + @searchWord1 + '%'
		                OR a.[Name] LIKE '%' + @searchWord2 + '%'
		                OR a.[Description] LIKE '%' + @searchWord2 + '%'
                 )
                ORDER BY a.Id
             */
        }

        public Task<IEnumerable<Location>> SearchLocations(decimal latitude, decimal longitude, decimal distance)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CombinedIndexedItem>> SearchCombinedIndex(string searchString, decimal latitude, decimal longitude, decimal distance)
        {
            throw new NotImplementedException();
        }
    }
}
