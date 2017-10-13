
using DH.Helpdesk.BusinessData.Models.WebApi;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.String;
using System;
using System.Configuration;
using System.Linq;

namespace DH.Helpdesk.SelfService.Infrastructure.Helpers
{
    public static class AppConfigHelper
    {

        public static string GetAppSetting(string name)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(name))
                return ConfigurationManager.AppSettings[name].ToString().CleanSpaceAndLowStr();

            return null;
        }

        public static WebApiCredentialModel GetAmApiInfo()
        {
            return new WebApiCredentialModel(GetAppSetting(AppSettingsKey.AmApiUri),
                                             GetAppSetting(AppSettingsKey.AmApiUserName),
                                             GetAppSetting(AppSettingsKey.AmApiPassword));
        }
    }
}