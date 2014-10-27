namespace DH.Helpdesk.Dal.NewInfrastructure
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<TEntity>
        where TEntity : class
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        void DeleteById(int id);

        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);

        TEntity GetById(long id);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
