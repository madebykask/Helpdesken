namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChangeAggregate;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.Models;
    using DH.Helpdesk.Web.Models.Changes;

    public sealed class UpdatedChangeAggregateFactory : IUpdatedChangeAggregateFactory
    {
        public UpdatedChangeAggregate Create(
            ChangeModel model,
            ChangeNewSubitems newSubitems,
            ChangeDeletedSubitems deletedSubitems,
            DateTime changedDate)
        {
            var header = CreateHeader(model);
            var registration = CreateRegistration(model);
            var analyze = CreateAnalyze(model, deletedSubitems);
            var implementation = CreateImplementation(model, deletedSubitems);
            var evaluation = CreateEvaluation(model, deletedSubitems);

            return new UpdatedChangeAggregate(
                model.Id,
                header,
                registration,
                analyze,
                implementation,
                evaluation,
                deletedSubitems.LogIds,
                changedDate);
        }

        private static UpdatedChangeHeader CreateHeader(ChangeModel model)
        {
            return new UpdatedChangeHeader(
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
        }

        private static UpdatedRegistrationFields CreateRegistration(ChangeModel model)
        {
            return new UpdatedRegistrationFields(
                new List<Contact>(),
                model.Input.Registration.OwnerId,
                model.Input.Registration.ProcessAffectedIds,
                model.Input.Registration.DepartmentAffectedIds,
                model.Input.Registration.Description.Value,
                model.Input.Registration.BusinessBenefits.Value,
                model.Input.Registration.Consequence.Value,
                model.Input.Registration.Impact.Value,
                model.Input.Registration.DesiredDate.Value,
                model.Input.Registration.Verified.Value,
                new List<DeletedFile>(),
                new List<NewFile>(), 
                model.Input.Registration.ApprovedValue,
                model.Input.Registration.ApprovedDateAndTime,
                model.Input.Registration.ApprovedUser,
                model.Input.Registration.ApprovableExplanation);
        }

        private static UpdatedAnalyzeFields CreateAnalyze(ChangeModel model, ChangeDeletedSubitems deletedSubitems)
        {
            return new UpdatedAnalyzeFields(
                model.Input.Analyze.CategoryId,
                model.Input.Analyze.RelatedChangeIds,
                model.Input.Analyze.PriorityId,
                model.Input.Analyze.ResponsibleId,
                model.Input.Analyze.Solution.Value,
                model.Input.Analyze.Cost.Value,
                model.Input.Analyze.YearlyCost.Value,
                model.Input.Analyze.CurrencyId,
                model.Input.Analyze.TimeEstimatesHours.Value,
                model.Input.Analyze.Risk.Value,
                model.Input.Analyze.StartDate.Value,
                model.Input.Analyze.EndDate.Value,
                model.Input.Analyze.HasImplementationPlan.Value,
                model.Input.Analyze.HasRecoveryPlan.Value,
                new List<DeletedFile>(), 
                new List<NewFile>(), 
                null, 
                model.Input.Analyze.ApprovedValue,
                model.Input.Analyze.ChangeRecommendation.Value);
        }

        private static UpdatedImplementationFields CreateImplementation(ChangeModel model, ChangeDeletedSubitems deletedSubitems)
        {
            return new UpdatedImplementationFields(
                model.Input.Implementation.ImplementationStatusId,
                model.Input.Implementation.RealStartDate.Value,
                model.Input.Implementation.FinishingDate.Value,
                model.Input.Implementation.BuildImplemented.Value,
                model.Input.Implementation.ImplementationPlanUsed.Value,
                model.Input.Implementation.ChangeDeviation.Value,
                model.Input.Implementation.RecoveryPlanUsed.Value,
                model.Input.Implementation.ImplementationReady.Value,
                null);
        }

        private static UpdatedEvaluationFields CreateEvaluation(ChangeModel model, ChangeDeletedSubitems deletedSubitems)
        {
            return new UpdatedEvaluationFields(
                model.Input.Evaluation.ChangeEvaluation.Value,
                null,
                model.Input.Evaluation.EvaluationReady.Value);
        }
    }
}