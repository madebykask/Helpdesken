using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Other
{
    public static class Extension
    {

        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static string IntNullCheck(this string value)
        {
            if (value == null)
            {
                return "0";
            }
            else
            {
                return value;
            }
        }

        public static string StringNullCheck(this string value)
        {
            if (value == null)
            {
                return "";
            }
            else
            {
                return value;
            }
        }

    }
}
