using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Common.Extensions.Decimal
{   
    public static class DecimalExtension
    {
        public static bool IsValueChanged(this decimal value)
        {
            return (value != NotChangedValue.DECIMAL);
        }

        public static decimal? IfNullThenElse(this decimal? value, decimal? elseValue)
        {
            return value ?? elseValue;
        }
    }
}
