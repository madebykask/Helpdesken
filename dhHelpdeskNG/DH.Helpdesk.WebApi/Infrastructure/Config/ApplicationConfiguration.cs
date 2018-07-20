using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using DH.Helpdesk.Common.Configuration;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.WebApi.Infrastructure.Config
{
internal sealed class ApplicationConfiguration : IApplicationConfiguration
    {
        private CultureInfo _defaultCulture;
        private string _applicationId;

        public CultureInfo DefaultCulture
        {
            get
            {
                if (_defaultCulture == null)
                {
                    var fromConfig = AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultCulture);
                    if (!string.IsNullOrEmpty(fromConfig))
                    {
                        _defaultCulture = new CultureInfo(fromConfig);
                        return _defaultCulture;
                    }

                    if (_defaultCulture == null)
                    {
                        if (HttpContext.Current.Request.UserLanguages != null)
                        {
                            var userLaunguage = HttpContext.Current.Request.UserLanguages.FirstOrDefault();
                            if (!string.IsNullOrEmpty(userLaunguage))
                            {
                                _defaultCulture = new CultureInfo(userLaunguage);
                                return _defaultCulture;
                            }
                        }                        
                    }

                    if (_defaultCulture == null)
                    {
                        _defaultCulture = new CultureInfo("en-US");
                    }
                }

                return _defaultCulture;
            }
        }

        public string ApplicationId
        {
            get
            {
                if (string.IsNullOrEmpty(_applicationId))
                {
                    _applicationId = AppConfigHelper.GetAppSetting(AppSettingsKey.ApplicationId);
                }

                return _applicationId;
            }
        }

        public LoginMode LoginMode
        {
            get
            {
                var val = AppConfigHelper.GetAppSetting(AppSettingsKey.LoginMode);
                if (string.IsNullOrEmpty(val))
                    return LoginMode.None;

                var loginMode = (LoginMode)Enum.Parse(typeof(LoginMode), val, true);
                return loginMode;
            }
        }
    }

    public interface IApplicationConfiguration
    {
        CultureInfo DefaultCulture { get; }
        string ApplicationId { get; }
        LoginMode LoginMode { get; }
    }
}