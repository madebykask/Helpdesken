using System.Collections.Generic;
using System.Data;
using System.Linq;
using ExtendedCase.Dal.Connection;
using ExtendedCase.Dal.Data;
using ExtendedCase.Dal.Extensions;

namespace ExtendedCase.Dal.Repositories
{
    public class OptionDataSourceRepository : HelpdeskRespositoryBase, IOptionDataSourceRepository
    {
        #region ctor()

        public OptionDataSourceRepository(IDbConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
        }

        #endregion

        #region MetaData

        public string GetMetaDataById(int id)
        {
            const string sql = @"
                SELECT MetaData FROM ExtendedCaseOptionDataSources
                WHERE Id = @id
            ";

            return QueryById<int, string>(sql, id);
        }

        public string GetMetaDataByDataSourceId(string dataSourceId)
        {
            const string sql = @"
                SELECT MetaData FROM ExtendedCaseOptionDataSources
                WHERE DataSourceId = @id
            ";

            return QueryById<string, string>(sql, dataSourceId);
        }

        #endregion //MetaData

        #region Data

        public IList<DataSourceOption> GetOptionsFromTable(string tableName, string idColumn, string valueColumn, IDictionary<string, string> parameters)
        {
            var sql = BuildOptionsTableQuery(tableName, idColumn, valueColumn, parameters);

            var res = QueryList<DataSourceOption>(sql, parameters);
            return res;
        }

        public IList<DataSourceOption> GetOptionsFromQuery(string sql, IDictionary<string, string> parameters)
        {
            var res = QueryList<DataSourceOption>(sql, parameters);
            return res;
        }

        public IList<DataSourceOption> GetOptionsFromSp(string spName, IDictionary<string, string> parameters)
        {
            var res = QueryList<DataSourceOption>(spName, parameters, CommandType.StoredProcedure);
            return res;
        }

        #endregion //Data
    }

    public interface IOptionDataSourceRepository
    {
        string GetMetaDataById(int id);
        string GetMetaDataByDataSourceId(string dataSourceId);

        IList<DataSourceOption> GetOptionsFromTable(string tableName, string idColumn, string valueColumn, IDictionary<string, string> parameters);
        IList<DataSourceOption> GetOptionsFromQuery(string sql, IDictionary<string, string> parameters);
        IList<DataSourceOption> GetOptionsFromSp(string spName, IDictionary<string, string> parameters);
    }
}
