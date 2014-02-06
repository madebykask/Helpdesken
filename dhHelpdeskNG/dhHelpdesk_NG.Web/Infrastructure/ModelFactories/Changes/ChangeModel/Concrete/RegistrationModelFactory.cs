namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

    public sealed class RegistrationModelFactory : IRegistrationModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public RegistrationModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public RegistrationModel Create(
            string temporaryId,
            RegistrationFieldEditSettings editSettings,
            ChangeOptionalData optionalData)
        {
            var owner = this.CreateOwner(null, optionalData);
            var processesAffected = this.CreateProcessesAffected(null, editSettings, optionalData);
            var departmentsAffected = this.CreateDepartmentsAffected(null, editSettings, optionalData);
            var description = this.CreateDescription(null, editSettings);
            var businessBenefits = this.CreateBusinessBenefits(null, editSettings);
            var consequence = this.CreateConsequence(null, editSettings);
            var impact = this.CreateImpact(null, editSettings);
            var desiredDate = this.CreateDesiredDate(null, editSettings);
            var verified = this.CreateVerified(null, editSettings);

            var attachedFilesContainer = new AttachedFilesContainerModel(temporaryId, Subtopic.Registration);
            var approval = this.CreateApproval(null);

            return new RegistrationModel(
                temporaryId,
                owner,
                processesAffected,
                departmentsAffected,
                description,
                businessBenefits,
                consequence,
                impact,
                desiredDate,
                verified,
                attachedFilesContainer,
                approval,
                null,
                null,
                null);
        }

        public RegistrationModel Create(
            ChangeAggregate change,
            RegistrationFieldEditSettings editSettings,
            ChangeOptionalData optionalData)
        {
            var id = change.Id.ToString(CultureInfo.InvariantCulture);
            var owner = this.CreateOwner(change, optionalData);
            var processesAffected = this.CreateProcessesAffected(change, editSettings, optionalData);
            var departmentsAffected = this.CreateDepartmentsAffected(change, editSettings, optionalData);
            var description = this.CreateDescription(change, editSettings);
            var businessBenefits = this.CreateBusinessBenefits(change, editSettings);
            var consequence = this.CreateConsequence(change, editSettings);
            var impact = this.CreateImpact(change, editSettings);
            var desiredDate = this.CreateDesiredDate(change, editSettings);
            var verified = this.CreateVerified(change, editSettings);

            var attachedFilesContainer =
                new AttachedFilesContainerModel(change.Id.ToString(CultureInfo.InvariantCulture), Subtopic.Registration);

            var approval = this.CreateApproval(change);

            return new RegistrationModel(
                id,
                owner,
                processesAffected,
                departmentsAffected,
                description,
                businessBenefits,
                consequence,
                impact,
                desiredDate,
                verified,
                attachedFilesContainer,
                approval,
                change.Registration.ApprovalExplanation,
                change.Registration.ApprovedDateAndTime,
                change.Registration.ApprovedUser);
        }

        private SelectList CreateOwner(ChangeAggregate change, ChangeOptionalData optionalData)
        {
            var value = change != null ? change.Registration.OwnerId : null;
            return new SelectList(optionalData.Owners, "Value", "Name", value);
        }

        private ConfigurableFieldModel<MultiSelectList> CreateProcessesAffected(
            ChangeAggregate change,
            RegistrationFieldEditSettings editSettings,
            ChangeOptionalData optionalData)
        {
            List<string> selectedValues = null;

            if (change != null)
            {
                selectedValues =
                    change.Registration.ProcessesAffectedIds.Select(i => i.ToString(CultureInfo.InvariantCulture))
                        .ToList();
            }

            return this.configurableFieldModelFactory.CreateMultiSelectListField(
                editSettings.ProcessesAffected,
                optionalData.ProcessesAffected,
                selectedValues);
        }

        private ConfigurableFieldModel<MultiSelectList> CreateDepartmentsAffected(
            ChangeAggregate change,
            RegistrationFieldEditSettings editSettings,
            ChangeOptionalData optionalData)
        {
            List<string> selectedValues = null;

            if (change != null)
            {
                selectedValues =
                    change.Registration.DepartmentAffectedIds.Select(i => i.ToString(CultureInfo.InvariantCulture))
                        .ToList();
            }

            return this.configurableFieldModelFactory.CreateMultiSelectListField(
                editSettings.DepartmentsAffected,
                optionalData.Departments,
                selectedValues);
        }

        private ConfigurableFieldModel<string> CreateDescription(
            ChangeAggregate change,
            RegistrationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Registration.Description : editSettings.Description.DefaultValue;
            return this.configurableFieldModelFactory.CreateStringField(editSettings.Description, value);
        }

        private ConfigurableFieldModel<string> CreateBusinessBenefits(
            ChangeAggregate change,
            RegistrationFieldEditSettings editSettings)
        {
            var value = change != null
                ? change.Registration.BusinessBenefits
                : editSettings.BusinessBenefits.DefaultValue;

            return this.configurableFieldModelFactory.CreateStringField(editSettings.BusinessBenefits, value);
        }

        private ConfigurableFieldModel<string> CreateConsequence(
            ChangeAggregate change,
            RegistrationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Registration.Consequece : editSettings.Consequence.DefaultValue;
            return this.configurableFieldModelFactory.CreateStringField(editSettings.Consequence, value);
        }

        private ConfigurableFieldModel<string> CreateImpact(
            ChangeAggregate change,
            RegistrationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Registration.Impact : null;
            return this.configurableFieldModelFactory.CreateStringField(editSettings.Impact, value);
        }

        private ConfigurableFieldModel<DateTime?> CreateDesiredDate(
            ChangeAggregate change,
            RegistrationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Registration.DesiredDate : null;
            return this.configurableFieldModelFactory.CreateNullableDateTimeField(editSettings.DesiredDate, value);
        }

        private ConfigurableFieldModel<bool> CreateVerified(
            ChangeAggregate change,
            RegistrationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Registration.Verified : false;
            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.Verified, value);
        }

        private SelectList CreateApproval(ChangeAggregate change)
        {
            var approveItem = new SelectListItem();
            approveItem.Text = Translation.Get("Approved", Enums.TranslationSource.TextTranslation);
            approveItem.Value = AnalyzeApproveResult.Approved.ToString();

            var rejectItem = new SelectListItem();
            rejectItem.Text = Translation.Get("Reject", Enums.TranslationSource.TextTranslation);
            rejectItem.Value = AnalyzeApproveResult.Rejected.ToString();

            var approvedItems = new List<object> { approveItem, rejectItem };
            var selectedValue = change != null ? change.Registration.Approved : RegistrationApproveResult.None;
            return new SelectList(approvedItems, "Value", "Text", selectedValue);
        }
    }
}