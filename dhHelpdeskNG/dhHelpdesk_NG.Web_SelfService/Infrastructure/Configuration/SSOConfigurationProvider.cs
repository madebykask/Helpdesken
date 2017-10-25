using DH.Helpdesk.SelfService.Infrastructure.Helpers;

namespace DH.Helpdesk.SelfService.Infrastructure.Configuration
{
    public class SSOConfiguration
    {
        private const string TokenLifeTimeKey = "SSO.TokenLifeTime";
        private const string EnableSlidingExpirationKey = "SSO.EnableSlidingExpiration";

        public int SecurityTokenDuration
        {
            get
            {
                var val = AppConfigHelper.GetAppSetting(TokenLifeTimeKey);
                int duration;
                if (int.TryParse(val, out duration))
                {
                    return duration;
                }
                return 0;
            }
        }

        public bool EnableSlidingExpiration
        {
            get
            {
                var val = AppConfigHelper.GetAppSetting(EnableSlidingExpirationKey);
                if (string.IsNullOrEmpty(val))
                    return false;

                bool enable;
                if (bool.TryParse(val, out enable))
                {
                    return enable;
                }
                return false;
            }
        }
    }
}