namespace DH.Helpdesk.Services.BusinessModelFactories.Changes.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate;

    using AnalyzeFields = DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate.AnalyzeFields;
    using ChangeHeader = DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate.ChangeHeader;
    using EvaluationFields = DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate.EvaluationFields;
    using ImplementationFields = DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate.ImplementationFields;
    using RegistrationFields = DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate.RegistrationFields;

    public sealed class ChangeAggregateFactory : IChangeAggregateFactory
    {
        public ChangeAggregate Create(Change change, List<Contact> contacts, List<HistoriesDifference> histories)
        {
            var header = new ChangeHeader(
                change.Header.Id,
                change.Header.Name,
                change.Header.Phone,
                change.Header.CellPhone,
                change.Header.Email,
                change.Header.DepartmentId,
                change.Header.Title,
                change.Header.StatusId,
                change.Header.SystemId,
                change.Header.ObjectId,
                change.Header.WorkingGroupId,
                change.Header.AdministratorId,
                change.Header.FinishingDate,
                change.Header.CreatedDate,
                change.Header.ChangedDate,
                change.Header.Rss);

            var registration = new RegistrationFields(
                contacts,
                change.Registration.OwnerId,
                new List<int>(),
                new List<int>(),
                change.Registration.Description,
                change.Registration.BusinessBenefits,
                change.Registration.Consequence,
                change.Registration.Impact,
                change.Registration.DesiredDate,
                change.Registration.Verified,
                change.Registration.Approved,
                change.Registration.ApprovedDateAndTime,
                change.Registration.ApprovedUser,
                change.Registration.ChangeRecommendation);

            var analyze = new AnalyzeFields(
                change.Analyze.CategoryId,
                change.Analyze.PriorityId,
                change.Analyze.ResponsibleId,
                change.Analyze.Solution,
                change.Analyze.Cost,
                change.Analyze.YearlyCost,
                change.Analyze.CurrencyId,
                change.Analyze.TimeEstimatesHours,
                change.Analyze.Risk,
                change.Analyze.StartDate,
                change.Analyze.EndDate,
                change.Analyze.HasImplementationPlan,
                change.Analyze.HasRecoveryPlan,
                change.Analyze.Approved,
                change.Analyze.ChangeRecommendation);

            var implementation = new ImplementationFields(
                change.Implementation.ImplementationStatusId,
                change.Implementation.RealStartDate,
                change.Implementation.FinishingDate,
                change.Implementation.BuildImplemented,
                change.Implementation.ImplementationPlanUsed,
                change.Implementation.ChangeDeviation,
                change.Implementation.RecoveryPlanUsed,
                change.Implementation.Ready);

            var evaluation = new EvaluationFields(change.Evaluation.ChangeEvaluation, change.Evaluation.EvaluationReady);

            return new ChangeAggregate(change.Id, header, registration, analyze, implementation, evaluation, histories);
        }
    }
}
