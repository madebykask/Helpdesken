using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IntegerExtensions
    {
        public static string TranslateBit(this int i)
        {
            if (i == 1)
                return Translation.Get("Ja", Enums.TranslationSource.TextTranslation);
            return Translation.Get("Nej", Enums.TranslationSource.TextTranslation);
        }

        public static string TranslateOperationObjectBit(this int i)
        {
            if (i > 0)
                return Translation.Get("Ja", Enums.TranslationSource.TextTranslation);
            return Translation.Get("Nej", Enums.TranslationSource.TextTranslation);
        }

        public static string GetClassforYesNo(this int i)
        {
            if (i == 1)
                return "label-success";
            return "";
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

        public static List<int> ToIntList(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new List<int>();
            }

            var arr = str.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            return arr.Select(int.Parse).ToList();
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
            if (value == 0)
                ret = Translation.Get("Nej", Enums.TranslationSource.TextTranslation);
            else
                ret = Translation.Get("Ja", Enums.TranslationSource.TextTranslation);

            return ret;

        }

        /// <summary>
        /// The boolean to yes no.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string BoolToYesNo(this bool value)
        {
            return value ? Translation.Get("Ja") : Translation.Get("Nej"); 
        }
        public static string GetClassforBoolToYesNoYesNo(this bool value)
        {
            return value ? "label-success" : "";
        }

        public static string GetLanguageIconFileName(this int value)
        {
            string ret = string.Empty;
            switch (value)
            {
                case LanguageIds.Swedish:
                    ret = "Swedish.png";
                    break;
                case LanguageIds.English:
                    ret = "English.png";
                    break;
                default:
                    ret = "";
                    break;                
            }
            return ret;
        }

        public static bool IsCustomerOrSystemAdminRole(this int roleId)
        {
            return (roleId >= (int)BusinessData.Enums.Admin.Users.UserGroup.CustomerAdministrator);
            
        }
    }
}