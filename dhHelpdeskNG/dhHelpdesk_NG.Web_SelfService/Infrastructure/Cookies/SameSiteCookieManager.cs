using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using SameSiteMode = System.Web.SameSiteMode;

namespace DH.Helpdesk.SelfService.Infrastructure.Cookies
{
    public class SameSiteCookieManager : ICookieManager
    {
        private readonly ICookieManager _innerManager;

        public SameSiteCookieManager() : this(new CookieManager())
        {
        }

        public SameSiteCookieManager(ICookieManager innerManager)
        {
            _innerManager = innerManager;
        }

        public void AppendResponseCookie(IOwinContext context, string key, string value,
            CookieOptions options)
        {
            CheckSameSite(context, options);
            _innerManager.AppendResponseCookie(context, key, value, options);
        }

        public void DeleteCookie(IOwinContext context, string key, CookieOptions options)
        {
            CheckSameSite(context, options);
            _innerManager.DeleteCookie(context, key, options);
        }

        public string GetRequestCookie(IOwinContext context, string key)
        {
            return _innerManager.GetRequestCookie(context, key);
        }

        private void CheckSameSite(IOwinContext context, CookieOptions options)
        {
            if (options.SameSite == Microsoft.Owin.SameSiteMode.None && DisallowsSameSiteNone(context))
            {
                options.SameSite = null;
            }
            else
            {
                options.Secure = true; //SameSiteMode.None should be always with Secure = true in chrome https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-5.0
            }
        }

        public static bool DisallowsSameSiteNone(IOwinContext context)
        {
            // TODO: Use your User Agent library of choice here.
            var userAgent = context.Request.Headers["User-Agent"];
            if (string.IsNullOrEmpty(userAgent))
            {
                return false;
            }

            return userAgent.Contains("BrokenUserAgent") ||
                   userAgent.Contains("BrokenUserAgent2");
        }
    }
}