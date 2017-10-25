using System;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.SelfService.Infrastructure.Helpers;
using Thinktecture.IdentityModel.Web;

namespace DH.Helpdesk.SelfService
{
    public static class IdentityConfiguration
    {
        public static void Configure()
        {
            var loginMode = AppConfigHelper.GetAppSetting(AppSettingsKey.LoginMode);
            if (loginMode.Equals(LoginMode.SSO))
            {
                var configuration = new SSOConfiguration();
                var duration = configuration.SecurityTokenDuration;
                if (duration > 0)
                {
                    PassiveSessionConfiguration.ConfigureDefaultSessionDuration(TimeSpan.FromMinutes(duration));
                }

                if (configuration.EnableSlidingExpiration)
                {
                    PassiveModuleConfiguration.EnableSlidingSessionExpirations();
                }
            }
        }
    }
}