using System;
using System.Collections.Generic;
using ExtendedCase.Dal.Repositories;
using ExtendedCase.Logic.OptionDataSourceProviders;
using ExtendedCase.Models;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.Logic.Services
{
    public class OptionDataSourceService : IOptionDataSourceService
    {
        private readonly IOptionDataSourceRepository _optionDataSourceRepository;
        private readonly IOptionDataSourceProviderFactory _optionDataSourceProviderFactory;

        public OptionDataSourceService(IOptionDataSourceRepository optionDataSourceRepository, IOptionDataSourceProviderFactory optionDataSourceProviderFactory)
        {
            _optionDataSourceRepository = optionDataSourceRepository;
            _optionDataSourceProviderFactory = optionDataSourceProviderFactory;
        }

        public IList<DataSourceOptionModel> GetOptions(JObject query)
        {
            var dataSourceId = (string)query["Id"];
            int dsInt;
            var metaData = int.TryParse(dataSourceId, out dsInt) ? _optionDataSourceRepository.GetMetaDataById(dsInt) : _optionDataSourceRepository.GetMetaDataByDataSourceId(dataSourceId);
            if (string.IsNullOrWhiteSpace(metaData))
                throw new Exception($"DataSource metadata for id {dataSourceId} not found.");

            var metaDataObj = JObject.Parse(metaData);
            var dataSourceType = (string)metaDataObj["Type"];

            var provider = _optionDataSourceProviderFactory.GetProvider(dataSourceType);

            return provider.GetOptions(metaDataObj, query);
        }
    }

    public interface IOptionDataSourceService
    {
        IList<DataSourceOptionModel> GetOptions(JObject query);
    }
}
