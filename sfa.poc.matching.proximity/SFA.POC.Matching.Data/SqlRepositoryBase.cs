using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SFA.POC.Matching.Data
{
    public abstract class SqlRepositoryBase
    {
        private readonly string _connectionString;

        protected SqlRepositoryBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await getData(connection);
            }
        }
    }
}
