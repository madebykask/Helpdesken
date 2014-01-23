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
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Data.Infrastructure;

    public sealed class ChangeRepository : RepositoryBase<ChangeEntity>, IChangeRepository
    {
        private readonly IEntityToBusinessModelMapper<ChangeEntity, Change> changeEntityToChangeMapper;

        public ChangeRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<ChangeEntity, Change> changeEntityToChangeMapper)
            : base(databaseFactory)
        {
            this.changeEntityToChangeMapper = changeEntityToChangeMapper;
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

            if (status == Enums.Changes.ChangeStatus.Active)
            {
                // Filter by status
            }
            else if (status == Enums.Changes.ChangeStatus.Finished)
            {
                // Filter by status
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
                // Filter by administrator ids
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
            var overviews = new List<ChangeDetailedOverviewDto>(changes.Count);
            overviews.AddRange(changes.Select(CreateChangeDetailedOverview));

            return new SearchResultDto(changesFound, overviews);
        }

        private static ChangeDetailedOverviewDto CreateChangeDetailedOverview(ChangeEntity change)
        {
            var ordererFields = CreateOrdererFieldGroup(change);
            var generalFields = CreateGeneralFieldGroup(change);
            var registrationFields = CreateRegistrationFieldGroup(change);
            var analyzeFields = CreateAnalyzeFieldGroup(change);
            var implementationFields = CreateImplementationFieldGroup(change);
            var evaluationFields = CreateEvaluationFieldGroup(change);

            return new ChangeDetailedOverviewDto(
                change.Id,
                ordererFields,
                generalFields,
                registrationFields,
                analyzeFields,
                implementationFields,
                evaluationFields);
        }

        private static OrdererFieldGroupDto CreateOrdererFieldGroup(ChangeEntity change)
        {
            return new OrdererFieldGroupDto(
                change.OrdererId,
                change.OrdererName,
                change.OrdererPhone,
                change.OrdererCellPhone,
                change.OrdererEMail,
                change.OrdererDepartment == null ? string.Empty : change.OrdererDepartment.DepartmentName);
        }

        private static GeneralFieldGroupDto CreateGeneralFieldGroup(ChangeEntity change)
        {
            return new GeneralFieldGroupDto(
                change.Prioritisation,
                change.ChangeTitle,
                change.ChangeStatus != null ? change.ChangeStatus.Name : string.Empty,
                change.System != null ? change.System.SystemName : string.Empty,
                change.ChangeObject != null ? change.ChangeObject.Name : string.Empty,
                change.InventoryNumber,
                string.Empty,
                change.WorkingGroup != null ? change.WorkingGroup.WorkingGroupName : string.Empty,
                string.Empty,
                change.FinishingDate,
                change.RSS != 0);
        }

        private static RegistrationFieldGroupDto CreateRegistrationFieldGroup(ChangeEntity change)
        {
            return new RegistrationFieldGroupDto(
                change.ChangeDescription,
                change.ChangeBenefits,
                change.ChangeConsequence,
                change.ChangeImpact,
                change.DesiredDate,
                change.Verified != 0,
                change.Approval != 0,
                change.ChangeExplanation);
        }

        private static AnalyzeFieldGroupDto CreateAnalyzeFieldGroup(ChangeEntity change)
        {
            return new AnalyzeFieldGroupDto(
                change.ChangeCategory != null ? change.ChangeCategory.Name : string.Empty,
                change.ChangePriority != null ? change.ChangePriority.Name : string.Empty,
                change.ResponsibleUser != null ? change.ResponsibleUser.UserID : string.Empty,
                change.ChangeSolution,
                change.TotalCost,
                change.YearlyCost,
                change.TimeEstimatesHours,
                change.ChangeRisk,
                change.ScheduledStartTime,
                change.ScheduledEndTime,
                change.ImplementationPlan != 0,
                change.RecoveryPlan != 0,
                change.ChangeRecommendation,
                (AnalyzeResult)change.AnalysisApproval);
        }

        private static ImplementationFieldGroupDto CreateImplementationFieldGroup(ChangeEntity change)
        {
            return new ImplementationFieldGroupDto(
                change.ImplementationStatus != null ? change.ImplementationStatus.Name : string.Empty,
                change.RealStartDate,
                change.BuildImplemented != 0,
                change.ImplementationPlanUsed != 0,
                change.ChangeDeviation,
                change.RecoveryPlanUsed != 0,
                change.FinishingDate,
                change.ImplementationReady != 0);
        }

        private static EvaluationFieldGroupDto CreateEvaluationFieldGroup(ChangeEntity change)
        {
            return new EvaluationFieldGroupDto(
                change.ChangeEvaluation,
                change.EvaluationReady != 0);
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
            // map
        }

        private ChangeEntity FindByIdCore(int id)
        {
            // INewBusinessModelToEntityMapper
            // IUpdatedBusinessModelToEntityMapper
            return this.DataContext.Changes.Find(id);
        }
    }
}
