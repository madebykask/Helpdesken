using System;
using DH.Helpdesk.Common.Configuration;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.SelfService.Infrastructure.Configuration
{
    public interface IApplicationSettings
    {
        string ApplicationType { get; }
        string HelpdeskPath { get; }
        LoginMode LoginMode { get; }
        bool ShowConfirmAfterCaseRegistration { get; }
        bool ShowCommunicationForSelfService { get; }
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

        public LoginMode LoginMode
        {
            get
            {
                var val = AppConfigHelper.GetAppSetting(AppSettingsKey.LoginMode);
                var loginType = (LoginMode)Enum.Parse(typeof(LoginMode), val, true);
                return loginType;
            }
        }

        public bool ShowConfirmAfterCaseRegistration
        {
            get
            {
                var val = AppConfigHelper.GetBoolean(AppSettingsKey.ConfirmMsgAfterCaseRegistration);
                return val ?? false;
            }
        }

        public bool ShowCommunicationForSelfService
        {
            get
            {
                var val = AppConfigHelper.GetBoolean(AppSettingsKey.ShowCommunicationForSelfService);
                return val ?? false;
            }
        }
    }
}