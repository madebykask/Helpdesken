using DH.Helpdesk.BusinessData.Models.WebApi;
using DH.Helpdesk.Common.Configuration;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.SelfService.Infrastructure.Helpers
{
    //todo: implement instance class and move to ISSConfiguration
    public static class WebApiConfig
    {
        public static WebApiCredentialModel GetAmApiInfo()
        {
            return new WebApiCredentialModel(AppConfigHelper.GetAppSetting(AppSettingsKey.AmApiUri),
                AppConfigHelper.GetAppSetting(AppSettingsKey.AmApiUserName),
                AppConfigHelper.GetAppSetting(AppSettingsKey.AmApiPassword));
        }
    }
}