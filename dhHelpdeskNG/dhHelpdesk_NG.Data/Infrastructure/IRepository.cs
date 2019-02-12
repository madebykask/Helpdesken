using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    

    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void AddText(T entity); //what is that?!
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(int Id);
        Task<T> GetByIdAsync(int id);
        T GetById(string Id);
        T Get(Expression<Func<T, bool>> where);
        Task<T> GetAsync(Expression<Func<T, bool>> where);
        TResult Get<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

       

        void Commit();
    }
}
