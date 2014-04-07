using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Dal.Infrastructure.Security;

namespace DH.Helpdesk.Dal.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Dal.DbContext;
    using DH.Helpdesk.Domain;

    public abstract class RepositoryBase<T> where T : class
    {
        private HelpdeskDbContext _dataContext;
        private readonly IDbSet<T> _dbset;

        private readonly List<Action> initializeAfterCommitActions;
        private readonly IWorkContext _workContext;        

        protected RepositoryBase(
            IDatabaseFactory databaseFactory,
            IWorkContext workContext = null)
        {
            this.DatabaseFactory = databaseFactory;
            this._dbset = this.DataContext.Set<T>();
            this.initializeAfterCommitActions = new List<Action>();
            _workContext = workContext;
        }

        protected void InitializeAfterCommit<T1, T2>(T1 businessModel, T2 entity)
            where T1 : INewBusinessModel
            where T2 : Entity
        {
            var initializeAfterCommit = new Action(() => businessModel.Id = entity.Id);
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
            get { return this._dataContext ?? (this._dataContext = this.DatabaseFactory.Get()); }
        }

        public virtual void Add(T entity)
        {
            this._dbset.Add(entity);
        }

        public virtual void AddText(T entity)
        {
            this._dataContext.Entry(entity).State = EntityState.Detached;
            this._dbset.Add(entity);
            this._dataContext.Entry(entity).State = EntityState.Added;

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
            this._dbset.Attach(entity);
            this._dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            this._dbset.Attach(entity);
            this._dbset.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = GetSecuredEntities(_dbset.Where(where).AsEnumerable());
            foreach (T obj in objects)
            {
                this._dbset.Attach(obj);
                this._dbset.Remove(obj);
            }
        }

        public virtual T GetById(int id)
        {
            return GetSecuredEntities(new[] { _dbset.Find(id) }).FirstOrDefault();
        }

        public virtual T GetById(string id)
        {
            return GetSecuredEntities(new [] { _dbset.Find(id) }).FirstOrDefault();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return GetSecuredEntities();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return GetSecuredEntities().AsQueryable().Where(where);
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return GetSecuredEntities().AsQueryable().Where(where).AsNoTracking<T>().FirstOrDefault<T>();
        }

        protected virtual IEnumerable<T> GetSecuredEntities()
        {
            return _dbset.CheckAccess(_workContext);
        }

        private IEnumerable<T> GetSecuredEntities(IEnumerable<T> entities)
        {
            return entities.CheckAccess(_workContext);
        }
    }
}