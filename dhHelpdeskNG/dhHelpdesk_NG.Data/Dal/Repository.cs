namespace DH.Helpdesk.Dal.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Dal.DbContext;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public abstract class Repository : INewRepository
    {
        #region Fields

        #endregion

        #region Constructors and Destructors

        protected Repository(IDatabaseFactory databaseFactory)
        {
            this.DbContext = databaseFactory.Get();
            this.InitializeAfterCommitActions = new List<Action>();
        }

        #endregion

        #region Properties

        protected HelpdeskDbContext DbContext { get; private set; }

        protected List<Action> InitializeAfterCommitActions { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void Commit()
        {
            this.DbContext.SaveChanges();

            foreach (var initializeAfterCommit in this.InitializeAfterCommitActions)
            {
                initializeAfterCommit();
            }

            this.InitializeAfterCommitActions.Clear();
        }

        #endregion

        #region Methods

        protected void InitializeAfterCommit<TBusinessModel, TEntity>(TBusinessModel businessModel, TEntity entity)
            where TBusinessModel : INewBusinessModel
            where TEntity : Entity
        {
            var initializeAfterCommit = new Action(() => businessModel.Id = entity.Id);
            this.InitializeAfterCommitActions.Add(initializeAfterCommit);
        }

        #endregion
    }

    public abstract class Repository<TEntity> : Repository
        where TEntity : class
    {
        protected DbSet<TEntity> DbSet { get; private set; }

        protected Repository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            this.DbSet = this.DbContext.Set<TEntity>();
        }
    }
}