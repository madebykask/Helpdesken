using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace dhHelpdesk_NG.Data.Infrastructure
{
    using System.Data.Entity.Validation;

    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.Domain;

    public abstract class RepositoryBase<T> where T : class
    {
        private HelpdeskDbContext _dataContext;
        private readonly IDbSet<T> _dbset;

        private readonly List<Action> initializeAfterCommitActions; 
        
        protected RepositoryBase(
            IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbset = DataContext.Set<T>();
            this.initializeAfterCommitActions = new List<Action>();
        }

        protected void InitializeAfterCommit<T1, T2>(T1 dto, T2 entity) where T1 : INewEntity where T2 : Entity
        {
            var initializeAfterCommit = new Action(() => dto.Id = entity.Id);
            this.initializeAfterCommitActions.Add(initializeAfterCommit);
        }

        public void Commit()
        {
            this.DataContext.SaveChanges();

            foreach (var initializeAfterCommit in this.initializeAfterCommitActions)
            {
                initializeAfterCommit();
            }

            this.initializeAfterCommitActions.Clear();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected HelpdeskDbContext DataContext
        {
            get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); }
        }

        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public virtual void AddText(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Detached;
            _dbset.Add(entity);
            _dataContext.Entry(entity).State = EntityState.Added;

            //var entry = _dataContext.Entry(entity);

            //if (entry.State == EntityState.Detached)
            //{
            //    _dbset.Add(entity);
            //}
            //else
            //{
            //    entry.State = EntityState.Added;
            //}
        }

        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbset.Attach(entity);
            _dbset.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
            {
                _dbset.Attach(obj);
                _dbset.Remove(obj);
            }
        }

        public virtual T GetById(int id)
        {
            return _dbset.Find(id);
        }

        public virtual T GetById(string id)
        {
            return _dbset.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbset;
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where);
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).AsNoTracking<T>().FirstOrDefault<T>();
        }
    }
}