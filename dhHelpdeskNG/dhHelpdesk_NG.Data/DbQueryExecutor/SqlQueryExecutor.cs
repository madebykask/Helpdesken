using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DH.Helpdesk.Dal.DbQueryExecutor
{
    public class SqlDbQueryExecutor : IDbQueryExecutor
    {
        private readonly string _connectionString;

        #region ctor()

        public SqlDbQueryExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region NonQuery Methods (Update, Insert)

        public int ExecQuery(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            using (IDbConnection conn = CreateConnection())
            {
                var rowsAffected = conn.Execute(query, parameters, commandTimeout: timeout, commandType: commandType);
                return rowsAffected;
            }
        }

        #endregion

        #region Select Methods

        public TEntity QuerySingle<TEntity>(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            using (IDbConnection conn = CreateConnection())
            {
                var result = conn.QuerySingle<TEntity>(query, parameters, commandTimeout: timeout, commandType: commandType);
                return result;
            }
        }

        public IList<TEntity> QueryList<TEntity>(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            using (IDbConnection conn = CreateConnection())
            {
                var result = conn.Query<TEntity>(query, parameters, commandTimeout: timeout, commandType: commandType).ToList();
                return result;
            }
        }

        //selects a single value (ex: int)
        public T ExecuteScalar<T>(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            using (IDbConnection conn = CreateConnection())
            {
                var result = conn.ExecuteScalar<T>(query, parameters, commandTimeout: timeout, commandType: commandType);
                return result;
            }
        }
        
        public DataTable ExecuteTable(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            var dataTable = new DataTable();

            using (var conn = CreateConnection())
            {
                using (var dataReader = conn.ExecuteReader(query, parameters, commandTimeout: timeout, commandType: commandType))
                {
                    
                    dataTable.Load(dataReader);
                }
            }

            return dataTable;
        }

        #endregion

        #region Helper Methods

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        #endregion
    }
}