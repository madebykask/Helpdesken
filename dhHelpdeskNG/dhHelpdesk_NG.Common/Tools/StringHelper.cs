using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Common.Tools
{
    public static class StringHelper
    {
        public static string HandleSwedishChars(string input)
        {
            return input.Replace('å', 'a').Replace('Å', 'A').Replace('ö', 'o').Replace('Ö', 'O').Replace('ä', 'a').Replace('Ä', 'A');
        }
    }
}
