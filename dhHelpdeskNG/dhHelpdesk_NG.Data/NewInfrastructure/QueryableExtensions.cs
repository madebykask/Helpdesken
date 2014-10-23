namespace DH.Helpdesk.Dal.NewInfrastructure
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludePath<T, TProperty>(
                                            this IQueryable<T> source,
                                            Expression<Func<T, TProperty>> path)
        {
            return source.Include(path);
        }
    }
}