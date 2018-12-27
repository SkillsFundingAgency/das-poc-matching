using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.POC.Matching.Application.Interfaces;
using SFA.POC.Matching.Application.Models;

namespace SFA.POC.Matching.Data
{
    public class SqlLocationWriter : SqlRepositoryBase, ILocationWriter
    {
        public SqlLocationWriter(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveAsync(LocationModel location)
        {
            await WithConnection(async c =>
            {
                using (var transaction = c.BeginTransaction())
                {
                    try
                    {
                        await c.ExecuteAsync(@"
                            IF NOT EXISTS (SELECT 1 FROM [dbo].[Locations] WHERE [Postcode] = @Postcode)
                              INSERT INTO [dbo].[Locations] (
                                    [Postcode], 
                                    [Latitude], 
                                    [Longitude], 
                                    [Location], 
                                    [Country],
                                    [Region],
                                    [AdminDistrict],
                                    [AdminDistrictCode],
                                    [AdminCounty])
                              VALUES (
                                    @Postcode, 
                                    @Latitude, 
                                    @Longitude, 
                                    geography::Point(@Latitude, @Longitude, 4326),
                                    @Country,
                                    @Region,
                                    @AdminDistrict,
                                    @AdminDistrictCode,
                                    @AdminCounty)",
                            location,
                            commandType: CommandType.Text,
                            transaction: transaction);

                        //Probably don't need the transaction since we do a single SQL statement
                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return 0;
            });
        }
    }
}
