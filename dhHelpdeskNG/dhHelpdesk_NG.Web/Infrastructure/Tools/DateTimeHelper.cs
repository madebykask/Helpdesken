namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System;

    public static class DateTimeHelper
    {
        public static string ToMonthString(this DateTime date)
        {
            switch (date.Month)
            {
                case 1:
                    return Translation.Get("Januari");
                case 2:
                    return Translation.Get("Februari");
                case 3:
                    return Translation.Get("Mars");
                case 4:
                    return Translation.Get("April");
                case 5:
                    return Translation.Get("Maj");
                case 6:
                    return Translation.Get("Juni");
                case 7:
                    return Translation.Get("Juli");
                case 8:
                    return Translation.Get("Augusti");
                case 9:
                    return Translation.Get("September");
                case 10:
                    return Translation.Get("Oktober");
                case 11:
                    return Translation.Get("November");
                case 12:
                    return Translation.Get("December");
                default:
                    return string.Empty;
            }
        }

        public static string ToYearString(this DateTime date)
        {
            return date.ToString("yyyy");
        }
    }
}