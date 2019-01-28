using System.Configuration;

namespace ExtendedCase.HelpdeskApiClient.Configuration
{
    public class HelpdeskApiSettings : ConfigurationSection, IHelpdeskApiSettings
    {
        [ConfigurationProperty("webApiBaseUri", IsRequired = true)]
        public string WebApiBaseUri => base["webApiBaseUri"]?.ToString();
    }

    public interface IHelpdeskApiSettings
    {
        string WebApiBaseUri { get; }
    }
}