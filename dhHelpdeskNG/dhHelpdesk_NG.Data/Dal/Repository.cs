namespace DH.Helpdesk.Dal.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Text;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
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
            try
            {
                this.DbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        sb.AppendLine(string.Format(
                                                "Property: {0}; Value: {1}; Error message: {2}",
                                                validationError.PropertyName,
                                                validationErrors.Entry.Property(validationError.PropertyName).CurrentValue,
                                                validationError.ErrorMessage));
                        global::System.Diagnostics.Trace.TraceInformation(
                            "Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }

                throw new Exception(sb.ToString());
            }

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

        public virtual void DeleteById(int id)
        {
            var entity = this.DbSet.Find(id);
            this.DbSet.Remove(entity);
        }
    }
}