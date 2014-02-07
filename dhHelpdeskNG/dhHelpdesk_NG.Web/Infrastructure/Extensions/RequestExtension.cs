namespace DH.Helpdesk.Web.Infrastructure.Extensions
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

    }
}