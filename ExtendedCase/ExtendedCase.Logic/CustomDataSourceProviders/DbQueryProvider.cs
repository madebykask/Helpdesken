using System.Collections.Generic;
using System.Linq;
using ExtendedCase.Dal.Repositories;
using ExtendedCase.Logic.Constants;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.Logic.CustomDataSourceProviders
{
    public class DbQueryProvider : ICustomDataSourceProvider
    {
        private readonly ICustomDataSourceRepository _customDataSourceRepository;

        public string Type => DbProviderTypes.Query;

        #region ctor()

        public DbQueryProvider(ICustomDataSourceRepository customDataSourceRepository)
        {
            _customDataSourceRepository = customDataSourceRepository;
        }

        #endregion

        #region GetData()

        public IList<IDictionary<string, string>> GetData(JObject metaData, JObject query)
        {
            var metaDataClr = metaData.ToObject<MetaData>();
            var queryClr = query.ToObject<Query>();

            var dbParameters = queryClr.Params.ToDictionary(x => x.Name, x => x.Value);

            var results = _customDataSourceRepository.GetDataFromQuery(metaDataClr.Query, metaDataClr.ConnectionString, dbParameters);
            return results;
        }

        #endregion

        #region MetaData classes

        private class MetaData
        {
            public string Query { get; set; }
			public string ConnectionString { get; set; }
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
