using System;

namespace DH.Helpdesk.Common.Tools
{
    public static class EnumHelper
    {
        public static TEnum Parse<TEnum>(string val, bool ignoreCase = true) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), val, ignoreCase);
        }
    }
}