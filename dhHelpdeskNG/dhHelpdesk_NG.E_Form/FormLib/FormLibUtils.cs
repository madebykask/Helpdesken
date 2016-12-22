using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Reflection;

namespace DH.Helpdesk.EForm.FormLib
{
    public class FormLibUtils
    {
        public static bool DateOver(DateTime? startDate, DateTime? endDate, int months)
        {
            if(!startDate.HasValue || !endDate.HasValue) return false;
            DateTime a = startDate.Value.AddMonths(months);
            int result = DateTime.Compare(endDate.Value, a);
            return result > 0;
        }

        public static bool IsSelfService()
        {
            return FormLibUtils.AppSettings.GetBoolean(FormLibConstants.AppSettings.InitFromSelfService);
        }

        public static int GetCaseInitState()
        {
            return (FormLibUtils.IsSelfService()
                && FormLibUtils.AppSettings.GetInteger(FormLibConstants.AppSettings.InitFromSelfServiceWorkflowInitValue) != 0)
                                                    ? FormLibUtils.AppSettings.GetInteger(FormLibConstants.AppSettings.InitFromSelfServiceWorkflowInitValue) : 0;
        }

        public static int GetCaseSaveState()
        {
            return (FormLibUtils.IsSelfService()
                && FormLibUtils.AppSettings.GetInteger(FormLibConstants.AppSettings.InitFromSelfServiceWorkflowInitValue) != 0)
                                                    ? FormLibUtils.AppSettings.GetInteger(FormLibConstants.AppSettings.InitFromSelfServiceWorkflowInitValue) : 20;
        }

        public static int GetSource()
        {
            return FormLibUtils.IsSelfService() ? 2 : 1;
        }

        //public static string AssemblyVersion()
        //{
        //    var assembly = Assembly.GetExecutingAssembly();
        //    var version = assembly.GetName().Version;
        //    return version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
        //}
        public static string AssemblyVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            return System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }

        public class AppSettings
        {
            public static int GetInteger(string key)
            {
                string value = ConfigurationManager.AppSettings[key];

                if(!string.IsNullOrWhiteSpace(value))
                {
                    int answer;
                    if(int.TryParse(value, out answer))
                        return answer;
                }

                return 0;
            }

            public static int? GetNullableInteger(string key)
            {
                string value = ConfigurationManager.AppSettings[key];

                if(!string.IsNullOrWhiteSpace(value))
                {
                    int answer;
                    if(int.TryParse(value, out answer))
                        return answer;
                }

                return null;
            }

            public static bool GetBoolean(string key)
            {
                string value = ConfigurationManager.AppSettings[key];

                if(!string.IsNullOrWhiteSpace(value))
                {
                    bool answer;
                    if(bool.TryParse(value, out answer))
                        return answer;
                }

                return false;
            }

            public static bool? GetNullableBoolean(string key)
            {
                string value = ConfigurationManager.AppSettings[key];

                if(!string.IsNullOrWhiteSpace(value))
                {
                    bool answer;
                    if(bool.TryParse(value, out answer))
                        return answer;
                }

                return null;
            }

            public static DateTime? GetNullableDateTime(string key)
            {
                string value = ConfigurationManager.AppSettings[key];

                if(!string.IsNullOrWhiteSpace(value))
                {
                    DateTime answer;
                    if(DateTime.TryParse(value, out answer))
                        return answer;
                }

                return null;
            }

            public static string GetString(string key)
            {
                string value = ConfigurationManager.AppSettings[key];

                if(!string.IsNullOrWhiteSpace(value))
                    return value;

                return null;
            }

            public static bool OptionalSettingHasValue(string key, string value)
            {
                string actualValue = ConfigurationManager.AppSettings[key];

                if(!string.IsNullOrWhiteSpace(actualValue) && actualValue.ToUpper() == value.ToUpper())
                    return true;

                return false;
            }
        }
    }
}