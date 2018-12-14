using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.POC.Matching.Application.Interfaces;
using SFA.POC.Matching.Application.Models;

namespace SFA.POC.Matching.Data
{
    public class SqlLocationReader : SqlRepositoryBase, ILocationReader
    {
        public SqlLocationReader(string connectionString) : base(connectionString)
        {
        }

        public async Task<IList<LocationModel>> SearchLocationsWithByDistanceAsync(string postcode, decimal searchRadiusInMeters)
        {
            IEnumerable<LocationModel> searchResult = null;

            await WithConnection(async c =>
            {
                    try
                    {
                        //var parameters = new DynamicParameters();
                        //parameters.Add("@postcode", postcode, DbType.String);
                        //parameters.Add("@distance", searchRadiusInMeters, DbType.String);
                        searchResult = await c.QueryAsync<LocationModel>(@"
                                SELECT * FROM [dbo].[Locations] WHERE [Postcode] = @postcode",
                            new { postcode, searchRadiusInMeters },   
                            commandType: CommandType.Text);
                    }
                    catch (SqlException ex)
                    {
                        throw;
                    }

                return searchResult;
            });

            return searchResult?.ToList() ?? new List<LocationModel>();
        }

        public async Task<IList<LocationModel>> SearchLocationsWithByDistanceAsync(decimal latitude, decimal longitude, decimal searchRadiusInMeters)
        {
            IEnumerable<LocationModel> searchResult = null;
            var searchRadiusFloat = Convert.ToDouble(searchRadiusInMeters);

            await WithConnection(async c =>
            {
                try
                {
                    searchResult = await c.QueryAsync<LocationModel>(@"
                            WITH [Params] AS 
                            (
	                            SELECT geography::Point(@latitude, @longitude, 4326) AS [CentrePoint]
                            )
                            SELECT [PostCode], 
		                            [Latitude], 
		                            [Longitude],
		                            [Location].STDistance([Params].[CentrePoint]) AS [Distance],
		                            [Country],
		                            [Region],
		                            [AdminDistrict],
		                            [AdminCounty]
                            FROM [Params]
                            CROSS APPLY [dbo].[Locations] 
                            WHERE [Location].STDistance([Params].[CentrePoint]) <= @searchRadiusFloat
                            ORDER BY [Location].STDistance([Params].[CentrePoint])",
                        new {
                            latitude,
                            longitude,
                            searchRadiusFloat
                        },
                        commandType: CommandType.Text);
                }
                catch (SqlException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }

                return searchResult;
            });

            return searchResult?.ToList() ?? new List<LocationModel>();
        }
    }
}
