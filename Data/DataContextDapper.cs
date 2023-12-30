using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotNetAPI.Data {
    class DataContextDapper {
        private readonly IConfiguration _config;
        IDbConnection _dbConnection;
        public DataContextDapper(IConfiguration config) {
            _config = config;
            _dbConnection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public IEnumerable<T> Query<T>(string sql) {
            return _dbConnection.Query<T>(sql);
        }
        public T QuerySingle<T>(string sql) {
            return _dbConnection.QuerySingle<T>(sql);
        }
        public bool ExecuteBool(string sql) {
            return _dbConnection.Execute(sql) > 0;
        }
        public int Execute(string sql) {
            return _dbConnection.Execute(sql);
        }

        public bool ExecuteSqlWithListOfParameters(string sql, List<SqlParameter> parameters) {
            SqlCommand commandWithParams = new SqlCommand(sql);

            foreach(SqlParameter param in parameters) {
                commandWithParams.Parameters.Add(param);
            }

            SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            dbConnection.Open();
            commandWithParams.Connection = dbConnection;

            int rowsAffected = commandWithParams.ExecuteNonQuery();

            dbConnection.Close();

            return rowsAffected > 0;
        }

        public bool ExecuteSqlWithDynamicParameters(string sql, DynamicParameters parameters) {
            return _dbConnection.Execute(sql, parameters) > 0;
        }

        public IEnumerable<T> QueryWithParameters<T>(string sql, DynamicParameters parameters) {
            return _dbConnection.Query<T>(sql, parameters);
        }
        public T QuerySingleWithParameters<T>(string sql, DynamicParameters parameters) {
            return _dbConnection.QuerySingle<T>(sql, parameters);
        }
    }
}