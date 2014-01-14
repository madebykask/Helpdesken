namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Changes;

    public sealed class ChangeRepository : RepositoryBase<Change>, IChangeRepository
    {
        public ChangeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public SearchResultDto SearchOverviews(
            int customerId,
            List<int> statusIds,
            List<int> objectIds,
            List<int> ownerIds,
            List<int> processAffectedIds,
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
                // Filter by owner id
            }

            if (processAffectedIds.Any())
            {
                // Filter by process affected id
            }
            
            if (workingGroupIds.Any())
            {
                searchRequest = searchRequest.Where(c => workingGroupIds.Any(i => i == c.WorkingGroup_Id));
            }

            if (administratorIds.Any())
            {
                // Filter by administrator id
            }

            if (!string.IsNullOrEmpty(pharse))
            {
                var pharseInLowerCase = pharse.ToLower();

                searchRequest =
                    searchRequest.Where(
                        c =>
                        c.ChangeBenefits.ToLower().Contains(pharseInLowerCase)
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

        private static ChangeDetailedOverviewDto CreateChangeDetailedOverview(Change change)
        {
            var ordererFields = CreateOrdererFieldGroup(change);
            var generalFields = CreateGeneralFieldGroup(change);
            var registrationFields = CreateRegistrationFieldGroup(change);
            var analyzeFields = CreateAnalyzeFieldGroup(change);
            var implementationFields = CreateImplementationFieldGroup(change);
            var evaluationFields = CreateEvaluationFieldGroup(change);

            return new ChangeDetailedOverviewDto(
                ordererFields, generalFields, registrationFields, analyzeFields, implementationFields, evaluationFields);
        }

        private static OrdererFieldGroupDto CreateOrdererFieldGroup(Change change)
        {
            return new OrdererFieldGroupDto(
                change.OrdererId,
                change.OrdererName,
                change.OrdererPhone,
                change.OrdererCellPhone,
                change.OrdererEMail,
                change.OrdererDepartment == null ? string.Empty : change.OrdererDepartment.DepartmentName);
        }

        private static GeneralFieldGroupDto CreateGeneralFieldGroup(Change change)
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

        private static RegistrationFieldGroupDto CreateRegistrationFieldGroup(Change change)
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

        private static AnalyzeFieldGroupDto CreateAnalyzeFieldGroup(Change change)
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

        private static ImplementationFieldGroupDto CreateImplementationFieldGroup(Change change)
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

        private static EvaluationFieldGroupDto CreateEvaluationFieldGroup(Change change)
        {
            return new EvaluationFieldGroupDto(
                change.ChangeEvaluation,
                change.EvaluationReady != 0);
        }

        public IList<Change> GetChanges(int customer)
        {
            return (from w in this.DataContext.Set<Change>()
                    where w.Customer_Id == customer
                    select w).ToList();
        }
    }
}
