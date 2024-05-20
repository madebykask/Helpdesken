using System.Collections.Generic;

namespace DH.Helpdesk.Dal.NewInfrastructure.Concrete
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;

    public sealed class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly IDbContext context;

        private readonly IDbSet<TEntity> dbset;

        public GenericRepository(IDbContext context)
        {
            this.context = context;
            this.dbset = context.Set<TEntity>();
        }

        public void Attach(TEntity entity)
        {
            this.dbset.Attach(entity);
        }

        public void Add(TEntity entity)
        {
            this.dbset.Add(entity);
        }

        public void Update(TEntity entity)
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

        public void DeleteById(int id)
        {
            TEntity entity = this.dbset.Find(id);
            this.dbset.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            this.dbset.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            ((DbSet<TEntity>) dbset).RemoveRange(entities);
        }

        public void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
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

        public TEntity GetById(long id)
        {
            return this.dbset.Find(id);
        }

        /// <summary>
        /// Applies new musltiselect list selection to an existing selection from repository - removes what needs to be removed, add what needs to be added
        /// </summary>
        /// <param name="currentPredicate">Predicate for existing repository to select a set of entries to be updated</param>
        /// <param name="newList">New selection, list of entries to what a repository needs to be updated to</param>
        /// /// <param name="comparePredicate">Comparer expression used to compare old and new entries</param>
        /// <returns></returns>
        public void MergeList(Expression<Func<TEntity, bool>> currentPredicate, IList<TEntity> newList, Func<TEntity, TEntity, bool> comparePredicate)
        {
            var current = GetAll().Where(currentPredicate).ToList();

            foreach (var toDel in current.Where(x => !newList.Any(y => comparePredicate(x, y))).ToList())
            {
                Delete(toDel);
            }
            foreach (var toIns in newList.Where(x => !current.Any(y => comparePredicate(x, y))).ToList())
            {
                Add(toIns);
            }
        }
        public void MergeList(IList<TEntity> current, IList<TEntity> newList, Func<TEntity, TEntity, bool> comparePredicate)
        {
            foreach (var toDel in current.Where(x => !newList.Any(y => comparePredicate(x, y))).ToList())
            {
                Delete(toDel);
            }
            foreach (var toIns in newList.Where(x => !current.Any(y => comparePredicate(x, y))).ToList())
            {
                Add(toIns);
            }
        }
    }
}