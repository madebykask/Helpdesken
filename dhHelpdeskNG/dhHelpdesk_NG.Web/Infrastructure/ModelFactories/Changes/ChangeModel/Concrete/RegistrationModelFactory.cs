namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class RegistrationModelFactory : IRegistrationModelFactory
    {
        //        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;
        //
        //        public RegistrationModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        //        {
        //            this.configurableFieldModelFactory = configurableFieldModelFactory;
        //        }
        //
        //        public RegistrationModel Create(
        //            string temporaryId,
        //            RegistrationFieldEditSettings editSettings,
        //            ChangeEditData editData)
        //        {
        //            var owner = this.CreateOwner(null, editData);
        //            var processesAffected = this.CreateProcessesAffected(null, editSettings, editData);
        //            var departmentsAffected = this.CreateDepartmentsAffected(null, editSettings, editData);
        //            var description = this.CreateDescription(null, editSettings);
        //            var businessBenefits = this.CreateBusinessBenefits(null, editSettings);
        //            var consequence = this.CreateConsequence(null, editSettings);
        //            var impact = this.CreateImpact(null, editSettings);
        //            var desiredDate = this.CreateDesiredDate(null, editSettings);
        //            var verified = this.CreateVerified(null, editSettings);
        //
        //            var attachedFilesContainer = new AttachedFilesModel(temporaryId, Subtopic.Registration);
        //            var approval = this.CreateApproval(null);
        //
        //            return new RegistrationModel(
        //                temporaryId,
        //                owner,
        //                processesAffected,
        //                departmentsAffected,
        //                description,
        //                businessBenefits,
        //                consequence,
        //                impact,
        //                desiredDate,
        //                verified,
        //                attachedFilesContainer,
        //                approval,
        //                null,
        //                null,
        //                null);
        //        }
        //
        //        public RegistrationModel Create(
        //            ChangeAggregate change,
        //            RegistrationFieldEditSettings editSettings,
        //            ChangeEditData editData)
        //        {
        //            var id = change.Id.ToString(CultureInfo.InvariantCulture);
        //            var owner = this.CreateOwner(change, editData);
        //            var processesAffected = this.CreateProcessesAffected(change, editSettings, editData);
        //            var departmentsAffected = this.CreateDepartmentsAffected(change, editSettings, editData);
        //            var description = this.CreateDescription(change, editSettings);
        //            var businessBenefits = this.CreateBusinessBenefits(change, editSettings);
        //            var consequence = this.CreateConsequence(change, editSettings);
        //            var impact = this.CreateImpact(change, editSettings);
        //            var desiredDate = this.CreateDesiredDate(change, editSettings);
        //            var verified = this.CreateVerified(change, editSettings);
        //
        //            var attachedFilesContainer =
        //                new AttachedFilesModel(change.Id.ToString(CultureInfo.InvariantCulture), Subtopic.Registration);
        //
        //            var approval = this.CreateApproval(change);
        //
        //            return new RegistrationModel(
        //                id,
        //                owner,
        //                processesAffected,
        //                departmentsAffected,
        //                description,
        //                businessBenefits,
        //                consequence,
        //                impact,
        //                desiredDate,
        //                verified,
        //                attachedFilesContainer,
        //                approval,
        //                change.Registration.ApprovalExplanation,
        //                change.Registration.ApprovedDateAndTime,
        //                change.Registration.ApprovedUser);
        //        }
        //
        //        private SelectList CreateOwner(ChangeAggregate change, ChangeEditData editData)
        //        {
        //            var value = change != null ? change.Registration.OwnerId : null;
        //            return new SelectList(editData.Owners, "Value", "Name", value);
        //        }
        //
        //        private ConfigurableFieldModel<MultiSelectList> CreateProcessesAffected(
        //            ChangeAggregate change,
        //            RegistrationFieldEditSettings editSettings,
        //            ChangeEditData editData)
        //        {
        //            List<string> selectedValues = null;
        //
        //            if (change != null)
        //            {
        //                selectedValues =
        //                    change.Registration.ProcessesAffectedIds.Select(i => i.ToString(CultureInfo.InvariantCulture))
        //                        .ToList();
        //            }
        //
        //            return this.configurableFieldModelFactory.CreateMultiSelectListField(
        //                editSettings.AffectedProcesses,
        //                editData.AffectedProcesses,
        //                selectedValues);
        //        }
        //
        //        private ConfigurableFieldModel<MultiSelectList> CreateDepartmentsAffected(
        //            ChangeAggregate change,
        //            RegistrationFieldEditSettings editSettings,
        //            ChangeEditData editData)
        //        {
        //            List<string> selectedValues = null;
        //
        //            if (change != null)
        //            {
        //                selectedValues =
        //                    change.Registration.DepartmentAffectedIds.Select(i => i.ToString(CultureInfo.InvariantCulture))
        //                        .ToList();
        //            }
        //
        //            return this.configurableFieldModelFactory.CreateMultiSelectListField(
        //                editSettings.AffectedDepartments,
        //                editData.Departments,
        //                selectedValues);
        //        }
        //
        //        private ConfigurableFieldModel<string> CreateDescription(
        //            ChangeAggregate change,
        //            RegistrationFieldEditSettings editSettings)
        //        {
        //            var value = change != null ? change.Registration.Description : editSettings.Description.DefaultValue;
        //            return this.configurableFieldModelFactory.CreateStringField(editSettings.Description, value);
        //        }
        //
        //        private ConfigurableFieldModel<string> CreateBusinessBenefits(
        //            ChangeAggregate change,
        //            RegistrationFieldEditSettings editSettings)
        //        {
        //            var value = change != null
        //                ? change.Registration.BusinessBenefits
        //                : editSettings.BusinessBenefits.DefaultValue;
        //
        //            return this.configurableFieldModelFactory.CreateStringField(editSettings.BusinessBenefits, value);
        //        }
        //
        //        private ConfigurableFieldModel<string> CreateConsequence(
        //            ChangeAggregate change,
        //            RegistrationFieldEditSettings editSettings)
        //        {
        //            var value = change != null ? change.Registration.Consequece : editSettings.Consequence.DefaultValue;
        //            return this.configurableFieldModelFactory.CreateStringField(editSettings.Consequence, value);
        //        }
        //
        //        private ConfigurableFieldModel<string> CreateImpact(
        //            ChangeAggregate change,
        //            RegistrationFieldEditSettings editSettings)
        //        {
        //            var value = change != null ? change.Registration.Impact : null;
        //            return this.configurableFieldModelFactory.CreateStringField(editSettings.Impact, value);
        //        }
        //
        //        private ConfigurableFieldModel<DateTime?> CreateDesiredDate(
        //            ChangeAggregate change,
        //            RegistrationFieldEditSettings editSettings)
        //        {
        //            var value = change != null ? change.Registration.DesiredDate : null;
        //            return this.configurableFieldModelFactory.CreateNullableDateTimeField(editSettings.DesiredDate, value);
        //        }
        //
        //        private ConfigurableFieldModel<bool> CreateVerified(
        //            ChangeAggregate change,
        //            RegistrationFieldEditSettings editSettings)
        //        {
        //            var value = change != null ? change.Registration.Verified : false;
        //            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.Verified, value);
        //        }
        //
        //        private SelectList CreateApproval(ChangeAggregate change)
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
        //            var selectedValue = change != null ? change.Registration.Approved : RegistrationApprovalResult.None;
        //            return new SelectList(approvedItems, "Value", "Text", selectedValue);
        //        }

        #region Public Methods and Operators

        public RegistrationModel Create(
            string temporaryId,
            RegistrationFieldEditSettings editSettings,
            ChangeEditData editData)
        {
            throw new NotImplementedException();
        }

        public RegistrationModel Create(
            Change change,
            RegistrationFieldEditSettings editSettings,
            ChangeEditData editData)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}