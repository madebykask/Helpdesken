using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Common.Extensions.Integer
{   
    public static class IntExtension
    {
        public static bool IsValueChanged(this int value)
        {
            return (value != NotChangedValue.INT);
        }

        public static bool IsValueChanged(this int? value)
        {
            return (value != NotChangedValue.NULLABLE_INT);
        }

        public static bool IsNew(this int? value)
        {
            return (!value.HasValue || value.Value == 0);
        }

        public static int? IfNullThenElse(this int? value, int? elseValue)
        {
            return value ?? elseValue;
        }

        public static int ConvertStringToInt(this string value)
        {
            var ret = 0;
            if (!int.TryParse(value, out ret))
                ret = 0;
            return ret;
        }
    }
}
