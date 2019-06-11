using System;
using System.Linq;
using System.Linq.Expressions;

namespace DH.Helpdesk.Common.Linq
{
    public static class DynamicSortingExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool isDesc = false, bool anotherLevel = false)
        {
            var param = Expression.Parameter(typeof(T), string.Empty);
            var property = Expression.PropertyOrField(param, propertyName);
            var sortExp = Expression.Lambda(property, param);

            var call = Expression.Call(
                typeof(Queryable), 
                (!anotherLevel ? "OrderBy" : "ThenBy") + (isDesc ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sortExp));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }
    }
}