namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Change;
    using dhHelpdesk_NG.DTO.DTOs.Changes.ChangeDetailedOverview;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data;
    using dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange;
    using dhHelpdesk_NG.Data.Infrastructure;

    public sealed class ChangeRepository : RepositoryBase<ChangeEntity>, IChangeRepository
    {
        private readonly IEntityToBusinessModelMapper<ChangeEntity, Change> changeEntityToChangeMapper;

        private readonly IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity> updatedChangeToChangeEntityMapper;

        private readonly IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview>
            changeEntityToChangeDetailedOverviewMapper; 

        public ChangeRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<ChangeEntity, Change> changeEntityToChangeMapper, 
            IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity> updatedChangeToChangeEntityMapper, 
            IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview> changeEntityToChangeDetailedOverviewMapper)
            : base(databaseFactory)
        {
            this.changeEntityToChangeMapper = changeEntityToChangeMapper;
            this.updatedChangeToChangeEntityMapper = updatedChangeToChangeEntityMapper;
            this.changeEntityToChangeDetailedOverviewMapper = changeEntityToChangeDetailedOverviewMapper;
        }

        public Change FindById(int changeId)
        {
            var change = this.DataContext.Changes.Find(changeId);
            return this.changeEntityToChangeMapper.Map(change);
        }
        
        public SearchResultDto SearchOverviews(
            int customerId,
            List<int> statusIds,
            List<int> objectIds,
            List<int> ownerIds,
            List<int> workingGroupIds,
            List<int> administratorIds,
            string pharse,
            Enums.Changes.ChangeStatus status,
            int selectCount)
        {
            var searchRequest = this.DataContext.Changes.Where(c => c.Customer_Id == customerId);

            switch (status)
            {
                case Enums.Changes.ChangeStatus.Active:
                    searchRequest = searchRequest.Where(c => c.ChangeStatus.CompletionStatus == 0);
                    break;
                case Enums.Changes.ChangeStatus.Finished:
                    searchRequest = searchRequest.Where(c => c.ChangeStatus.CompletionStatus != 0);
                    break;
            }

            if (statusIds.Any())
            {
                searchRequest = searchRequest.Where(c => statusIds.Any(i => i == c.ChangeStatus_Id));
            }

            if (objectIds.Any())
            {
                searchRequest = searchRequest.Where(c => objectIds.Any(i => i == c.ChangeObject_Id));
            }

            if (ownerIds.Any())
            {
                searchRequest = searchRequest.Where(c => ownerIds.Any(i => i == c.ChangeGroup_Id));
            }

            if (workingGroupIds.Any())
            {
                searchRequest = searchRequest.Where(c => workingGroupIds.Any(i => i == c.WorkingGroup_Id));
            }

            if (administratorIds.Any())
            {
                searchRequest = searchRequest.Where(c => administratorIds.Any(i => i == c.User_Id));
            }

            if (!string.IsNullOrEmpty(pharse))
            {
                var pharseInLowerCase = pharse.ToLower();

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
            searchRequest = searchRequest.Take(selectCount);
            var changes = searchRequest.ToList();
            var overviews = new List<ChangeDetailedOverview>(changes.Count);
            overviews.AddRange(changes.Select(this.changeEntityToChangeDetailedOverviewMapper.Map));

            return new SearchResultDto(changesFound, overviews);
        }

        public IList<ChangeEntity> GetChanges(int customer)
        {
            return (from w in this.DataContext.Set<ChangeEntity>()
                    where w.Customer_Id == customer
                    select w).ToList();
        }

        public void DeleteById(int id)
        {
            var change = this.DataContext.Changes.Find(id);
            this.DataContext.Changes.Remove(change);
        }

        public void Update(UpdatedChange change)
        {
            var entity = this.FindByIdCore(change.Id);
            this.updatedChangeToChangeEntityMapper.Map(change, entity);
        }

        private ChangeEntity FindByIdCore(int id)
        {
            return this.DataContext.Changes.Find(id);
        }
    }
}
