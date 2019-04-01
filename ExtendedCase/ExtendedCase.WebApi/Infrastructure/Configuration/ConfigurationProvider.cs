using System.Configuration;
using ExtendedCase.HelpdeskApiClient.Configuration;

namespace ExtendedCase.WebApi.Infrastructure.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private IHelpdeskApiSettings _helpdeskApiSettings;

        public IHelpdeskApiSettings HelpdeskApiSettings
        {
            get
            {
                if (_helpdeskApiSettings == null)
                {
                    _helpdeskApiSettings = (IHelpdeskApiSettings)ConfigurationManager.GetSection("helpdeskApiSettings");
                }
                
                return _helpdeskApiSettings;
            }
        }
    }

    public interface IConfigurationProvider
    {
        IHelpdeskApiSettings HelpdeskApiSettings { get; }
    }
}