using System;
using System.Web.WebPages;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Http.Controllers;

    public static class RequestExtension
    {

        public static string GetIpAddress(this HttpRequestBase request)
        {
            string ret;

            try
            {
                ret = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrWhiteSpace(ret))
                {
                    ret = request.UserHostAddress;
                }
            }
            catch { ret = string.Empty; }

            return ret;
        }

        public static string GetAbsoluteUrl()
        {
            //Return variable declaration
            string appPath = string.Empty;

            //Getting the current context of HTTP request
            HttpContext context = HttpContext.Current;

            //Checking the current context content
            if (context != null)
            {
                //Formatting the fully qualified website url/name
                appPath = string.Format("{0}://{1}{2}{3}",
                context.Request.Url.Scheme,
                context.Request.Url.Host,
                context.Request.Url.Port == 80
                    ? string.Empty : ":" + context.Request.Url.Port,
                context.Request.ApplicationPath);
            }
            if (!appPath.EndsWith("/"))
                appPath += "/";
            return appPath;
        }

		public static bool IsAbsoluteUrlLocalToHost(this HttpRequestBase request, string url)
		{
			if (url.IsEmpty())
			{
				return false;
			}

			Uri absoluteUri;
			if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
			{
				return String.Equals(request.Url.Host, absoluteUri.Host, StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				bool isLocal = !url.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
					&& !url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
					&& Uri.IsWellFormedUriString(url, UriKind.Relative);
				return isLocal;
			}
		}

        //public static Tuple<int, string> GetCurrentUser(this HttpRequestContext request)
        public static string[] GetClaims(this HttpRequestContext request, params string[] claimTypes)
        {            
            var ret = new string[] { };
            var idt = request.Principal.Identity as ClaimsIdentity;
            if (idt == null)
                return ret;

            ret = new string[claimTypes.Length];
            for (var i=0; i<claimTypes.Length; i++)
            {
                var curClaim = idt.Claims.Where(x => x.Type == claimTypes[i])
                                         .Select(x => x.Value)
                                         .FirstOrDefault();
                ret[i] = curClaim;
            }

            return ret;
        }
    }
}