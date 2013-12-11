using System;

namespace dhHelpdesk_NG.Web.Infrastructure.Extensions
{
    public static class DateExtensions
    {

        public static string FormatDate(this DateTime d)
        {
            var ret = string.Empty;

            if (d != DateTime.MinValue)  
                ret = d.ToShortDateString();

            return ret;
        }

        public static string GetDateFormat()
        {
            return "yyyy-mm-dd";
        }

    }
}