namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.InputModel;

    public sealed class NewChangeModelFactory : INewChangeModelFactory
    {
        public NewChangeModel Create(string temporatyId, ChangeOptionalData optionalData)
        {
            var header = CreateHeader(optionalData);
            var registration = CreateRegistration(optionalData);
            var analyze = CreateAnalyze(optionalData);
            var implementation = CreateImplementation(optionalData);
            var evaluation = CreateEvaluation();

            var inputModel = new InputModel(
                header,
                registration,
                analyze, 
                implementation,
                evaluation,
                new HistoryModel(new List<HistoryItemModel>(0)));

            return new NewChangeModel(temporatyId, inputModel);
        }

        private static ChangeHeaderModel CreateHeader(ChangeOptionalData optionalData)
        {
            var departmentList = new SelectList(optionalData.Departments, "Value", "Name");
            var statusList = new SelectList(optionalData.Statuses, "Value", "Name");
            var systemList = new SelectList(optionalData.Systems, "Value", "Name");
            var objectList = new SelectList(optionalData.Objects, "Value", "Name");
            var workingGroupList = new SelectList(optionalData.WorkingGroups, "Value", "Name");
            var administratorList = new SelectList(optionalData.Administrators, "Value", "Name");

            return new ChangeHeaderModel(
                null,
                null,
                null,
                null,
                null,
                departmentList,
                null,
                statusList,
                systemList,
                objectList,
                workingGroupList,
                administratorList,
                null,
                null,
                null,
                false);
        }

        private static RegistrationModel CreateRegistration(ChangeOptionalData optionalData)
        {
            var ownerList = new SelectList(optionalData.Owners, "Value", "Name");
            var processAffectedList = new MultiSelectList(optionalData.ProcessesAffected, "Value", "Name");
            var departmentAffectedList = new MultiSelectList(optionalData.Departments, "Value", "Name");

            var approveItem = new SelectListItem();
            approveItem.Text = Translation.Get("Approved", Enums.TranslationSource.TextTranslation);
            approveItem.Value = RegistrationApproveResult.Approved.ToString();

            var rejectItem = new SelectListItem();
            rejectItem.Text = Translation.Get("Reject", Enums.TranslationSource.TextTranslation);
            rejectItem.Value = RegistrationApproveResult.Rejected.ToString();

            var approvedItems = new List<object> { approveItem, rejectItem };
            var approvedList = new SelectList(approvedItems, "Value", "Text");

            return new RegistrationModel(
                ownerList,
                processAffectedList,
                departmentAffectedList,
                null,
                null,
                null,
                null,
                null,
                false,
                approvedList,
                null,
                null,
                null);
        }

        private static AnalyzeModel CreateAnalyze(ChangeOptionalData optionalData)
        {
            var categoryList = new SelectList(optionalData.Categories, "Value", "Name");
            var priorityList = new SelectList(optionalData.Priorities, "Value", "Name");
            var responsibleList = new SelectList(optionalData.Responsibles, "Value", "Name");
            var currencyList = new SelectList(optionalData.Currencies, "Value", "Name");

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
                null,
                0,
                0,
                currencyList,
                0,
                null,
                null,
                null,
                false,
                false,
                approvedList,
                null);
        }

        private static ImplementationModel CreateImplementation(ChangeOptionalData optionalData)
        {
            var implementationStatusList = new SelectList(optionalData.ImplementationStatuses, "Value", "Name");
            return new ImplementationModel(implementationStatusList, null, null, false, false, null, false, false);
        }

        private static EvaluationModel CreateEvaluation()
        {
            return new EvaluationModel(null, false);
        }
    }
}