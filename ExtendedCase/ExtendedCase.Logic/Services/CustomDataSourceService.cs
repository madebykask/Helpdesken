using System;
using System.Collections.Generic;
using ExtendedCase.Dal.Repositories;
using ExtendedCase.Logic.CustomDataSourceProviders;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.Logic.Services
{
    public class CustomDataSourceService : ICustomDataSourceService
    {
        private readonly ICustomDataSourceRepository _customDataSourceRepository;
        private readonly ICustomDataSourceProviderFactory _customDataSourceProviderFactory;

        public CustomDataSourceService(ICustomDataSourceRepository customDataSourceRepository, ICustomDataSourceProviderFactory customDataSourceProviderFactory)
        {
            _customDataSourceRepository = customDataSourceRepository;
            _customDataSourceProviderFactory = customDataSourceProviderFactory;
        }

        public IList<IDictionary<string, string>> GetData(JObject query)
        {
            var dataSourceId = (string)query["Id"];
            int dsInt;

            var metaData = int.TryParse(dataSourceId, out dsInt)
                ? _customDataSourceRepository.GetMetaDataById(dsInt)
                : _customDataSourceRepository.GetMetaDataByDataSourceId(dataSourceId);

            if (string.IsNullOrWhiteSpace(metaData))
                throw new Exception($"DataSource metadata for id {dataSourceId} not found.");

            var metaDataObj = JObject.Parse(metaData);
            var dataSourceType = (string)metaDataObj["Type"];

            var provider = _customDataSourceProviderFactory.GetProvider(dataSourceType);

            return provider.GetData(metaDataObj, query);
        }
    }

    public interface ICustomDataSourceService
    {
        IList<IDictionary<string, string>> GetData(JObject query);
    }
}
