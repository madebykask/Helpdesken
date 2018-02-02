using System;
using System.Web;

namespace DH.Helpdesk.SelfService.Infrastructure.Helpers
{
    public static class WebMethodsExtensions
    {
        private const string CustomerIdQueryStringParam = "customerId";
        private const string CustomerIdCookieName = "_customerId";

        public static void SetCustomerIdCookie(this HttpContextBase ctx, int customerId)
        {
            SetSessionCookie(ctx, CustomerIdCookieName, customerId.ToString());
        }

        public static int GetCustomerIdFromQueryString(this HttpContextBase ctx)
        {
            var val = ctx.Request.QueryString[CustomerIdQueryStringParam];
            var customerId = -1;
            int.TryParse(val, out customerId);
            return customerId;
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