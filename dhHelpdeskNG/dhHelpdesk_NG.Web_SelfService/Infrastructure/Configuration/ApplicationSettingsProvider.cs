using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.SelfService.Infrastructure.Helpers;

namespace DH.Helpdesk.SelfService.Infrastructure.Configuration
{
    public interface IApplicationSettings
    {
        string ApplicationType { get; }
        string HelpdeskPath { get; }
    }

    public class ApplicationSettingsProvider : IApplicationSettings
    {
        public string ApplicationType
        {
            get
            {
                return AppConfigHelper.GetAppSetting(AppSettingsKey.CurrentApplicationType);
            }
        }

        public string HelpdeskPath
        {
            get
            {
                return AppConfigHelper.GetAppSetting(AppSettingsKey.HelpdeskPath);
            }
        }
    }
}