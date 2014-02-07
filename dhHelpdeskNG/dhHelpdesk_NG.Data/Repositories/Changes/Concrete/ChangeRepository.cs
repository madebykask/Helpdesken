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
    using DH.Helpdesk.Dal.Dal.Mappers;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeRepository : Repository, IChangeRepository
    {
        private readonly IEntityToBusinessModelMapper<ChangeEntity, Change> changeEntityToChangeMapper;

        private readonly IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity> updatedChangeToChangeEntityMapper;

        private readonly IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview>
            changeEntityToChangeDetailedOverviewMapper;

        private readonly INewBusinessModelToEntityMapper<NewChange, ChangeEntity> newChangeToChangeEntityMapper; 

        public ChangeRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<ChangeEntity, Change> changeEntityToChangeMapper, 
            IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity> updatedChangeToChangeEntityMapper, 
            IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview> changeEntityToChangeDetailedOverviewMapper, 
            INewBusinessModelToEntityMapper<NewChange, ChangeEntity> newChangeToChangeEntityMapper)
            : base(databaseFactory)
        {
            this.changeEntityToChangeMapper = changeEntityToChangeMapper;
            this.updatedChangeToChangeEntityMapper = updatedChangeToChangeEntityMapper;
            this.changeEntityToChangeDetailedOverviewMapper = changeEntityToChangeDetailedOverviewMapper;
            this.newChangeToChangeEntityMapper = newChangeToChangeEntityMapper;
        }

        public Change FindById(int changeId)
        {
            var change = this.DbContext.Changes.Find(changeId);
            return this.changeEntityToChangeMapper.Map(change);
        }
        
        public SearchResultDto SearchOverviews(SearchParameters parameters)
        {
            var searchRequest = this.DbContext.Changes.Where(c => c.Customer_Id == parameters.CustomerId);

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
                        c.ChangeBenefits.ToUpper().Contains(pharseInLowerCase)
                        || c.ChangeConsequence.ToLower().Contains(pharseInLowerCase)
                        || c.ChangeDescription.ToLower().Contains(pharseInLowerCase)
                        || c.ChangeDeviation.ToLower().Contains(pharseInLowerCase)
                        || c.ChangeEvaluation.ToLower().Contains(pharseInLowerCase)
                        || c.ChangeExplanation.ToLower().Contains(pharseInLowerCase)
                        || c.ChangeImpact.ToLower().Contains(pharseInLowerCase)
                        || c.ChangeRecommendation.ToLower().Contains(pharseInLowerCase)
                        || c.ChangeRisk.ToLower().Contains(pharseInLowerCase)
                        || c.ChangeSolution.ToLower().Contains(pharseInLowerCase)
                        || c.ChangeTitle.ToLower().Contains(pharseInLowerCase)
                        || c.Currency.ToLower().Contains(pharseInLowerCase)
                        || c.InventoryNumber.ToLower().Contains(pharseInLowerCase)
                        || c.OrdererCellPhone.ToLower().Contains(pharseInLowerCase)
                        || c.OrdererEMail.ToLower().Contains(pharseInLowerCase));
            }

            var changesFound = searchRequest.Count();
            searchRequest = searchRequest.Take(parameters.SelectCount);
            var changes = searchRequest.ToList();
            var overviews = new List<ChangeDetailedOverview>(changes.Count);
            overviews.AddRange(changes.Select(this.changeEntityToChangeDetailedOverviewMapper.Map));

            return new SearchResultDto(changesFound, overviews);
        }

        public IList<ChangeEntity> GetChanges(int customer)
        {
            return (from w in this.DbContext.Set<ChangeEntity>()
                    where w.Customer_Id == customer
                    select w).ToList();
        }

        public void DeleteById(int id)
        {
            var change = this.DbContext.Changes.Find(id);
            this.DbContext.Changes.Remove(change);
        }

        public void Update(UpdatedChange change)
        {
            var entity = this.FindByIdCore(change.Id);
            this.updatedChangeToChangeEntityMapper.Map(change, entity);
        }

        private ChangeEntity FindByIdCore(int id)
        {
            return this.DbContext.Changes.Find(id);
        }

        public List<ItemOverview> FindOverviewsExcludeChange(int customerId, int changeId)
        {
            var changes =
                this.DbContext.Changes.Where(c => c.Customer_Id == customerId && c.Id != changeId)
                    .Select(c => new { Id = c.Id, Title = c.ChangeTitle })
                    .ToList();

            return
                changes.Select(
                    c => new ItemOverview("#" + c.Id + " " + c.Title, c.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var changes =
                this.DbContext.Changes.Where(c => c.Customer_Id == customerId)
                    .Select(c => new { Id = c.Id, Title = c.ChangeTitle })
                    .ToList();

            return
                changes.Select(
                    c => new ItemOverview("#" + c.Id + " " + c.Title, c.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();
        }

        public void AddChange(NewChange change)
        {
            var entity = this.newChangeToChangeEntityMapper.Map(change);
            this.DbContext.Changes.Add(entity);
            this.InitializeAfterCommit(change, entity);
        }
    }
}
