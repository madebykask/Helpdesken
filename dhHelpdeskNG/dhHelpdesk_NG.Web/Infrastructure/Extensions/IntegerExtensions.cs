﻿using System;

namespace dhHelpdesk_NG.Web.Infrastructure.Extensions
{
    public static class IntegerExtensions
    {
        public static string TranslateBit(this int i)
        {
            if (i == 1)
                return Translation.Get("Ja", Enums.TranslationSource.TextTranslation);
            return Translation.Get("Nej", Enums.TranslationSource.TextTranslation);
        }

        public static string RoundQty(this int quantity)
        {
            return Math.Round(quantity / 1024.0, 0, MidpointRounding.AwayFromZero).ToString() + " kb";
        }

        public static int convertStringToInt(this string value)
        {
            int ret;
            if (!int.TryParse(value, out ret))
                ret = 0;
            return ret;
        }

        public static string supressZero(this int value)
        {
            if (value == 0)
                return string.Empty;

            return value.ToString(); 
        }

        public static string IntToYES_NO(this int value)
        {
            var ret = string.Empty;
            if (value==0)
                ret = Translation.Get("Nej", Enums.TranslationSource.TextTranslation);
            else
                ret = Translation.Get("Ja", Enums.TranslationSource.TextTranslation);

            return ret;

        }
    }
}