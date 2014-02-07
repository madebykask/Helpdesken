namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.Edit;

    public sealed class AnalyzeModelFactory : IAnalyzeModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        public AnalyzeModelFactory(
            IConfigurableFieldModelFactory configurableFieldModelFactory,
            ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        public AnalyzeModel Create(string temporaryId, AnalyzeFieldEditSettings editSettings, ChangeEditOptions optionalData)
        {
            var category = this.CreateCategory(editSettings, optionalData, null);
            var relatedChanges = this.CreateRelatedChanges(optionalData, null);
            var priority = this.CreatePriority(editSettings, optionalData, null);
            var responsible = this.CreateResponsible(editSettings, optionalData, null);
            var solution = this.CreateSolution(editSettings, null);
            var cost = this.CreateCost(editSettings, null);
            var yearlyCost = this.CreateYearlyCost(editSettings, null);
            var currency = this.CreateCurrency(editSettings, optionalData, null);
            var timeEstimatesHours = this.CreateTimeEstimatesHours(editSettings, null);
            var risk = this.CreateRisk(editSettings, null);
            var startDate = this.CreateStartDate(editSettings, null);
            var endDate = this.CreateEndDate(editSettings, null);
            var hasImplementationPlan = this.CreateHasImplementationPlan(editSettings, null);
            var hasRecoveryPlan = this.CreateHasRecoveryPlan(editSettings, null);
            var attachedFilesContainer = this.CreateAttachedFilesContainer(temporaryId);
            var sendToDialog = this.CreateSendToDialog(optionalData);
            var approval = this.CreateApproval(editSettings);
            var rejectExplanation = this.CreateRejectExplanation(editSettings, null);

            return new AnalyzeModel(
                temporaryId,
                category,
                relatedChanges,
                priority,
                responsible,
                solution,
                cost,
                yearlyCost,
                currency,
                timeEstimatesHours,
                risk,
                startDate,
                endDate,
                hasImplementationPlan,
                hasRecoveryPlan,
                attachedFilesContainer,
                sendToDialog,
                approval,
                rejectExplanation);
        }

        public AnalyzeModel Create(ChangeAggregate change, AnalyzeFieldEditSettings editSettings, ChangeEditOptions optionalData)
        {
            var id = change.Id.ToString(CultureInfo.InvariantCulture);
            var category = this.CreateCategory(editSettings, optionalData, change);
            var relatedChanges = this.CreateRelatedChanges(optionalData, change);
            var priority = this.CreatePriority(editSettings, optionalData, change);
            var responsible = this.CreateResponsible(editSettings, optionalData, change);
            var solution = this.CreateSolution(editSettings, change);
            var cost = this.CreateCost(editSettings, change);
            var yearlyCost = this.CreateYearlyCost(editSettings, change);
            var currency = this.CreateCurrency(editSettings, optionalData, change);
            var timeEstimatesHours = this.CreateTimeEstimatesHours(editSettings, change);
            var risk = this.CreateRisk(editSettings, change);
            var startDate = this.CreateStartDate(editSettings, change);
            var endDate = this.CreateEndDate(editSettings, change);
            var hasImplementationPlan = this.CreateHasImplementationPlan(editSettings, change);
            var hasRecoveryPlan = this.CreateHasRecoveryPlan(editSettings, change);

            var attachedFilesContainer =
                this.CreateAttachedFilesContainer(change.Id.ToString(CultureInfo.InvariantCulture));
            
            var sendToDialog = this.CreateSendToDialog(optionalData);
            var approval = this.CreateApproval(editSettings);
            var rejectExplanation = this.CreateRejectExplanation(editSettings, change);

            return new AnalyzeModel(
                id,
                category,
                relatedChanges,
                priority,
                responsible,
                solution,
                cost,
                yearlyCost,
                currency,
                timeEstimatesHours,
                risk,
                startDate,
                endDate,
                hasImplementationPlan,
                hasRecoveryPlan,
                attachedFilesContainer,
                sendToDialog,
                approval,
                rejectExplanation);
        }

        private ConfigurableFieldModel<SelectList> CreateCategory(
            AnalyzeFieldEditSettings editSettings,
            ChangeEditOptions optionalData,
            ChangeAggregate change)
        {
            string selectedValue = null;

            if (change != null && change.Analyze.CategoryId.HasValue)
            {
                selectedValue = change.Analyze.CategoryId.ToString();
            }

            return this.configurableFieldModelFactory.CreateSelectListField(
                editSettings.Category,
                optionalData.Categories,
                selectedValue);
        }

        private MultiSelectList CreateRelatedChanges(ChangeEditOptions optionalData, ChangeAggregate change)
        {
            return new MultiSelectList(optionalData.RelatedChanges, "Value", "Name");
        }

        private ConfigurableFieldModel<SelectList> CreatePriority(
            AnalyzeFieldEditSettings editSettings,
            ChangeEditOptions optionalData,
            ChangeAggregate change)
        {
            string selectedValue = null;

            if (change != null && change.Analyze.PriorityId.HasValue)
            {
                selectedValue = change.Analyze.PriorityId.ToString();
            }

            return this.configurableFieldModelFactory.CreateSelectListField(
                editSettings.Priority,
                optionalData.Priorities,
                selectedValue);
        }

        private ConfigurableFieldModel<SelectList> CreateResponsible(
            AnalyzeFieldEditSettings editSettings,
            ChangeEditOptions optionalData,
            ChangeAggregate change)
        {
            string selectedValue = null;

            if (change != null && change.Analyze.ResponsibleId.HasValue)
            {
                selectedValue = change.Analyze.ResponsibleId.ToString();
            }

            return this.configurableFieldModelFactory.CreateSelectListField(
                editSettings.Responsible,
                optionalData.Responsibles,
                selectedValue);
        }

        private ConfigurableFieldModel<string> CreateSolution(
            AnalyzeFieldEditSettings editSettings,
            ChangeAggregate change)
        {
            var value = change != null ? change.Analyze.Solution : editSettings.Solution.DefaultValue;
            return this.configurableFieldModelFactory.CreateStringField(editSettings.Solution, value);
        }

        private ConfigurableFieldModel<int> CreateCost(AnalyzeFieldEditSettings editSettings, ChangeAggregate change)
        {
            var value = change != null ? change.Analyze.Cost : 0;
            return this.configurableFieldModelFactory.CreateIntegerField(editSettings.Cost, value);
        }

        private ConfigurableFieldModel<int> CreateYearlyCost(
            AnalyzeFieldEditSettings editSettings,
            ChangeAggregate change)
        {
            var value = change != null ? change.Analyze.YearlyCost : 0;
            return this.configurableFieldModelFactory.CreateIntegerField(editSettings.YearlyCost, value);
        }

        private ConfigurableFieldModel<SelectList> CreateCurrency(
            AnalyzeFieldEditSettings editSettings,
            ChangeEditOptions optionalData,
            ChangeAggregate change)
        {
            if (!editSettings.Cost.Show && !editSettings.YearlyCost.Show)
            {
                return new ConfigurableFieldModel<SelectList>(false);
            }

            var currencySetting = new FieldEditSetting(true, "Currency", true, null);

            string selectedValue = null;

            if (change != null && change.Analyze.CurrencyId.HasValue)
            {
                selectedValue = change.Analyze.CurrencyId.ToString();
            }

            return this.configurableFieldModelFactory.CreateSelectListField(
                currencySetting,
                optionalData.Currencies,
                selectedValue);
        }

        private ConfigurableFieldModel<int> CreateTimeEstimatesHours(
            AnalyzeFieldEditSettings editSettings,
            ChangeAggregate change)
        {
            var value = change != null ? change.Analyze.TimeEstimatesHours : 0;
            return this.configurableFieldModelFactory.CreateIntegerField(editSettings.TimeEstimatesHours, value);
        }

        private ConfigurableFieldModel<string> CreateRisk(AnalyzeFieldEditSettings editSettings, ChangeAggregate change)
        {
            var value = change != null ? change.Analyze.Risk : editSettings.Risk.DefaultValue;
            return this.configurableFieldModelFactory.CreateStringField(editSettings.Risk, value);
        }

        private ConfigurableFieldModel<DateTime?> CreateStartDate(
            AnalyzeFieldEditSettings editSettings,
            ChangeAggregate change)
        {
            var value = change != null ? change.Analyze.StartDate : null;
            return this.configurableFieldModelFactory.CreateNullableDateTimeField(editSettings.StartDate, value);
        }

        private ConfigurableFieldModel<DateTime?> CreateEndDate(
            AnalyzeFieldEditSettings editSettings,
            ChangeAggregate change)
        {
            var value = change != null ? change.Analyze.EndDate : null;
            return this.configurableFieldModelFactory.CreateNullableDateTimeField(editSettings.FinishDate, value);
        }

        private ConfigurableFieldModel<bool> CreateHasImplementationPlan(
            AnalyzeFieldEditSettings editSettings,
            ChangeAggregate change)
        {
            var value = change != null ? change.Analyze.HasImplementationPlan : false;
            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.HasImplementationPlan, value);
        }

        private ConfigurableFieldModel<bool> CreateHasRecoveryPlan(
            AnalyzeFieldEditSettings editSettings,
            ChangeAggregate change)
        {
            var value = change != null ? change.Analyze.HasRecoveryPlan : false;
            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.HasRecoveryPlan, value);
        }

        private AttachedFilesContainerModel CreateAttachedFilesContainer(string id)
        {
            return new AttachedFilesContainerModel(id, Subtopic.Analyze);
        }

        private SendToDialogModel CreateSendToDialog(ChangeEditOptions optionalData)
        {
            return this.sendToDialogModelFactory.Create(
                optionalData.EmailGroups,
                optionalData.WorkingGroups,
                optionalData.Administrators);
        }

        private ConfigurableFieldModel<string> CreateRejectExplanation(
            AnalyzeFieldEditSettings editSettings,
            ChangeAggregate change)
        {
            var value = change != null ? change.Analyze.ChangeRecommendation : editSettings.Recommendation.DefaultValue;
            return this.configurableFieldModelFactory.CreateStringField(editSettings.Recommendation, value);
        }

        private ConfigurableFieldModel<SelectList> CreateApproval(AnalyzeFieldEditSettings editSettings)
        {
            var approveItem = new SelectListItem();
            approveItem.Text = Translation.Get("Approved", Enums.TranslationSource.TextTranslation);
            approveItem.Value = AnalyzeApprovalResult.Approved.ToString();

            var rejectItem = new SelectListItem();
            rejectItem.Text = Translation.Get("Reject", Enums.TranslationSource.TextTranslation);
            rejectItem.Value = AnalyzeApprovalResult.Rejected.ToString();

            var approvedItems = new List<object> { approveItem, rejectItem };
            var approvedList = new SelectList(approvedItems, "Value", "Text");

            return this.configurableFieldModelFactory.CreateSelectListField(editSettings.Approval, approvedList);
        }
    }
}