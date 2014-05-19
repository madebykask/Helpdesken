
namespace DH.Helpdesk.NewSelfService.Infrastructure.Extensions
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

        public static bool convertIntToBool(this int value)
        {
            if (value == 1)
                return true;

            return false;
        }

        public static bool IntHasValue(this int? value)
        {
            return value != null && value != 0;
        }
    }
}