using System;

namespace DH.Helpdesk.Common.Constants
{
    public static class NotChangedValue
    {
        public static int INT = int.MinValue + 1;

        public static int? NULLABLE_INT = int.MinValue + 1;

        public static decimal DECIMAL = int.MinValue + 1;

        public static string STRING = "{NOT_CHANGED}";

        public static DateTime DATETIME = new DateTime(1900, 1, 1, 0, 0, 0);

        public static DateTime? NULLABLE_DATETIME = new DateTime(1900, 1, 1, 0, 0, 0);        
    }
}
