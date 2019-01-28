using System.Text.RegularExpressions;

namespace DH.Helpdesk.Web.Common.Extensions
{

    public static class StringExtensions
    {

        public static string ReturnCustomerUserValue(this string valueToReturn)
        {
            var ret = string.Empty;

            if (!string.IsNullOrWhiteSpace(valueToReturn))  
                if (valueToReturn != "0")   
                    ret = valueToReturn; 

            return ret;
        }

        public static string RemoveHtmlTags(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return Regex.Replace(value, @"<[^>]*>", "<HTMLTAG>");
        }
    }
}