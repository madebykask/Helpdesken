﻿namespace DH.Helpdesk.NewSelfService.Infrastructure.Extensions
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

    }
}