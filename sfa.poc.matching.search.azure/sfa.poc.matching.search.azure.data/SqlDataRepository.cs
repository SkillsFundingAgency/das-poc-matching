using System;
using Dapper;
using sfa.poc.matching.search.azure.application.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Spatial;
using sfa.poc.matching.search.azure.application.Interfaces;

namespace sfa.poc.matching.search.azure.data
{
    public class SqlDataRepository : SqlRepositoryBase, ISqlDataRepository
    {
        public SqlDataRepository(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<IEnumerable<Course>> GetPageOfCourses(int pageNumber, int pageSize)
        {
            var offset = pageSize * (pageNumber - 1);

            var sql = $@"
                ;WITH pagedItems AS (
                    SELECT * FROM [dbo].[Courses] c
                    ORDER BY c.Id
                    OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY
                )
                SELECT *
                FROM pagedItems";

            return await WithConnection<IEnumerable<Course>>(async c =>
            {
                try
                {
                    var results = await c.QueryAsync<Course>(
                        sql,
                        //parameters,
                        commandType: CommandType.Text);
                    return results.ToList();
                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<IEnumerable<Location>> GetPageOfLocations(int pageNumber, int pageSize)
        {
            var offset = pageSize * (pageNumber - 1);

            var sql = $@"
                ;WITH pagedItems AS (
                    SELECT * FROM [dbo].[Locations] l
                    ORDER BY l.Id
                    OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY
                )
                SELECT *
                FROM pagedItems";

            return await WithConnection<IEnumerable<Location>>(async c =>
            {
                try
                {
                    var results = await c.QueryAsync<Location>(
                        sql,
                        //parameters,
                        commandType: CommandType.Text);
                    return results.ToList();
                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<IEnumerable<CombinedIndexedItem>> GetPageOfCombinedItems(int pageNumber, int pageSize)
        {
            var offset = pageSize * (pageNumber - 1);

            var sql = $@"
                ;WITH pagedItems AS (
                    SELECT      CONVERT(varchar(19), pcl.[Id]) AS [Id], --Must be a string
                                pcl.[ProviderId],
                                p.[Name] AS [ProviderName],
                                pcl.[CourseId],
					            c.[LearnAimRef] AS [LarsId],
                                c.[Name] AS [CourseName],
                                c.[Description] AS [CourseDescription],
                                pcl.[LocationId],
                                l.[Postcode],
                                l.[Latitude],
                                l.[Longitude],
                                l.[Country],
                                l.[Region],
                                l.[AdminCounty],
                                l.[AdminDistrict],
                                l.[AdminDistrictCode]
                    FROM        [dbo].[ProviderCourseLocations] pcl
                    INNER JOIN  [dbo].[Providers] p
                    ON          p.[Id] = pcl.[ProviderId]
                    INNER JOIN [dbo].[Courses] c
                    ON          c.[Id] = pcl.[CourseId]
                    INNER JOIN  [dbo].[Locations] l
                    ON          l.[Id] = pcl.[LocationId]
                    ORDER BY    pcl.Id
                    OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY
                )
                SELECT *
                FROM pagedItems";

            return await WithConnection<IEnumerable<CombinedIndexedItem>>(async c =>
            {
                try
                {
                    var query = await c.QueryAsync<CombinedIndexedItem>(
                        sql,
                        //parameters,
                        commandType: CommandType.Text);

                        var results = query.ToList();

                    foreach (var item in results)
                    {
                        item.Location = GeographyPoint.Create(
                            Convert.ToDouble(item.Latitude),
                            Convert.ToDouble(item.Longitude));
                    }
                    return results;
                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }
    }
}
