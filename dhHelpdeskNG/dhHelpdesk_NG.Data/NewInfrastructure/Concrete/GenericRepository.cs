namespace DH.Helpdesk.Dal.NewInfrastructure.Concrete
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;

    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly IDbContext context;

        private readonly IDbSet<TEntity> dbset;

        public GenericRepository(IDbContext context)
        {
            this.context = context;
            this.dbset = context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            this.dbset.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            DbEntityEntry<TEntity> entry = this.context.Entry(entity);
            this.dbset.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] excludedProperties)
        {
            DbEntityEntry<TEntity> entry = this.context.Entry(entity);
            this.dbset.Attach(entity);
            entry.State = EntityState.Modified;

            foreach (Expression<Func<TEntity, object>> property in excludedProperties)
            {
                entry.Property(property).IsModified = false;
            }
        }

        public virtual void DeleteById(int id)
        {
            TEntity entity = this.dbset.Find(id);
            this.dbset.Remove(entity);
        }

        public virtual void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> delList = this.dbset.Where(predicate);

            foreach (var entity in delList)
            {
                DbEntityEntry<TEntity> entry = this.context.Entry(entity);
                entry.State = EntityState.Deleted;
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.dbset;
        }

        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this.dbset;

            return includeProperties.Aggregate(query, (current, property) => current.Include(property));
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbset.Where(predicate);
        }

        public IQueryable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this.dbset.Where(predicate);

            return includeProperties.Aggregate(query, (current, property) => current.Include(property));
        }

        public virtual TEntity GetById(long id)
        {
            return this.dbset.Find(id);
        }
    }
}