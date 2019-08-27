using System.Linq;
using System.Net;
using System.Web;

namespace DH.Helpdesk.SelfService.Infrastructure.Extensions
{
    public static class RequestExtension
    {
        public static string GetComputerName(this HttpRequestBase request)
        {
            try
            {
                var ipAddress = request.GetIpAddress();
                var hostInfo = Dns.GetHostEntry(ipAddress);
                var splitedName = hostInfo.HostName.ToString().Split('.').ToList();
                return splitedName.First();                
            }
            catch 
            {
                return "";
            }
        }

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

        public static byte[] GetFileContent(this HttpRequestBase request)
        {
            byte[] fileContent = null;
            var postedFile = request.Files[0];
            if (postedFile != null && postedFile.HasFile())
            {
                fileContent = new byte[postedFile.InputStream.Length];
                postedFile.InputStream.Read(fileContent, 0, fileContent.Length);
            }
            return fileContent;
        }
    }
}