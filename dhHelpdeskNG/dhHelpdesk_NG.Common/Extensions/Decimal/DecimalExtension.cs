using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Common.Extensions.Decimal
{   
    public static class DecimalExtension
    {
        public static bool IsValueChanged(this decimal value)
        {
            return (value != NotChangedValue.DECIMAL);
        }        
    }
}
