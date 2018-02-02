
namespace DH.Helpdesk.SelfService.Infrastructure.Configuration
{
    public interface ISelfServiceConfigurationService
    {
        IApplicationSettings AppSettings { get; }
        ISelfServiceUrlSettings UrlSettings { get; }
    }

    public class SelfServiceConfigurationService : ISelfServiceConfigurationService
    {
        private readonly ApplicationSettingsProvider _appSettings = new ApplicationSettingsProvider();
        private readonly ISelfServiceUrlSettings _urlSettings = SelfServiceUrlSetting.GetSelfServiceUrlSettings();

        public IApplicationSettings AppSettings
        {
            get { return _appSettings; }
        }

        public ISelfServiceUrlSettings UrlSettings
        {
            get { return _urlSettings; }
        }
    }
}