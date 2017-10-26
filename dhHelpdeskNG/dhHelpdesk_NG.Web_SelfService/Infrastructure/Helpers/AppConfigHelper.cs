
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
            if (ConfigurationManager.AppSettings.AllKeys.Contains(AppSettingsKey.CurrentApplicationType,
                                                                  StringComparer.CurrentCultureIgnoreCase))
                return ConfigurationManager.AppSettings[name].ToString().CleanSpaceAndLowStr();

            return null;
        }

        public static WebApiCredentialModel GetAmApiInfo()
        {
            return new WebApiCredentialModel(GetAppSetting(AppSettingsKey.AmApiUri),
                                             GetAppSetting(AppSettingsKey.AmApiUserName),
                                             GetAppSetting(AppSettingsKey.AmApiPassword));
        }

        public static int? GetInt32(string key)
        {
            var val = GetAppSetting(key);
            if (string.IsNullOrEmpty(val))
                return null;

            int result;
            if (Int32.TryParse(val, out result))
                return result;

            return null;
        }

        public static bool? GetBoolean(string key)
        {
            var val = GetAppSetting(key);
            if (string.IsNullOrEmpty(val))
                return null;

            bool result;
            return Boolean.TryParse(val, out result) && result;
        }
    }
}