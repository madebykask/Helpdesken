using System;
using System.Web;

namespace DH.Helpdesk.SelfService.Infrastructure.Helpers
{
    public static class UrlsHelper
    {
        public static string BuildHomeUrl(int customerId)
        {
            var url = $"/Start/?customerId={customerId}";
            return url;
        }

        public static string ConvertRelativeUrlToAbsoluteUrl(string relativeUrl, Uri requestUrl)
        {
            var baseUrl = GetBaseUrl(requestUrl);
            var absoluteUrl = $"{baseUrl}{relativeUrl.TrimStart('/')}";
            return absoluteUrl;
        }

        public static string GetBaseUrl(Uri requestUrl)
        {
            var port = requestUrl.Port == 80 || requestUrl.Port == 443 ? "" : $":{requestUrl.Port}";

            var url = string.Format("{0}://{1}{2}/",
                requestUrl.Scheme,
                requestUrl.Host,
                port);

            return url;
        }
    }
}