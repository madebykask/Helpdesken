namespace dhHelpdesk_NG.Web.Infrastructure.DtoFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChangeAggregate;
    using dhHelpdesk_NG.Web.Models.Changes;

    public sealed class UpdatedChangeAggregateFactory : IUpdatedChangeAggregateFactory
    {
        public UpdatedChangeAggregate Create(ChangeModel model, DateTime changedDate)
        {
            var header = new UpdatedChangeHeader(
                model.Input.Header.Id,
                model.Input.Header.Name,
                model.Input.Header.Phone,
                model.Input.Header.CellPhone,
                model.Input.Header.Email,
                model.Input.Header.DepartmentId,
                model.Input.Header.Title,
                model.Input.Header.StatusId,
                model.Input.Header.SystemId,
                model.Input.Header.ObjectId,
                model.Input.Header.WorkingGroupId,
                model.Input.Header.AdministratorId,
                model.Input.Header.FinishingDate,
                model.Input.Header.ChangedDate,
                model.Input.Header.Rss);

            var registration = new UpdatedRegistrationFields(
                new List<Contact>(), 
                model.Input.Registration.OwnerId,
                model.Input.Registration.ProcessAffectedIds,
                model.Input.Registration.DepartmentAffectedIds,
                model.Input.Registration.Description,
                model.Input.Registration.BusinessBenefits,
                model.Input.Registration.Consequence,
                model.Input.Registration.Impact,
                model.Input.Registration.DesiredDate,
                model.Input.Registration.Verified,
                model.Input.Registration.ApprovedValue,
                model.Input.Registration.ApprovedDateAndTime,
                model.Input.Registration.ApprovedUser,
                model.Input.Registration.ApprovableExplanation);

            var analyze = new UpdatedAnalyzeFields(
                model.Input.Analyze.CategoryId,
                model.Input.Analyze.PriorityId,
                model.Input.Analyze.ResponsibleId,
                model.Input.Analyze.Solution,
                model.Input.Analyze.Cost,
                model.Input.Analyze.YearlyCost,
                model.Input.Analyze.CurrencyId,
                model.Input.Analyze.TimeEstimatesHours,
                model.Input.Analyze.Risk,
                model.Input.Analyze.StartDate,
                model.Input.Analyze.EndDate,
                model.Input.Analyze.HasImplementationPlan,
                model.Input.Analyze.HasRecoveryPlan,
                model.Input.Analyze.ApprovedValue,
                model.Input.Analyze.ChangeRecommendation);

            var implementation = new UpdatedImplementationFields(
                model.Input.Implementation.ImplementationStatusId,
                model.Input.Implementation.RealStartDate,
                model.Input.Implementation.FinishingDate,
                model.Input.Implementation.BuildImplemented,
                model.Input.Implementation.ImplementationPlanUsed,
                model.Input.Implementation.ChangeDeviation,
                model.Input.Implementation.RecoveryPlanUsed,
                model.Input.Implementation.ImplementationReady);

            var evaluation = new UpdatedEvaluationFields(
                model.Input.Evaluation.ChangeEvaluation,
                model.Input.Evaluation.EvaluationReady);

            return new UpdatedChangeAggregate(
                model.Id,
                header,
                registration,
                analyze,
                implementation,
                evaluation,
                changedDate);
        }
    }
}