using System;
using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Common.Extensions.GUID
{   
    public static class GuidExtension
    {
        public static bool IsValueChanged(this Guid value)
        {
            return (value != NotChangedValue.GUID);
        }

        public static Guid? IfNullThenElse(this Guid? value, Guid? elseValue)
        {
            return value ?? elseValue;
        }
    }
}
