using Dapper;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StoredProcuduresTest.Data
{
    public interface IDapperContext
    {
        public Task<IEnumerable<T>> LoadDataAsync<T>(string sql);

        public Task<T> LoadDataSingleAsync<T>(string sql);

        public Task<bool> ExecuteSql(string sql);

        public Task<int> ExecuteRowCountSql(string sql);

        public Task<bool> ExecuteSqlWithParameters(string sql, DynamicParameters parameters);
        public T LoadDataSingleWithParameters<T>(string sql, DynamicParameters parameters);

    }
}
