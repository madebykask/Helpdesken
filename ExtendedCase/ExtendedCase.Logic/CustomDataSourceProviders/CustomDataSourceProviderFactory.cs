using System;
using System.Linq;

namespace ExtendedCase.Logic.CustomDataSourceProviders
{
    public class CustomDataSourceProviderFactory : ICustomDataSourceProviderFactory
    {
        private readonly ICustomDataSourceProvider[] _providers;

        public CustomDataSourceProviderFactory(ICustomDataSourceProvider[] providers)
        {
            _providers = providers;
        }

        public ICustomDataSourceProvider GetProvider(string type)
        {
            var provider = _providers.FirstOrDefault(x => x.Type == type);

            if (provider == null)
                throw new Exception($"CustomDatasource type {type} is not supported");

            return provider;
        }
    }

    public interface ICustomDataSourceProviderFactory
    {
        ICustomDataSourceProvider GetProvider(string type);
    }
}