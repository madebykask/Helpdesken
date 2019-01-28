using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using ExtendedCase.Dal.Connection;
using ExtendedCase.Dal.Extensions;

namespace ExtendedCase.Dal.Repositories
{
    public abstract class DapperRepositoryBase
    {
        private readonly string _connectionStringName;
        private readonly IDbConnectionFactory _connectionFactory;

        #region ctor()

        protected DapperRepositoryBase(string connectionStringName, IDbConnectionFactory connectionFactory)
        {
            _connectionStringName = connectionStringName;
            _connectionFactory = connectionFactory;
        }

        #endregion

        #region CreateConnection

        protected IDbConnection CreateConnection()
        {
            return _connectionFactory.GetConnection(_connectionStringName);
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

        #region QueryMethods

        protected TEntity QueryById<TKey, TEntity>(string query, TKey id, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            using (IDbConnection conn = CreateConnection())
            {
                var result = conn.QuerySingle<TEntity>(query, new {id}, commandTimeout: timeout, commandType: commandType);
                return result;
            }
        }

        public T QueryScalar<T>(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            using (IDbConnection conn = CreateConnection())
            {
                var result = conn.ExecuteScalar<T>(query, parameters, commandTimeout: timeout, commandType: commandType);
                return result;
            }
        }

        protected TEntity QuerySingle<TEntity>(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            using (IDbConnection conn = CreateConnection())
            {
                var result = conn.QuerySingle<TEntity>(query, parameters, commandTimeout: timeout, commandType: commandType);
                return result;
            }
        }

        protected TEntity QuerySingle<TEntity>(string query, IDictionary<string, string> parametersDict = null, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            using (IDbConnection conn = CreateConnection())
            {
                var result = conn.QuerySingle<TEntity>(query, parametersDict?.ToDynamicParameters(), commandTimeout: timeout, commandType: commandType);
                return result;
            }
        }

        protected IList<TEntity> QueryList<TEntity>(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            using (IDbConnection conn = CreateConnection())
            {
                var result = conn.Query<TEntity>(query, parameters, commandTimeout: timeout, commandType: commandType).ToList();
                return result;
            }
        }

        protected IList<TEntity> QueryList<TEntity>(string query, IDictionary<string, string> parametersDict = null, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            using (IDbConnection conn = CreateConnection())
            {
                var result = conn.Query<TEntity>(query, parametersDict?.ToDynamicParameters(), commandTimeout: timeout, commandType: commandType).ToList();
                return result;
            }
        }

        protected IList<IDictionary<string, string>> QueryDictionaryList(string query, IDictionary<string, string> parametersDict = null, CommandType commandType = CommandType.Text, int timeout = 60)
        {
            IList<IDictionary<string, string>> results = null;

            using (IDbConnection conn = CreateConnection())
            {
                var dynamicResults = conn.Query(query, parametersDict?.ToDynamicParameters(), commandTimeout: timeout, commandType: commandType) as IEnumerable<IDictionary<string, object>>;
                if (dynamicResults != null)
                {
                    results = dynamicResults.Select(r => (IDictionary<string, string>)r.ToDictionary(d => d.Key, d => d.Value?.ToString())).ToList();
                }
            }

            return results;
        }

        #endregion
    }
}
