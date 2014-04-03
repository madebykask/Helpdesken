namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class AnalyzeModelFactory : IAnalyzeModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        #endregion

        #region Constructors and Destructors

        public AnalyzeModelFactory(
            IConfigurableFieldModelFactory configurableFieldModelFactory, 
            ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public AnalyzeViewModel Create(
            FindChangeResponse response,
            ChangeEditData editData,
            AnalyzeEditSettings settings)
        {
            var textId = response.Change.Id.ToString(CultureInfo.InvariantCulture);
            var analyze = response.Change.Analyze;

            var category = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Category,
                editData.Categories,
                analyze.CategoryId);

            var relatedChanges = new MultiSelectList(
                editData.RelatedChanges,
                "Value",
                "Name",
                response.RelatedChangeIds);

            var priority = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Priority,
                editData.Priorities,
                analyze.PriorityId);

            var responsible = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Responsible,
                editData.Responsibles,
                analyze.ResponsibleId);

            var solution = this.configurableFieldModelFactory.CreateStringField(settings.Solution, analyze.Solution);

            var cost = this.configurableFieldModelFactory.CreateIntegerField(settings.Cost, analyze.Cost);

            var yearlyCost = this.configurableFieldModelFactory.CreateIntegerField(
                settings.YearlyCost,
                analyze.YearlyCost);

            SelectList currency = null;

            if (cost.Show || yearlyCost.Show)
            {
                currency = new SelectList(editData.Currencies, "Value", "Name", analyze.CurrencyId);
            }

            var estimatedTimeInHours =
                this.configurableFieldModelFactory.CreateIntegerField(
                    settings.EstimatedTimeInHours,
                    analyze.EstimatedTimeInHours);

            var risk = this.configurableFieldModelFactory.CreateStringField(settings.Risk, analyze.Risk);

            var startDate = this.configurableFieldModelFactory.CreateDateTimeField(
                settings.StartDate,
                analyze.StartDate);

            var finishDate = this.configurableFieldModelFactory.CreateDateTimeField(
                settings.FinishDate,
                analyze.FinishDate);

            var hasImplementationPlan =
                this.configurableFieldModelFactory.CreateBooleanField(
                    settings.HasImplementationPlan,
                    analyze.HasImplementationPlan);

            var hasRecoveryPlan = this.configurableFieldModelFactory.CreateBooleanField(
                settings.HasRecoveryPlan,
                analyze.HasRecoveryPlan);

            var attachedFiles = this.configurableFieldModelFactory.CreateAttachedFiles(
                settings.AttachedFiles,
                textId,
                Subtopic.Analyze,
                response.Files);

            var logs = this.configurableFieldModelFactory.CreateLogs(
                settings.Logs,
                response.Change.Id,
                Subtopic.Analyze,
                response.Logs);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                editData.EmailGroups,
                editData.WorkingGroupsWithEmails,
                editData.Administrators);

            var approvalItems = CreateApprovalItems();

            var approval = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Approval,
                approvalItems,
                analyze.Approval);

            var rejectExplanation = this.configurableFieldModelFactory.CreateStringField(
                settings.RejectExplanation,
                analyze.RejectExplanation);

            var analyzeModel = new AnalyzeModel(
                response.Change.Id,
                solution,
                cost,
                yearlyCost,
                estimatedTimeInHours,
                risk,
                startDate,
                finishDate,
                hasImplementationPlan,
                hasRecoveryPlan,
                attachedFiles,
                logs,
                sendToDialog,
                response.Change.Analyze.ApprovedDateAndTime,
                response.Change.Analyze.ApprovedByUser,
                rejectExplanation);

            return new AnalyzeViewModel(
                category,
                relatedChanges,
                priority,
                responsible,
                currency,
                approval,
                analyzeModel);
        }

        #endregion

        #region Methods

        private static List<SelectListItem> CreateApprovalItems()
        {
            var approveItem = new SelectListItem();
            approveItem.Text = Translation.Get("Approve", Enums.TranslationSource.TextTranslation);
            approveItem.Value = StepStatus.Approved.ToString();

            var rejectItem = new SelectListItem();
            rejectItem.Text = Translation.Get("Reject", Enums.TranslationSource.TextTranslation);
            rejectItem.Value = StepStatus.Rejected.ToString();

            return new List<SelectListItem> { approveItem, rejectItem };
        }

        #endregion
    }
}