using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedCase.Logic.OptionDataSourceProviders
{
    public class OptionDataSourceProviderFactory : IOptionDataSourceProviderFactory
    {
        private readonly IOptionDataSourceProvider[] _providers;

        public OptionDataSourceProviderFactory(IOptionDataSourceProvider[] providers)
        {
            _providers = providers;
        }

        public IOptionDataSourceProvider GetProvider(string type)
        {
            var provider = _providers.FirstOrDefault(x => x.Type == type);
            
            if (provider == null)
                throw new Exception($"OptionDatasource type {type} is not supported");

            return provider;
        }
    }

    public interface IOptionDataSourceProviderFactory
    {
        IOptionDataSourceProvider GetProvider(string type);
    }
}
