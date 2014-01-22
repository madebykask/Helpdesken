namespace dhHelpdesk_NG.Data.Dal
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs;

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
            where TBusinessModel : IBusinessModelWithId
            where TEntity : Entity
        {
            var initializeAfterCommit = new Action(() => businessModel.Id = entity.Id);
            this.InitializeAfterCommitActions.Add(initializeAfterCommit);
        }

        #endregion
    }
}