namespace DH.Helpdesk.Dal.NewInfrastructure
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        void DeleteById(int id);

        TEntity GetById(long id);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    }
}
