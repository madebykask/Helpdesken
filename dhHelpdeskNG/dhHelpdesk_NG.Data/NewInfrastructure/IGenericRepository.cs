using System.Collections.Generic;

namespace DH.Helpdesk.Dal.NewInfrastructure
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<TEntity>
        where TEntity : class
    {
        void Attach(TEntity entity);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] excludedProperties);

        void DeleteById(int id);

        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);

        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);

        TEntity GetById(long id);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        void MergeList(Expression<Func<TEntity, bool>> currentPredicate, IList<TEntity> newList,
            Func<TEntity, TEntity, bool> comparePredicate);
        void MergeList(IList<TEntity> current, IList<TEntity> newList, Func<TEntity, TEntity, bool> comparePredicate);
    }
}
