using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using System.Data.Common;

namespace StoredProcuduresTest.Data
{
    public class DapperContext : IDapperContext
    {
        private readonly IConfiguration _config;

        public DapperContext(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IEnumerable<T>> LoadDataAsync<T>(string sql)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            {
                return await connection.QueryAsync<T>(sql);
            }

        }

        public async Task<T> LoadDataSingleAsync<T>(string sql)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            {
                return await connection.QuerySingleAsync<T>(sql);
            }

        }

        public async Task<bool> ExecuteSql(string sql)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            {
                return await connection.ExecuteAsync(sql) > 0;
            }

        }
        public async Task<int> ExecuteRowCountSql(string sql)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            {
                return await connection.ExecuteAsync(sql);
            }

        }

        public async Task<bool> ExecuteSqlWithParameters(string sql, DynamicParameters parameters)
        {
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                return await dbConnection.ExecuteAsync(sql, parameters) > 0;

            }
        }

        public T LoadDataSingleWithParameters<T>(string sql, DynamicParameters parameters)
        {
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            {
                return dbConnection.QuerySingle<T>(sql, parameters);
            }
        }

        public IEnumerable<T> ExecuteSqlWithParameters<T>(string sql, List<SqlParameter> parameters)
        {
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            {
                return dbConnection.Query<T>(sql, parameters);
            }
        }
    }
}
