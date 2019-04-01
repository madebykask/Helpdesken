using System.Collections.Generic;
using System.Linq;
using ExtendedCase.Dal.Repositories;
using ExtendedCase.Logic.Constants;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.Logic.CustomDataSourceProviders
{
    public class DbSpProvider  : ICustomDataSourceProvider
    {
        private readonly ICustomDataSourceRepository _customDataSourceRepository;

        public string Type => DbProviderTypes.StoredProc;

        #region ctor()

        public DbSpProvider(ICustomDataSourceRepository customDataSourceRepository)
        {
            _customDataSourceRepository = customDataSourceRepository;
        }

        #endregion

        #region GetData

        public IList<IDictionary<string, string>> GetData(JObject metaData, JObject query)
        {
            var metaDataClr = metaData.ToObject<MetaData>();
            var queryClr = query.ToObject<Query>();

            var dbParameters = queryClr.Params.ToDictionary(x => x.Name, x => x.Value);

            var results = _customDataSourceRepository.GetDataFromSp(metaDataClr.ProcedureName, dbParameters);
            return results;
        }

        #endregion

        #region Metadata classes

        private class MetaData
        {
            public string ProcedureName { get; set; }
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
