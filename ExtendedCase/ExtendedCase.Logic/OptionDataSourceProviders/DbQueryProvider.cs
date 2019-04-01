using System.Collections.Generic;
using System.Linq;
using ExtendedCase.Dal.Repositories;
using ExtendedCase.Logic.Constants;
using ExtendedCase.Models;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.Logic.OptionDataSourceProviders
{
    public class DbQueryProvider : IOptionDataSourceProvider
    {
        private readonly IOptionDataSourceRepository _optionDataSourceRepository;

        public string Type => DbProviderTypes.Query;

        #region ctor()

        public DbQueryProvider(IOptionDataSourceRepository optionDataSourceRepository)
        {
            _optionDataSourceRepository = optionDataSourceRepository;
        }

        #endregion

        #region GetOptions

        public IList<DataSourceOptionModel> GetOptions(JObject metaData, JObject query)
        {
            var metaDataClr = metaData.ToObject<MetaData>();
            var queryClr = query.ToObject<Query>();

            var dbParameters = queryClr.Params.ToDictionary(x => x.Name, x => x.Value);

            var dbModel = _optionDataSourceRepository.GetOptionsFromQuery(metaDataClr.Query, dbParameters);
            
            //TODO: mapping
            return dbModel.Select(x => new DataSourceOptionModel { Value = x.Value, Text = x.Text }).ToList();
        }

        #endregion

        #region MetaData Classes

        private class MetaData
        {
            public string Query { get; set; }
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
