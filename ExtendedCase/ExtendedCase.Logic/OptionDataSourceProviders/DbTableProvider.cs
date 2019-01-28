using System.Collections.Generic;
using System.Linq;
using ExtendedCase.Dal.Repositories;
using ExtendedCase.Logic.Constants;
using ExtendedCase.Models;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.Logic.OptionDataSourceProviders
{
    public class DbTableProvider : IOptionDataSourceProvider
    {
        private readonly IOptionDataSourceRepository _optionDataSourceRepository;

        public string Type => DbProviderTypes.Table;

        #region ctor()

        public DbTableProvider(IOptionDataSourceRepository optionDataSourceRepository)
        {
            _optionDataSourceRepository = optionDataSourceRepository;
        }

        #endregion

        public IList<DataSourceOptionModel> GetOptions(JObject metaData, JObject query)
        {
            var metaDataClr = metaData.ToObject<MetaData>();
            var queryClr = query.ToObject<Query>();

            var metaDataParams = metaDataClr.Params.ToDictionary(x => x.Name, x => x.ColumnName);
            var queryParams = queryClr.Params.ToDictionary(x => x.Name, x => x.Value);
            var contsParams = metaDataClr.ConstParams.ToDictionary(x => x.ColumnName, x => x.Value);

            var parameters = DbQueryHelper.ResolveParameters(queryParams, metaDataParams, contsParams);

            var dbModel = _optionDataSourceRepository.GetOptionsFromTable(metaDataClr.TableName, metaDataClr.IdColumn, metaDataClr.ValueColumn, parameters);
            
            //TODO: mapping
            return dbModel.Select(x => new DataSourceOptionModel { Value = x.Value, Text = x.Text }).ToList();
        }

        #region MetaData classes

        private class MetaData
        {
            public string TableName { get; set; }
            public string IdColumn { get; set; }
            public string ValueColumn { get; set; }
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
