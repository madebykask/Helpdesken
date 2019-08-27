using System;
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Common.Extensions.Lists
{
    public static class ListExtensions
    {
        public static IList<T> Apply<T>(this IList<T> source, Action<T> action)
        {
            foreach (var e in source)
            {
                action(e);
            }
            return source;
        }

        public static IList<string> ToDistintList(this IList<string> items, bool makeLowerCase = false)
        {
            if (items == null)
                return items;

            return items.Where(t => !string.IsNullOrWhiteSpace(t)).Select(x => makeLowerCase ? x.Trim().ToLower() : x.Trim()).Distinct().ToList();
        }

        public static string JoinToString<T>(this IEnumerable<T> items, string sep = ",")
        {
            var res = string.Empty;

            if (items == null)
                return res;

            res = string.Join(sep, items);
            return res;
        }
    }
}