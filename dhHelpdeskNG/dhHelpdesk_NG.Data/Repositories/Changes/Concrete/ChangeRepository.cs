namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeRepository : Repository, IChangeRepository
    {
        #region Fields

        private readonly IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview>
            changeEntityToChangeDetailedOverviewMapper;

        private readonly IEntityToBusinessModelMapper<ChangeEntity, Change> changeEntityToChangeMapper;

        private readonly INewBusinessModelToEntityMapper<NewChange, ChangeEntity> newChangeToChangeEntityMapper;

        private readonly IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity> updatedChangeToChangeEntityMapper;

        #endregion

        #region Constructors and Destructors

        public ChangeRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview>
                changeEntityToChangeDetailedOverviewMapper,
            IEntityToBusinessModelMapper<ChangeEntity, Change> changeEntityToChangeMapper,
            INewBusinessModelToEntityMapper<NewChange, ChangeEntity> newChangeToChangeEntityMapper,
            IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity> updatedChangeToChangeEntityMapper)
            : base(databaseFactory)
        {
            this.changeEntityToChangeDetailedOverviewMapper = changeEntityToChangeDetailedOverviewMapper;
            this.changeEntityToChangeMapper = changeEntityToChangeMapper;
            this.newChangeToChangeEntityMapper = newChangeToChangeEntityMapper;
            this.updatedChangeToChangeEntityMapper = updatedChangeToChangeEntityMapper;
        }

        #endregion

        #region Public Methods and Operators

        public void AddChange(NewChange change)
        {
            var entity = this.newChangeToChangeEntityMapper.Map(change);
            this.DbContext.Changes.Add(entity);
            this.InitializeAfterCommit(change, entity);
        }

        public void DeleteById(int changeId)
        {
            var change = this.DbContext.Changes.Find(changeId);
            this.DbContext.Changes.Remove(change);
        }

        public Change FindById(int changeId)
        {
            var change = this.DbContext.Changes.Find(changeId);
            return this.changeEntityToChangeMapper.Map(change);
        }

        public Change GetById(int changeId)
        {
            var change = this.DbContext.Changes.Single(c => c.Id == changeId);
            return this.changeEntityToChangeMapper.Map(change);
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var changes = this.FindByCustomerIdCore(customerId).Select(c => new { c.Id, c.ChangeTitle }).ToList();

            return
                changes.Select(c => new ItemOverview(c.ChangeTitle, c.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();
        }

        public List<ItemOverview> FindOverviewsExcludeSpecified(int customerId, int changeId)
        {
            var changes =
                this.DbContext.Changes.Where(c => c.Customer_Id == customerId && c.Id != changeId)
                    .Select(c => new { c.Id, c.ChangeTitle })
                    .ToList();

            return
                changes.Select(c => new ItemOverview(c.ChangeTitle, c.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();
        }

        public IList<ChangeEntity> GetChanges(int customer)
        {
            return (from w in this.DbContext.Set<ChangeEntity>() where w.Customer_Id == customer select w).ToList();
        }

        public SearchResult Search(SearchParameters parameters)
        {
            var searchRequest = this.FindByCustomerIdCore(parameters.CustomerId);

            switch (parameters.Status)
            {
                case ChangeStatus.Active:
                    searchRequest = searchRequest.Where(c => c.ChangeStatus.CompletionStatus == 0);
                    break;
                case ChangeStatus.Finished:
                    searchRequest = searchRequest.Where(c => c.ChangeStatus.CompletionStatus != 0);
                    break;
            }

            if (parameters.StatusIds.Any())
            {
                searchRequest = searchRequest.Where(c => parameters.StatusIds.Any(i => i == c.ChangeStatus_Id));
            }

            if (parameters.ObjectIds.Any())
            {
                searchRequest = searchRequest.Where(c => parameters.ObjectIds.Any(i => i == c.ChangeObject_Id));
            }

            if (parameters.OwnerIds.Any())
            {
                searchRequest = searchRequest.Where(c => parameters.OwnerIds.Any(i => i == c.ChangeGroup_Id));
            }

            if (parameters.AffectedProcessIds.Any())
            {
                searchRequest =
                    searchRequest.Where(
                        c =>
                            this.DbContext.ChangeChangeGroups.Where(cg => cg.Change_Id == c.Id)
                                .Any(cg => parameters.AffectedProcessIds.Contains(cg.ChangeGroup_Id)));
            }

            if (parameters.WorkingGroupIds.Any())
            {
                searchRequest = searchRequest.Where(c => parameters.WorkingGroupIds.Any(i => i == c.WorkingGroup_Id));
            }

            if (parameters.AdministratorIds.Any())
            {
                searchRequest = searchRequest.Where(c => parameters.AdministratorIds.Any(i => i == c.User_Id));
            }

            if (!string.IsNullOrEmpty(parameters.Pharse))
            {
                var pharseInLowerCase = parameters.Pharse.ToLower();

                searchRequest =
                    searchRequest.Where(
                        c =>
                        c.OrdererId.ToLower().Contains(pharseInLowerCase)
                        || c.OrdererName.ToLower().Contains(pharseInLowerCase)
                        || c.OrdererPhone.ToLower().Contains(pharseInLowerCase)
                        || c.OrdererCellPhone.ToLower().Contains(pharseInLowerCase)
                        || c.OrdererEMail.ToLower().Contains(pharseInLowerCase)
                        || (c.OrdererDepartment_Id.HasValue
                            && c.OrdererDepartment.DepartmentName.ToLower().Contains(pharseInLowerCase)));
            }

            var changesFound = searchRequest.Count();
            searchRequest = searchRequest.Take(parameters.SelectCount);
            var changes = searchRequest.ToList();
            var overviews = new List<ChangeDetailedOverview>(changes.Count);
            overviews.AddRange(changes.Select(this.changeEntityToChangeDetailedOverviewMapper.Map));

            return new SearchResult(changesFound, overviews);
        }

        public void Update(UpdatedChange change)
        {
            var entity = this.FindByIdCore(change.Id);
            this.updatedChangeToChangeEntityMapper.Map(change, entity);
        }

        public ChangeOverview GetChangeOverview(int id)
        {
            var entity = this.FindByIdCore(id);
            if (entity == null)
            {
                return null;
            }

            return new ChangeOverview()
                       {
                           Id = entity.Id,
                           ChangeTitle = entity.ChangeTitle
                       };
        }

        #endregion

        #region Methods

        private ChangeEntity FindByIdCore(int id)
        {
            return this.DbContext.Changes.Find(id);
        }

        private IQueryable<ChangeEntity> FindByCustomerIdCore(int customerId)
        {
            return this.DbContext.Changes.Where(c => c.Customer_Id == customerId);
        }

        #endregion
    }
}