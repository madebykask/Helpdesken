using System.Collections.Generic;
using System.Data;
using ExtendedCase.Dal.Connection;

namespace ExtendedCase.Dal.Repositories
{
    public class CustomDataSourceRepository : HelpdeskRespositoryBase, ICustomDataSourceRepository
    {
        #region ctor()

        public CustomDataSourceRepository(IDbConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
        }

        #endregion

        #region MetaData

        public string GetMetaDataById(int id)
        {
            const string sql = @"
                SELECT MetaData FROM ExtendedCaseCustomDataSources
                WHERE Id = @id
            ";

            var res = QueryById<int, string>(sql, id);
            return res;
        }

        public string GetMetaDataByDataSourceId(string dataSourceId)
        {
            const string sql = @"
                SELECT MetaData FROM ExtendedCaseCustomDataSources
                WHERE DataSourceId = @id
            ";

            var res = QueryById<string, string>(sql, dataSourceId);
            return res;
        }

        #endregion MetaData

        #region Data

        public IList<IDictionary<string, string>> GetDataFromTable(string tableName, List<string> columns, IDictionary<string, string> parameters)
        {
            var sql = BuildTableQuery(tableName, columns, parameters);
            return QueryDictionaryList(sql, parameters);
        }

        public IList<IDictionary<string, string>> GetDataFromQuery(string sql, string connectionString, IDictionary<string, string> parameters)
        {
			return QueryDictionaryList(query: sql, parametersDict: parameters, connectionString: connectionString);
        }

        public IList<IDictionary<string, string>> GetDataFromSp(string spName, IDictionary<string, string> parameters)
        {
            return QueryDictionaryList(spName, parameters, CommandType.StoredProcedure);
        }

        #endregion Data
    }

    public interface ICustomDataSourceRepository
    {
        string GetMetaDataById(int id);
        string GetMetaDataByDataSourceId(string dataSourceId);

        IList<IDictionary<string, string>> GetDataFromTable(string tableName, List<string> columns, IDictionary<string, string> parameters);
        IList<IDictionary<string, string>> GetDataFromQuery(string sql, string connectionString, IDictionary<string, string> parameters);
        IList<IDictionary<string, string>> GetDataFromSp(string spName, IDictionary<string, string> parameters);
    }
}