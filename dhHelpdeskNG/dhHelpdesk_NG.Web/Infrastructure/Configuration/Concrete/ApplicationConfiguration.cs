﻿using System;
using System.Collections.Generic;
using DH.Helpdesk.Common.Configuration;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Web.Infrastructure.Configuration.Concrete
{
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Web;

    internal sealed class ApplicationConfiguration : IApplicationConfiguration
    {
        private CultureInfo defaultCulture;
        private string applicationId;

        public CultureInfo DefaultCulture
        {
            get
            {
                if (this.defaultCulture == null)
                {
                    var fromConfig = ConfigurationManager.AppSettings["Application.DefaultCulture"];
                    if (!string.IsNullOrEmpty(fromConfig))
                    {
                        this.defaultCulture = new CultureInfo(fromConfig);
                        return this.defaultCulture;
                    }

                    if (this.defaultCulture == null)
                    {
                        if (HttpContext.Current.Request.UserLanguages != null)
                        {
                            var userLaunguage = HttpContext.Current.Request.UserLanguages.FirstOrDefault();
                            if (!string.IsNullOrEmpty(userLaunguage))
                            {
                                this.defaultCulture = new CultureInfo(userLaunguage);
                                return this.defaultCulture;
                            }
                        }                        
                    }

                    if (this.defaultCulture == null)
                    {
                        this.defaultCulture = new CultureInfo("en-US");
                    }
                }

                return this.defaultCulture;
            }
        }

        public string ApplicationId
        {
            get
            {
                if (string.IsNullOrEmpty(this.applicationId))
                {
                    this.applicationId = ConfigurationManager.AppSettings["ApplicationId"];
                }

                return this.applicationId;
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

        public IList<string> WinAuthIPFilter
        {
            get
            {
                var ipList = ConfigurationManager.AppSettings["winAuthIPFilter"] ?? string.Empty;
                return ipList.Split(new [] {';'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }
    }
}