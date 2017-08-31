namespace DH.Helpdesk.Dal.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    

    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void AddText(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(int Id);
        T GetById(string Id);
        T Get(Expression<Func<T, bool>> where);
        TResult Get<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

       

        void Commit();
    }
}
