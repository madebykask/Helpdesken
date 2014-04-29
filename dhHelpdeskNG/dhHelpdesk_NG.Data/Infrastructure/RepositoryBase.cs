// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the RepositoryBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Dal.DbContext;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.Infrastructure.Security;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The repository base.
    /// </summary>
    /// <typeparam name="T">
    /// entity type
    /// </typeparam>
    public abstract class RepositoryBase<T> where T : class
    {
        /// <summary>
        /// The work context.
        /// </summary>
        protected readonly IWorkContext WorkContext;

        /// <summary>
        /// The table.
        /// </summary>
        private readonly IDbSet<T> dbset;

        /// <summary>
        /// The initialize after commit actions.
        /// </summary>
        private readonly List<Action> initializeAfterCommitActions;

        /// <summary>
        /// The _data context.
        /// </summary>
        private HelpdeskDbContext dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        /// <param name="workContext">
        /// The work context.
        /// </param>
        protected RepositoryBase(
            IDatabaseFactory databaseFactory,
            IWorkContext workContext = null)
        {
            this.DatabaseFactory = databaseFactory;
            this.dbset = this.DataContext.Set<T>();
            this.initializeAfterCommitActions = new List<Action>();
            this.WorkContext = workContext;
        }

        /// <summary>
        /// Gets the database factory.
        /// </summary>
        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the data context.
        /// </summary>
        protected HelpdeskDbContext DataContext
        {
            get { return this.dataContext ?? (this.dataContext = this.DatabaseFactory.Get()); }
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        protected IDbSet<T> Table
        {
            get
            {
                return this.dbset;
            }
        }

        /// <summary>
        /// The commit.
        /// </summary>
        public void Commit()
        {
            this.DataContext.SaveChanges();

            foreach (var initializeAfterCommit in this.initializeAfterCommitActions)
            {
                initializeAfterCommit();
            }

            this.initializeAfterCommitActions.Clear();
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public virtual void Add(T entity)
        {
            this.dbset.Add(entity);
        }

        /// <summary>
        /// The add text.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public virtual void AddText(T entity)
        {
            this.dataContext.Entry(entity).State = EntityState.Detached;
            this.dbset.Add(entity);
            this.dataContext.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public virtual void Update(T entity)
        {
            this.dbset.Attach(entity);
            this.dataContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public virtual void Delete(T entity)
        {
            this.dbset.Attach(entity);
            this.dbset.Remove(entity);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = this.GetSecuredEntities(this.dbset.Where(where).AsEnumerable());
            foreach (T obj in objects)
            {
                this.dbset.Attach(obj);
                this.dbset.Remove(obj);
            }
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public virtual T GetById(int id)
        {
            return this.GetSecuredEntities(new[] { this.dbset.Find(id) }).FirstOrDefault();
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public virtual T GetById(string id)
        {
            return this.GetSecuredEntities(new[] { this.dbset.Find(id) }).FirstOrDefault();
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public virtual IEnumerable<T> GetAll()
        {
            return this.GetSecuredEntities(this.dbset);
        }

        /// <summary>
        /// The get many.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return this.GetSecuredEntities(this.dbset.Where(where));
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="where">
        /// The where.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return this.GetSecuredEntities(this.dbset.Where(where).AsNoTracking()).FirstOrDefault();
        }

        /// <summary>
        /// The initialize after commit.
        /// </summary>
        /// <param name="businessModel">
        /// The business model.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <typeparam name="T1">
        /// first type
        /// </typeparam>
        /// <typeparam name="T2">
        /// second type
        /// </typeparam>
        protected void InitializeAfterCommit<T1, T2>(T1 businessModel, T2 entity)
            where T1 : INewBusinessModel
            where T2 : Entity
        {
            var initializeAfterCommit = new Action(() => businessModel.Id = entity.Id);
            this.initializeAfterCommitActions.Add(initializeAfterCommit);
        }

        /// <summary>
        /// The get secured entities.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        /// <typeparam name="TEntity">
        /// entity type
        /// </typeparam>
        /// <returns>
        /// The result.
        /// </returns>
        protected IEnumerable<TEntity> GetSecuredEntities<TEntity>(IEnumerable<TEntity> entities)
        {
            return entities.CheckAccess(this.WorkContext);
        }
    }
}