using System;
using System.Collections.Generic;

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
    }
}