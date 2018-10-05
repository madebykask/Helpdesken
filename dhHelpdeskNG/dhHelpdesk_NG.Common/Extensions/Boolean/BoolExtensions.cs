namespace DH.Helpdesk.Common.Extensions.Boolean
{
    public static class BoolExtensions
    {
        public static string ToYesNoString(this bool value)
        {
            return value ? "yes" : "no";
        }

        public static bool ToBool(this string value)
        {
            var ret = false;

            if (!string.IsNullOrWhiteSpace(value))
                bool.TryParse(value, out ret); 

            return ret;
        }

        public static string ToJavaScriptBool(this bool value)
        {
            return value ? "true" : "false";
        }
    }
}