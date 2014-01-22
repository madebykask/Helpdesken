namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.InputModel;

    public sealed class ChangeModelFactory : IChangeModelFactory
    {
        public InputModel Create(
            ChangeAggregate change,
            ChangeOptionalData optionalData)
        {
            var header = CreateHeader(
                change,
                optionalData.Departments,
                optionalData.Statuses,
                optionalData.Systems,
                optionalData.Objects,
                optionalData.WorkingGroups,
                optionalData.Administrators);

            var registration = CreateRegistration(change);

            var analyze = CreateAnalyze(
                change,
                optionalData.Categories,
                optionalData.Priorities,
                optionalData.Responsibles,
                optionalData.Currencies);

            var implementation = CreateImplementation(change, optionalData.ImplementationStatuses);
            var evaluation = CreateEvaluation(change);

            return new InputModel(header, registration, analyze, implementation, evaluation);
        }

        private static ChangeHeaderModel CreateHeader(
            ChangeAggregate change,
            List<ItemOverviewDto> departments,
            List<ItemOverviewDto> statuses,
            List<ItemOverviewDto> systems,
            List<ItemOverviewDto> objects,
            List<ItemOverviewDto> workingGroups,
            List<ItemOverviewDto> administrators)
        {
            var departmentList = new SelectList(departments, "Value", "Name", change.Header.DepartmentId);
            var statusList = new SelectList(statuses, "Value", "Name", change.Header.StatusId);
            var systemList = new SelectList(systems, "Value", "Name", change.Header.SystemId);
            var objectList = new SelectList(objects, "Value", "Name", change.Header.ObjectId);
            var workingGroupList = new SelectList(workingGroups, "Value", "Name", change.Header.WorkingGroupId);
            var administratorList = new SelectList(administrators, "Value", "Name", change.Header.AdministratorId);

            return new ChangeHeaderModel(
                change.Header.Id,
                change.Header.Name,
                change.Header.Phone,
                change.Header.CellPhone,
                change.Header.Email,
                departmentList,
                change.Header.Title,
                statusList,
                systemList,
                objectList,
                workingGroupList,
                administratorList,
                change.Header.FinishingDate,
                change.Header.CreatedDate,
                change.Header.ChangedDate,
                change.Header.Rss);
        }

        private static RegistrationModel CreateRegistration(ChangeAggregate change)
        {
            var approveItem = new SelectListItem();
            approveItem.Text = Translation.Get("Approved", Enums.TranslationSource.TextTranslation);
            approveItem.Value = AnalyzeApproveResult.Approved.ToString();

            var rejectItem = new SelectListItem();
            rejectItem.Text = Translation.Get("Reject", Enums.TranslationSource.TextTranslation);
            rejectItem.Value = AnalyzeApproveResult.Rejected.ToString();

            var approvedItems = new List<object> { approveItem, rejectItem };
            var approvedList = new SelectList(approvedItems, "Value", "Text");

            return new RegistrationModel(
                new MultiSelectList(new List<object>()),
                change.Registration.Description,
                change.Registration.BusinessBenefits,
                change.Registration.Consequece,
                change.Registration.Impact,
                change.Registration.DesiredDate,
                change.Registration.Verified,
                approvedList,
                change.Registration.ApprovalExplanation,
                change.Registration.ApprovedDateAndTime,
                change.Registration.ApprovedUser);
        }

        private static AnalyzeModel CreateAnalyze(
            ChangeAggregate change,
            List<ItemOverviewDto> categories,
            List<ItemOverviewDto> priorities,
            List<ItemOverviewDto> responsibles,
            List<ItemOverviewDto> currencies)
        {
            var categoryList = new SelectList(categories, "Value", "Name");
            var priorityList = new SelectList(priorities, "Value", "Name");
            var responsibleList = new SelectList(responsibles, "Value", "Name");
            var currencyList = new SelectList(currencies, "Value", "Name");

            var approveItem = new SelectListItem();
            approveItem.Text = Translation.Get("Approved", Enums.TranslationSource.TextTranslation);
            approveItem.Value = AnalyzeApproveResult.Approved.ToString();

            var rejectItem = new SelectListItem();
            rejectItem.Text = Translation.Get("Reject", Enums.TranslationSource.TextTranslation);
            rejectItem.Value = AnalyzeApproveResult.Rejected.ToString();

            var approvedItems = new List<object> { approveItem, rejectItem };
            var approvedList = new SelectList(approvedItems, "Value", "Text");

            return new AnalyzeModel(
                categoryList,
                priorityList,
                responsibleList,
                change.Analyze.Solution,
                change.Analyze.Cost,
                change.Analyze.YearlyCost,
                currencyList,
                change.Analyze.TimeEstimatesHours,
                change.Analyze.Risk,
                change.Analyze.StartDate,
                change.Analyze.EndDate,
                change.Analyze.HasImplementationPlan,
                change.Analyze.HasRecoveryPlan,
                approvedList,
                change.Analyze.ChangeRecommendation);
        }

        private static ImplementationModel CreateImplementation(
            ChangeAggregate change,
            List<ItemOverviewDto> implementationStatuses)
        {
            var implementationStatusList = new SelectList(implementationStatuses, "Value", "Name");

            return new ImplementationModel(
                implementationStatusList,
                change.Implementation.RealStartDate,
                change.Implementation.FinishingDate,
                change.Implementation.BuildImplemented,
                change.Implementation.ImplementationPlanUsed,
                change.Implementation.ChangeDeviation,
                change.Implementation.RecoveryPlanUsed,
                change.Implementation.ImplementationReady);
        }

        private static EvaluationModel CreateEvaluation(ChangeAggregate change)
        {
            return new EvaluationModel(change.Evaluation.ChangeEvaluation, change.Evaluation.EvaluationReady);
        }
    }
}