using System;
using Dapper;
using sfa.poc.matching.search.azure.application.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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
                    SELECT * FROM[dbo].[Courses] c
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
                    SELECT * FROM[dbo].[Locations] l
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
    }
}
