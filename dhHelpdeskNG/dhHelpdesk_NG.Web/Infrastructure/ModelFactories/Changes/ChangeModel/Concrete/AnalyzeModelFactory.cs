namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Requests.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class AnalyzeModelFactory : IAnalyzeModelFactory
    {
        //        #region Fields
        //
        //        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;
        //
        //        private readonly ISendToDialogModelFactory sendToDialogModelFactory;
        //
        //        #endregion
        //
        //        #region Constructors and Destructors
        //
        //        public AnalyzeModelFactory(
        //            IConfigurableFieldModelFactory configurableFieldModelFactory,
        //            ISendToDialogModelFactory sendToDialogModelFactory)
        //        {
        //            this.configurableFieldModelFactory = configurableFieldModelFactory;
        //            this.sendToDialogModelFactory = sendToDialogModelFactory;
        //        }
        //
        //        #endregion
        //
        //        #region Public Methods and Operators
        //
        //        public AnalyzeModel Create(string temporaryId, ChangeEditData editData, AnalyzeFieldEditSettings editSettings)
        //        {
        //            var category = this.CreateCategory(editSettings, editData, null);
        //            var relatedChanges = this.CreateRelatedChanges(editData, null);
        //            var priority = this.CreatePriority(editSettings, editData, null);
        //            var responsible = this.CreateResponsible(editSettings, editData, null);
        //            var solution = this.CreateSolution(editSettings, null);
        //            var cost = this.CreateCost(editSettings, null);
        //            var yearlyCost = this.CreateYearlyCost(editSettings, null);
        //            var currency = this.CreateCurrency(editSettings, editData, null);
        //            var timeEstimatesHours = this.CreateTimeEstimatesHours(editSettings, null);
        //            var risk = this.CreateRisk(editSettings, null);
        //            var startDate = this.CreateStartDate(editSettings, null);
        //            var endDate = this.CreateEndDate(editSettings, null);
        //            var hasImplementationPlan = this.CreateHasImplementationPlan(editSettings, null);
        //            var hasRecoveryPlan = this.CreateHasRecoveryPlan(editSettings, null);
        //            var attachedFilesContainer = this.CreateAttachedFilesContainer(temporaryId);
        //            var sendToDialog = this.CreateSendToDialog(editData);
        //            var approval = this.CreateApproval(editSettings);
        //            var rejectExplanation = this.CreateRejectExplanation(editSettings, null);
        //
        //            return new AnalyzeModel(
        //                temporaryId,
        //                category,
        //                relatedChanges,
        //                priority,
        //                responsible,
        //                solution,
        //                cost,
        //                yearlyCost,
        //                currency,
        //                timeEstimatesHours,
        //                risk,
        //                startDate,
        //                endDate,
        //                hasImplementationPlan,
        //                hasRecoveryPlan,
        //                attachedFilesContainer,
        //                sendToDialog,
        //                approval,
        //                rejectExplanation);
        //        }
        //
        //        public AnalyzeModel Create(
        //            ChangeAggregate change,
        //            AnalyzeFieldEditSettings editSettings,
        //            ChangeEditData editData)
        //        {
        //            var id = change.Id.ToString(CultureInfo.InvariantCulture);
        //            var category = this.CreateCategory(editSettings, editData, change);
        //            var relatedChanges = this.CreateRelatedChanges(editData, change);
        //            var priority = this.CreatePriority(editSettings, editData, change);
        //            var responsible = this.CreateResponsible(editSettings, editData, change);
        //            var solution = this.CreateSolution(editSettings, change);
        //            var cost = this.CreateCost(editSettings, change);
        //            var yearlyCost = this.CreateYearlyCost(editSettings, change);
        //            var currency = this.CreateCurrency(editSettings, editData, change);
        //            var timeEstimatesHours = this.CreateTimeEstimatesHours(editSettings, change);
        //            var risk = this.CreateRisk(editSettings, change);
        //            var startDate = this.CreateStartDate(editSettings, change);
        //            var endDate = this.CreateEndDate(editSettings, change);
        //            var hasImplementationPlan = this.CreateHasImplementationPlan(editSettings, change);
        //            var hasRecoveryPlan = this.CreateHasRecoveryPlan(editSettings, change);
        //
        //            var attachedFilesContainer =
        //                this.CreateAttachedFilesContainer(change.Id.ToString(CultureInfo.InvariantCulture));
        //
        //            var sendToDialog = this.CreateSendToDialog(editData);
        //            var approval = this.CreateApproval(editSettings);
        //            var rejectExplanation = this.CreateRejectExplanation(editSettings, change);
        //
        //            return new AnalyzeModel(
        //                id,
        //                category,
        //                relatedChanges,
        //                priority,
        //                responsible,
        //                solution,
        //                cost,
        //                yearlyCost,
        //                currency,
        //                timeEstimatesHours,
        //                risk,
        //                startDate,
        //                endDate,
        //                hasImplementationPlan,
        //                hasRecoveryPlan,
        //                attachedFilesContainer,
        //                sendToDialog,
        //                approval,
        //                rejectExplanation);
        //        }
        //
        //        #endregion
        //
        //        #region Methods
        //
        //        private ConfigurableFieldModel<SelectList> CreateApproval(AnalyzeFieldEditSettings editSettings)
        //        {
        //            var approveItem = new SelectListItem();
        //            approveItem.Text = Translation.Get("Approved", Enums.TranslationSource.TextTranslation);
        //            approveItem.Value = AnalyzeApprovalResult.Approved.ToString();
        //
        //            var rejectItem = new SelectListItem();
        //            rejectItem.Text = Translation.Get("Reject", Enums.TranslationSource.TextTranslation);
        //            rejectItem.Value = AnalyzeApprovalResult.Rejected.ToString();
        //
        //            var approvedItems = new List<object> { approveItem, rejectItem };
        //            var approvedList = new SelectList(approvedItems, "Value", "Text");
        //
        //            return this.configurableFieldModelFactory.CreateSelectListField(editSettings.Approval, approvedList);
        //        }
        //
        //        private AttachedFilesModel CreateAttachedFilesContainer(string id)
        //        {
        //            return new AttachedFilesModel(id, Subtopic.Analyze);
        //        }
        //
        //        private ConfigurableFieldModel<SelectList> CreateCategory(
        //            AnalyzeFieldEditSettings editSettings,
        //            ChangeEditData editData,
        //            object a)
        //        {
        //            string selectedValue = null;
        //
        //            if (change != null && change.Analyze.CategoryId.HasValue)
        //            {
        //                selectedValue = change.Analyze.CategoryId.ToString();
        //            }
        //
        //            return this.configurableFieldModelFactory.CreateSelectListField(
        //                editSettings.Category,
        //                editData.Categories,
        //                selectedValue);
        //        }
        //
        //        private ConfigurableFieldModel<int> CreateCost(AnalyzeFieldEditSettings editSettings, ChangeAggregate change)
        //        {
        //            var value = change != null ? change.Analyze.Cost : 0;
        //            return this.configurableFieldModelFactory.CreateIntegerField(editSettings.Cost, value);
        //        }
        //
        //        private ConfigurableFieldModel<SelectList> CreateCurrency(
        //            AnalyzeFieldEditSettings editSettings,
        //            ChangeEditData editData,
        //            ChangeAggregate change)
        //        {
        //            if (!editSettings.Cost.Show && !editSettings.YearlyCost.Show)
        //            {
        //                return new ConfigurableFieldModel<SelectList>(false);
        //            }
        //
        //            var currencySetting = new FieldEditSetting(true, "Currency", true, null);
        //
        //            string selectedValue = null;
        //
        //            if (change != null && change.Analyze.CurrencyId.HasValue)
        //            {
        //                selectedValue = change.Analyze.CurrencyId.ToString();
        //            }
        //
        //            return this.configurableFieldModelFactory.CreateSelectListField(
        //                currencySetting,
        //                editData.Currencies,
        //                selectedValue);
        //        }
        //
        //        private ConfigurableFieldModel<DateTime?> CreateEndDate(
        //            AnalyzeFieldEditSettings editSettings,
        //            ChangeAggregate change)
        //        {
        //            var value = change != null ? change.Analyze.EndDate : null;
        //            return this.configurableFieldModelFactory.CreateNullableDateTimeField(editSettings.FinishDate, value);
        //        }
        //
        //        private ConfigurableFieldModel<bool> CreateHasImplementationPlan(
        //            AnalyzeFieldEditSettings editSettings,
        //            ChangeAggregate change)
        //        {
        //            var value = change != null ? change.Analyze.HasImplementationPlan : false;
        //            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.HasImplementationPlan, value);
        //        }
        //
        //        private ConfigurableFieldModel<bool> CreateHasRecoveryPlan(
        //            AnalyzeFieldEditSettings editSettings,
        //            ChangeAggregate change)
        //        {
        //            var value = change != null ? change.Analyze.HasRecoveryPlan : false;
        //            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.HasRecoveryPlan, value);
        //        }
        //
        //        private ConfigurableFieldModel<SelectList> CreatePriority(
        //            AnalyzeFieldEditSettings editSettings,
        //            ChangeEditData editData,
        //            ChangeAggregate change)
        //        {
        //            string selectedValue = null;
        //
        //            if (change != null && change.Analyze.PriorityId.HasValue)
        //            {
        //                selectedValue = change.Analyze.PriorityId.ToString();
        //            }
        //
        //            return this.configurableFieldModelFactory.CreateSelectListField(
        //                editSettings.Priority,
        //                editData.Priorities,
        //                selectedValue);
        //        }
        //
        //        private ConfigurableFieldModel<string> CreateRejectExplanation(
        //            AnalyzeFieldEditSettings editSettings,
        //            ChangeAggregate change)
        //        {
        //            var value = change != null
        //                ? change.Analyze.ChangeRecommendation
        //                : editSettings.RejectExplanation.DefaultValue;
        //            return this.configurableFieldModelFactory.CreateStringField(editSettings.RejectExplanation, value);
        //        }
        //
        //        private MultiSelectList CreateRelatedChanges(ChangeEditData editData, ChangeAggregate change)
        //        {
        //            return new MultiSelectList(editData.RelatedChanges, "Value", "Name");
        //        }
        //
        //        private ConfigurableFieldModel<SelectList> CreateResponsible(
        //            AnalyzeFieldEditSettings editSettings,
        //            ChangeEditData editData,
        //            Change change)
        //        {
        //            string selectedValue = null;
        //
        //            if (change != null && change.Analyze.ResponsibleId.HasValue)
        //            {
        //                selectedValue = change.Analyze.ResponsibleId.ToString();
        //            }
        //
        //            return this.configurableFieldModelFactory.CreateSelectListField(
        //                editSettings.Responsible,
        //                editData.Responsibles,
        //                selectedValue);
        //        }
        //
        //        private ConfigurableFieldModel<string> CreateRisk(AnalyzeFieldEditSettings editSettings, Change change change)
        //        {
        //            var value = change != null ? change.Analyze.Risk : editSettings.Risk.DefaultValue;
        //            return this.configurableFieldModelFactory.CreateStringField(editSettings.Risk, value);
        //        }
        //
        //        private SendToDialogModel CreateSendToDialog(ChangeEditData editData)
        //        {
        //            return this.sendToDialogModelFactory.Create(
        //                editData.EmailGroups,
        //                editData.WorkingGroupsWithEmails,
        //                editData.Administrators);
        //        }
        //
        //        private ConfigurableFieldModel<string> CreateSolution(
        //            AnalyzeFieldEditSettings editSettings,
        //            Change change)
        //        {
        //            var value = change != null ? change.Analyze.Solution : editSettings.Solution.DefaultValue;
        //            return this.configurableFieldModelFactory.CreateStringField(editSettings.Solution, value);
        //        }
        //
        //        private ConfigurableFieldModel<DateTime?> CreateStartDate(
        //            AnalyzeFieldEditSettings editSettings,
        //            Change change)
        //        {
        //            var value = change != null ? change.Analyze.StartDate : null;
        //            return this.configurableFieldModelFactory.CreateNullableDateTimeField(editSettings.StartDate, value);
        //        }
        //
        //        private ConfigurableFieldModel<int> CreateTimeEstimatesHours(
        //            AnalyzeFieldEditSettings editSettings,
        //            Change change)
        //        {
        //            var value = change != null ? change.Analyze.TimeEstimatesHours : 0;
        //            return this.configurableFieldModelFactory.CreateIntegerField(editSettings.EstimatedTimeInHours, value);
        //        }
        //
        //        private ConfigurableFieldModel<int> CreateYearlyCost(
        //            AnalyzeFieldEditSettings editSettings,
        //            Change change)
        //        {
        //            var value = change != null ? change.Analyze.YearlyCost : 0;
        //            return this.configurableFieldModelFactory.CreateIntegerField(editSettings.YearlyCost, value);
        //        }
        //
        //        #endregion
        //    }

        #region Public Methods and Operators

        public AnalyzeModel Create(string temporaryId, ChangeEditData editData, AnalyzeFieldEditSettings editSettings)
        {
            throw new NotImplementedException();
        }

        public AnalyzeModel Create(
            UpdateChangeRequest request,
            ChangeEditData editData,
            AnalyzeFieldEditSettings editSettings)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}