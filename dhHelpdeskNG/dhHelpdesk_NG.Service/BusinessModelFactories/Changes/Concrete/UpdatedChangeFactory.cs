namespace dhHelpdesk_NG.Service.BusinessModelFactories.Changes.Concrete
{
    using System;

    using dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange;
    using dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChangeAggregate;

    using UpdatedAnalyzeFields = dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange.UpdatedAnalyzeFields;
    using UpdatedChangeHeader = dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange.UpdatedChangeHeader;
    using UpdatedEvaluationFields = dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange.UpdatedEvaluationFields;
    using UpdatedImplementationFields = dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange.UpdatedImplementationFields;
    using UpdatedRegistrationFields = dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange.UpdatedRegistrationFields;

    public sealed class UpdatedChangeFactory : IUpdatedChangeFactory
    {
        public UpdatedChange Create(UpdatedChangeAggregate updatedChange)
        {
            var header = new UpdatedChangeHeader(
                updatedChange.Header.Id,
                updatedChange.Header.Name,
                updatedChange.Header.Phone,
                updatedChange.Header.CellPhone,
                updatedChange.Header.Email,
                updatedChange.Header.DepartmentId,
                updatedChange.Header.Title,
                updatedChange.Header.StatusId,
                updatedChange.Header.SystemId,
                updatedChange.Header.ObjectId,
                updatedChange.Header.WorkingGroupId,
                updatedChange.Header.AdministratorId,
                updatedChange.Header.FinishingDate,
                updatedChange.Header.ChangedDate,
                updatedChange.Header.Rss);

            var registration = new UpdatedRegistrationFields(
                updatedChange.Registration.OwnerId,
                updatedChange.Registration.Description,
                updatedChange.Registration.BusinessBenefits,
                updatedChange.Registration.Consequece,
                updatedChange.Registration.Impact,
                updatedChange.Registration.DesiredDate,
                updatedChange.Registration.Verified,
                updatedChange.Registration.Approved,
                updatedChange.Registration.ApprovedUser,
                updatedChange.Registration.ApprovedDateAndTime,
                updatedChange.Registration.ApprovalExplanation);

            var analyze = new UpdatedAnalyzeFields(
                updatedChange.Analyze.CategoryId,
                updatedChange.Analyze.PriorityId,
                updatedChange.Analyze.ResponsibleId,
                updatedChange.Analyze.Solution,
                updatedChange.Analyze.Cost,
                updatedChange.Analyze.YearlyCost,
                updatedChange.Analyze.CurrencyId,
                updatedChange.Analyze.TimeEstimatesHours,
                updatedChange.Analyze.Risk,
                updatedChange.Analyze.StartDate,
                updatedChange.Analyze.EndDate,
                updatedChange.Analyze.HasImplementationPlan,
                updatedChange.Analyze.HasRecoveryPlan,
                updatedChange.Analyze.Approved,
                DateTime.Now,
                string.Empty,
                updatedChange.Analyze.ChangeRecommendation);

            var implementation = new UpdatedImplementationFields(
                updatedChange.Implementation.ImplementationStatusId,
                updatedChange.Implementation.RealStartDate,
                updatedChange.Implementation.FinishingDate,
                updatedChange.Implementation.BuildImplemented,
                updatedChange.Implementation.ImplementationPlanUsed,
                updatedChange.Implementation.ChangeDeviation,
                updatedChange.Implementation.RecoveryPlanUsed,
                updatedChange.Implementation.ImplementationReady);

            var evaluation = new UpdatedEvaluationFields(
                updatedChange.Evaluation.ChangeEvaluation,
                updatedChange.Evaluation.EvaluationReady);

            return new UpdatedChange(updatedChange.Id, header, registration, analyze, implementation, evaluation);
        }
    }
}
