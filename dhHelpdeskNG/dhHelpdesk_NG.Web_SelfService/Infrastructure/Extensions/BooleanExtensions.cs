
namespace DH.Helpdesk.SelfService.Infrastructure.Extensions
{
    public static class BooleanExtensions
    {
        public static bool ConvertStringToBool(this string value)
        {
            bool ret = false;

            if (!string.IsNullOrWhiteSpace(value))
                bool.TryParse(value, out ret); 

            return ret;
        }

        public static bool ConvertIntToBool(this int value)
        {
            if (value == 1)
                return true;

            return false;
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