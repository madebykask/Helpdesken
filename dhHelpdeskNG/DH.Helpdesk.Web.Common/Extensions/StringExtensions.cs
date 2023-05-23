using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DH.Helpdesk.Web.Common.Extensions
{

    public static class StringExtensions
    {

        public static string ReturnCustomerUserValue(this string valueToReturn)
        {
            var ret = string.Empty;

            if (!string.IsNullOrWhiteSpace(valueToReturn) && valueToReturn != "0")  
                ret = valueToReturn;

            return ret;
        }

        public static string RemoveHtmlTags(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return Regex.Replace(value, @"<[^>]*>", "").Replace("&nbsp;", "\u0020");
        }
        public static string RemoveAllUnnecessaryTags(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            return Regex.Replace(value, @"<(?!\/?(p|br)(?=>|\s.*>))\/?.*?>", "");
        }

        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

        public static string AddCharacterInParts(this string s, int partLength, string charToSearch, string replaceStr)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            if (s.Length <= partLength)
                return s;

            var newStr = "";
            var arryStr = s.Split(new string[] { replaceStr }, StringSplitOptions.None);
            foreach (var t in arryStr)
            {
                if (t.Length > partLength)
                {
                    var splitedStr = t.SplitInParts(partLength).ToList();
                    var linesStr = "";
                    var lastCarryPart = "";
                    splitedStr.Add(" ");
                    splitedStr.Add(" ");
                    foreach (var part in splitedStr)
                    {
                        var partProcess = lastCarryPart + part;
                        lastCarryPart = "";
                        if (!partProcess.Contains(charToSearch) && partProcess.Length > partLength - 1)
                        {
                            var _splitedStr = partProcess.SplitInParts(partLength);
                            var newLongStr = _splitedStr.First();

                            var extraStr = "";
                            var replaceIndex = 0;
                            replaceIndex = newLongStr.LastIndexOf(" ") > replaceIndex ? newLongStr.LastIndexOf(" ") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf(".") > replaceIndex ? newLongStr.LastIndexOf(".") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf(",") > replaceIndex ? newLongStr.LastIndexOf(",") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf(";") > replaceIndex ? newLongStr.LastIndexOf(";") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf("?") > replaceIndex ? newLongStr.LastIndexOf("?") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf("!") > replaceIndex ? newLongStr.LastIndexOf("!") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf("(") > replaceIndex ? newLongStr.LastIndexOf("(") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf(")") > replaceIndex ? newLongStr.LastIndexOf(")") : replaceIndex;
                            replaceIndex = newLongStr.LastIndexOf("-") > replaceIndex ? newLongStr.LastIndexOf("-") : replaceIndex;

                            var isFirst = true;
                            foreach (var _part in _splitedStr)
                                if (isFirst)
                                    isFirst = false;
                                else
                                    lastCarryPart += _part;

                            var _str = string.Empty;
                            if (replaceIndex == 0)
                            {
                                extraStr = "-";
                                _str = newLongStr + extraStr;
                                linesStr += _str + replaceStr;
                            }
                            else
                            {
                                var baseIndex = replaceIndex + 1;
                                _str = newLongStr.Substring(0, baseIndex);
                                lastCarryPart = newLongStr.Substring(baseIndex) + lastCarryPart;
                                linesStr += _str.Insert(baseIndex, replaceStr) + extraStr;
                            }
                        }
                        else
                            linesStr += partProcess;
                    }
                    newStr = newStr + linesStr + lastCarryPart + replaceStr; // Added 2 replaceStr because split removes them
                }
                else
                {
                    newStr = newStr + t + replaceStr; // Added 2 replaceStr because split removes them
                }
            }

            return newStr;
        }

        public static string HtmlEncode(this string src)
        {
            return HttpUtility.HtmlEncode(src);
        }
    }
}