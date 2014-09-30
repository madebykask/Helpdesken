namespace DH.Helpdesk.Dal.NewInfrastructure.Concrete
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
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
            var entry = this.context.Entry(entity);
            this.dbset.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public virtual void DeleteById(int id)
        {
            var entity = this.dbset.Find(id);
            this.dbset.Remove(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.dbset;
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbset.Where(predicate);
        }

        public virtual TEntity GetById(long id)
        {
            return this.dbset.Find(id);
        }
    }
}