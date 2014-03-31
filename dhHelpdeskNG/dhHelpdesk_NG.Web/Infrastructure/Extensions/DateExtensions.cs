namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using System;

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
            return "yyyy-MM-dd";
        }

        public static string ToFormattedDate(this DateTime date)
        {
            return date.ToString("dd-MM-yyyy");
        }

        public static string ToFormattedDateReverse(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static string ToFormattedDateTime(this DateTime date)
        {
            return date.ToString("dd-MM-yyyy H:mm:ss");
        }

        public static string ToFormattedDateTimeReverse(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd H:mm:ss");
        }
    }
}