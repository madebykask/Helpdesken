
namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    public static class BooleanExtansions
    {
        public static bool convertStringToBool(this string value)
        {
            bool ret = false;

            if (!string.IsNullOrWhiteSpace(value))
                bool.TryParse(value, out ret); 

            return ret;
        }

        public static bool IntHasValue(this int? value)
        {
            return value != null && value != 0;
        }

        public static string ToJavaScriptBool(this bool value)
        {
            return value ? "true" : "false";
        }
    }
}