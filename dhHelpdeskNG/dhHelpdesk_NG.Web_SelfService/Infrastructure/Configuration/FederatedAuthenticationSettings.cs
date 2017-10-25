using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.SelfService.Infrastructure.Helpers;

namespace DH.Helpdesk.SelfService.Infrastructure.Configuration
{
    public class FederatedAuthenticationSettings
    {
        public int SecurityTokenDuration
        {
            get
            {
                var val = AppConfigHelper.GetInt32(AppSettingsKey.TokenLifeTime);
                return val ?? 0;
            }
        }

        public bool EnableSlidingExpiration
        {
            get
            {
                var val = AppConfigHelper.GetBoolean(AppSettingsKey.EnableSlidingExpiration);
                return val ?? false;
            }
        }

        public bool HandleSecurityTokenExceptions
        {
            get
            {
                var val = AppConfigHelper.GetBoolean(AppSettingsKey.HandleSecurityTokenExceptions);
                return val ?? false;
            }
        }

        public bool LogoutCustomerOnSessionExpire
        {
            get
            {
                var val = AppConfigHelper.GetBoolean(AppSettingsKey.LogoutCustomerOnSessionExpire);
                return val ?? false;
            }
        }
    }
}