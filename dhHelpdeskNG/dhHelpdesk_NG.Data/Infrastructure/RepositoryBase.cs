namespace DH.Helpdesk.Dal.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
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
        
        protected RepositoryBase(
            IDatabaseFactory databaseFactory)
        {
            this.DatabaseFactory = databaseFactory;
            this._dbset = this.DataContext.Set<T>();
            this.initializeAfterCommitActions = new List<Action>();
        }

        protected void InitializeAfterCommit<T1, T2>(T1 dto, T2 entity)
            where T1 : INewBusinessModel
            where T2 : Entity
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
            IEnumerable<T> objects = this._dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
            {
                this._dbset.Attach(obj);
                this._dbset.Remove(obj);
            }
        }

        public virtual T GetById(int id)
        {
            return this._dbset.Find(id);
        }

        public virtual T GetById(string id)
        {
            return this._dbset.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this._dbset;
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return this._dbset.Where(where);
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return this._dbset.Where(where).AsNoTracking<T>().FirstOrDefault<T>();
        }
    }
}