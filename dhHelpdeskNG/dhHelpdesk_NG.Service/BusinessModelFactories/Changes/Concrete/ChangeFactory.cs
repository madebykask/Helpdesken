namespace dhHelpdesk_NG.Service.BusinessModelFactories.Changes.Concrete
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Change;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    using AnalyzeFields = dhHelpdesk_NG.DTO.DTOs.Changes.Output.AnalyzeFields;
    using ChangeHeader = dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeHeader;
    using ImplementationFields = dhHelpdesk_NG.DTO.DTOs.Changes.Output.ImplementationFields;
    using RegistrationFields = dhHelpdesk_NG.DTO.DTOs.Changes.Output.RegistrationFields;

    public sealed class ChangeFactory : IChangeFactory
    {
        public ChangeAggregate Create(Change change, List<Contact> contacts)
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

            return new ChangeAggregate(
                header,
                registration,
                analyze, 
                implementation);
        }
    }
}
