using System;
using System.Web;

namespace DH.Helpdesk.SelfService.Infrastructure.Helpers
{
    public static class WebMethodsExtensions
    {
        private const string CustomerIdCookieName = "_customerId";

        #region BuildHomeUrl

        public static string BuildHomeUrl(this HttpRequest request, int customerId)
        {
            var requestUrl = request.Url;
            var url = ConvertRelativeUrlToAbsoluteUrl($"/Start/?customerId={customerId}", requestUrl);
            return url;
        }

        private static string ConvertRelativeUrlToAbsoluteUrl(string relativeUrl, Uri requestUrl)
        {
            var port = requestUrl.Port == 80 ? "" : $":{requestUrl.Port}";
                
            var url = string.Format("{0}://{1}{2}/{3}",
                      requestUrl.Scheme,
                      requestUrl.Host,
                      port,
                      relativeUrl.TrimStart('/'));
            return url;
        }

        #endregion

        public static void SetCustomerIdCookie(this HttpContextBase ctx, int customerId)
        {
            SetSessionCookie(ctx, CustomerIdCookieName, customerId.ToString());
        }

        public static int GetCustomerIdFromCookie(this HttpContextBase ctx)
        {
            var cookie = ctx.Request.Cookies[CustomerIdCookieName];
            var val = cookie != null ? cookie.Value : "";
            var customerId = -1;
            int.TryParse(val, out customerId);
            return customerId;
        }

        public static void SetSessionCookie(this HttpContextBase ctx, string name, string value)
        {
            var cookie = ctx.Request.Cookies[name] ?? new HttpCookie(name, value);
            cookie.Expires = DateTime.MinValue;
            cookie.HttpOnly = true;
            cookie.Value = value;
            ctx.Response.Cookies.Set(cookie);
        }

        public static void SetCookie(this HttpContextBase ctx, string name, string value, TimeSpan? expireTime = null)
        {
            var cookie = ctx.Request.Cookies[name] ?? new HttpCookie(name, value);
            cookie.Expires = expireTime != null ? DateTime.Now.Add(expireTime.Value) : DateTime.Now.AddYears(1);
            cookie.Value = value;
            ctx.Response.Cookies.Set(cookie);
        }
    }
}