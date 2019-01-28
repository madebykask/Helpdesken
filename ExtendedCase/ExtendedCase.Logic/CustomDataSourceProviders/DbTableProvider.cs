using System.Collections.Generic;
using System.Linq;
using ExtendedCase.Dal.Repositories;
using ExtendedCase.Logic.Constants;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.Logic.CustomDataSourceProviders
{
    public class DbTableProvider : ICustomDataSourceProvider
    {
        private readonly ICustomDataSourceRepository _customDataSourceRepository;

        public string Type => DbProviderTypes.Table;

        #region ctor()

        public DbTableProvider(ICustomDataSourceRepository customDataSourceRepository)
        {
            _customDataSourceRepository = customDataSourceRepository;
        }

        #endregion

        #region GetData

        public IList<IDictionary<string, string>> GetData(JObject metaData, JObject query)
        {
            var metaDataClr = metaData.ToObject<MetaData>();
            var queryClr = query.ToObject<Query>();

            var metaDataParameters = metaDataClr.Params.ToDictionary(x => x.Name, x => x.ColumnName);
            var queryParameters = queryClr.Params.ToDictionary(x => x.Name, x => x.Value);
            var constantParameters = metaDataClr.ConstParams.ToDictionary(x => x.ColumnName, x => x.Value);

            var parameters = DbQueryHelper.ResolveParameters(queryParameters, metaDataParameters, constantParameters);

            var results = 
                _customDataSourceRepository.GetDataFromTable(metaDataClr.TableName, metaDataClr.Columns, parameters);

            return results;
        }

        #endregion

        #region MetaData Classes

        private class MetaData
        {
            public string TableName { get; set; }
            public List<string> Columns { get; set; }
            public List<Param> Params { get; set; }
            public List<ConstParam> ConstParams { get; set; }
        }

        private class Param
        {
            public string Name { get; set; }
            public string ColumnName { get; set; }
        }

        private class ConstParam
        {
            public string Value { get; set; }
            public string ColumnName { get; set; }
        }

        private class Query
        {
            public List<ParamValue> Params { get; set; }
        }

        private class ParamValue
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        #endregion
    }
}