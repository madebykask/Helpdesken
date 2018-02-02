using System;
using System.Configuration;
using System.Linq;
using DH.Helpdesk.Common.Extensions.String;

namespace DH.Helpdesk.Common.Configuration
{
    public static class AppConfigHelper
    {
        public static string GetAppSetting(string name)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(name))
                return ConfigurationManager.AppSettings[name]?.CleanSpaceAndLowStr();

            return null;
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