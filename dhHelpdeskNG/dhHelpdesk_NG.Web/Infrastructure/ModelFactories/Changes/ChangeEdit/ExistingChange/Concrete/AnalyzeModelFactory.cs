namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
    using DH.Helpdesk.Web.Models.Shared;
    using System;

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

        public AnalyzeModel Create(FindChangeResponse response)
        {
            var settings = response.EditSettings.Analyze;
            var options = response.EditOptions;

            var textId = response.EditData.Change.Id.ToString(CultureInfo.InvariantCulture);
            var analyze = response.EditData.Change.Analyze;

            var categories = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Category,
                options.Categories,
                analyze.CategoryId.ToString(),
                true);

            var relatedChanges = new MultiSelectList(
                options.RelatedChanges,
                "Value",
                "Name",
                response.EditData.RelatedChangeIds);

            var priorities = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Priority,
                options.Priorities,
                analyze.PriorityId.ToString(),
                true);

            var responsibles = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Responsible,
                options.Responsibles,
                analyze.ResponsibleId.ToString(),
                true);

            var solution = this.configurableFieldModelFactory.CreateStringField(settings.Solution, analyze.Solution);

            var cost = this.configurableFieldModelFactory.CreateIntegerField(settings.Cost, analyze.Cost);

            var yearlyCost = this.configurableFieldModelFactory.CreateIntegerField(
                settings.YearlyCost,
                analyze.YearlyCost);

            var showCurrency = settings.Cost.Show || settings.YearlyCost.Show;

            var currency =
                this.configurableFieldModelFactory.CreateSelectListField(
                    new FieldEditSetting(showCurrency, "Currency", false, null),
                    options.Currencies,
                    analyze.CurrencyId.ToString(),
                    true);

            var estimatedTimeInHours =
                this.configurableFieldModelFactory.CreateIntegerField(
                    settings.EstimatedTimeInHours,
                    analyze.EstimatedTimeInHours);

            var risk = this.configurableFieldModelFactory.CreateStringField(settings.Risk, analyze.Risk);

            var startDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(
                settings.StartDate,
                analyze.StartDate);
            var startTime = new ConfigurableFieldModel<string>()
            {
                Caption = startDate.Caption,
                IsRequired = startDate.IsRequired,
                Show = startDate.Show
            };

            if (startDate.Value != null)
            {
                var analyzeStartTime = startDate.Value.Value.ToShortTimeString(); 
                 startTime = this.configurableFieldModelFactory.CreateNullableTimeField(
                    settings.StartDate,
                    analyzeStartTime);                
            }
            ConfigurableContainerModel<DateAndTimeModel> startDateAndTime;

            if (settings.StartDate.Show)
            {                
                var dateAndTime = new DateAndTimeModel(startDate, startTime);
                startDateAndTime = new ConfigurableContainerModel<DateAndTimeModel>(
                    settings.StartDate.Caption,
                    dateAndTime);
            }
            else
            {
                startDateAndTime = ConfigurableContainerModel<DateAndTimeModel>.CreateUnshowable();
            }

            var finishDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(
                settings.FinishDate,
                analyze.FinishDate);
            var finishTime = new ConfigurableFieldModel<string>()
                                  { Caption = finishDate.Caption,
                                    IsRequired = finishDate.IsRequired,
                                    Show = finishDate.Show};

            if (finishDate.Value != null)
            {
                var analyzeFinishTime = finishDate.Value.Value.ToShortTimeString();
                finishTime = this.configurableFieldModelFactory.CreateNullableTimeField(
                    settings.FinishDate,
                    analyzeFinishTime);
            }

            ConfigurableContainerModel<DateAndTimeModel> fininshDateAndTime;

            if (settings.FinishDate.Show)
            {
                var dateAndTime = new DateAndTimeModel(finishDate, finishTime);
                fininshDateAndTime = new ConfigurableContainerModel<DateAndTimeModel>(
                    settings.FinishDate.Caption,
                    dateAndTime);
            }
            else
            {
                fininshDateAndTime = ConfigurableContainerModel<DateAndTimeModel>.CreateUnshowable();
            }

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
                response.EditData.Files.Where(f => f.Subtopic == Subtopic.Analyze).Select(f => f.Name).ToList());

            var logs = this.configurableFieldModelFactory.CreateLogs(
                settings.Logs,
                response.EditData.Change.Id,
                Subtopic.Analyze,
                response.EditData.Logs.Where(l => l.Subtopic == Subtopic.Analyze).ToList(),
                options.EmailGroups,
                options.WorkingGroupsWithEmails,
                options.AdministratorsWithEmails);

            var inviteToCabDialog = this.sendToDialogModelFactory.Create(
                options.EmailGroups,
                options.WorkingGroupsWithEmails,
                options.AdministratorsWithEmails);

            var inviteToModel = new InviteToModel(inviteToCabDialog);
            var approvalItems = CreateApprovalItems();

            var approvalSelectList = new SelectList(
                approvalItems,
                "Value",
                "Text",
                response.EditData.Change.Analyze.Approval);

            var approval = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Approval,
                approvalSelectList);

            var rejectExplanation = this.configurableFieldModelFactory.CreateStringField(
                settings.RejectExplanation,
                analyze.RejectExplanation);

            return new AnalyzeModel(
                response.EditData.Change.Id,
                relatedChanges,
                categories,
                priorities,
                responsibles,
                solution,
                cost,
                yearlyCost,
                currency,
                estimatedTimeInHours,
                risk,
                startDateAndTime,
                fininshDateAndTime,
                hasImplementationPlan,
                hasRecoveryPlan,
                attachedFiles,
                logs,
                inviteToModel,
                approval,
                analyze.ApprovedDateAndTime,
                analyze.ApprovedByUser,
                rejectExplanation);
        }

        #endregion

        #region Methods

        private static List<SelectListItem> CreateApprovalItems()
        {
            var noneItem = new SelectListItem();
            noneItem.Text = string.Empty;
            noneItem.Value = StepStatus.None.ToString();

            var approveItem = new SelectListItem();
            approveItem.Text = Translation.Get("Approve");
            approveItem.Value = StepStatus.Approved.ToString();

            var rejectItem = new SelectListItem();
            rejectItem.Text = Translation.Get("Reject");
            rejectItem.Value = StepStatus.Rejected.ToString();

            return new List<SelectListItem> { noneItem, approveItem, rejectItem };
        }

        #endregion
    }
}