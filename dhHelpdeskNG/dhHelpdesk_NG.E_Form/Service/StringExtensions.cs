using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.EForm.Service
{
    public static class StringExtensions
    {
        public static string ExceptBlanks(this string str)
        {
            var sb = new StringBuilder(str.Length);
            for(int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if(!char.IsWhiteSpace(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
