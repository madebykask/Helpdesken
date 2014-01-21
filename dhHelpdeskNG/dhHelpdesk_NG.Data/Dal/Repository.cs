namespace dhHelpdesk_NG.Data.Dal
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs;

    public abstract class Repository<TEntity, TNewBusinessModel, TUpdatedBusinessModel> :
        INewRepository<TNewBusinessModel, TUpdatedBusinessModel>
        where TEntity : Entity
        where TNewBusinessModel : class, IBusinessModelWithId
        where TUpdatedBusinessModel : class, IBusinessModelWithId
    {
        #region Fields

        private readonly List<Action> initializeAfterCommitActions;

        private readonly IBusinessModelToEntityMapper<TNewBusinessModel, TEntity> newModelMapper;

        private readonly IEntityChangerFromBusinessModel<TUpdatedBusinessModel, TEntity> updatedModelMapper;

        #endregion

        #region Constructors and Destructors

        protected Repository(
            IDatabaseFactory databaseFactory,
            IBusinessModelToEntityMapper<TNewBusinessModel, TEntity> newModelMapper,
            IEntityChangerFromBusinessModel<TUpdatedBusinessModel, TEntity> updatedModelMapper)
        {
            this.DbContext = databaseFactory.Get();
            this.newModelMapper = newModelMapper;
            this.updatedModelMapper = updatedModelMapper;
            this.initializeAfterCommitActions = new List<Action>();
        }

        #endregion

        #region Properties

        protected HelpdeskDbContext DbContext { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void Add(TNewBusinessModel businessModel)
        {
            var entity = this.newModelMapper.Map(businessModel);
            this.InitializeAfterCommit(businessModel, entity);
            this.DbContext.Set<TEntity>().Add(entity);
        }

        public void Commit()
        {
            this.DbContext.SaveChanges();

            foreach (var initializeAfterCommit in this.initializeAfterCommitActions)
            {
                initializeAfterCommit();
            }

            this.initializeAfterCommitActions.Clear();
        }

        public void DeleteById(int id)
        {
            var entity = this.FindById(id);
            this.DbContext.Set<TEntity>().Remove(entity);
        }

        public void Update(TUpdatedBusinessModel businessModel)
        {
            var entity = this.FindById(businessModel.Id);
            this.updatedModelMapper.Map(businessModel, entity);
        }

        #endregion

        #region Methods

        protected void InitializeAfterCommit<TBusinessModel, TEntity>(TBusinessModel businessModel, TEntity entity)
            where TBusinessModel : IBusinessModelWithId where TEntity : Entity
        {
            var initializeAfterCommit = new Action(() => businessModel.Id = entity.Id);
            this.initializeAfterCommitActions.Add(initializeAfterCommit);
        }

        private TEntity FindById(int id)
        {
            return this.DbContext.Set<TEntity>().Find(id);
        }

        #endregion
    }
}