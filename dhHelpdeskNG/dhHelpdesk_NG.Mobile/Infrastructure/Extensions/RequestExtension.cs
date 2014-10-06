namespace DH.Helpdesk.Mobile.Infrastructure.Extensions
{
    using System.Web;

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

    }
}