namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewRegistrationModelFactory : INewRegistrationModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public NewRegistrationModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        private ContactModel CreateEmptyContact(RegistrationEditSettings settings)
        {
            var name = this.configurableFieldModelFactory.CreateStringField(settings.Name, null);
            var phone = this.configurableFieldModelFactory.CreateStringField(settings.Phone, null);
            var email = this.configurableFieldModelFactory.CreateStringField(settings.Email, null);
            var company = this.configurableFieldModelFactory.CreateStringField(settings.Company, null);

            return new ContactModel(0, name, phone, email, company);
        }

        public RegistrationModel Create(string temporaryId, ChangeEditData editData, RegistrationEditSettings settings)
        {
            var contactOne = this.CreateEmptyContact(settings);
            var contactTwo = this.CreateEmptyContact(settings);
            var contactThree = this.CreateEmptyContact(settings);
            var contactFourth = this.CreateEmptyContact(settings);
            var contactFive = this.CreateEmptyContact(settings);
            var contactSix = this.CreateEmptyContact(settings);

            var contacts = new ContactsModel(
                contactOne,
                contactTwo,
                contactThree,
                contactFourth,
                contactFive,
                contactSix);
                
            var owner = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Owner, editData.Owners, null);

            var affectedProcesses =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedProcesses, editData.AffectedProcesses, new List<object>(0));

            var affectedDepartments =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedDepartments, editData.AffectedDepartments, new List<object>(0));

            var description = this.configurableFieldModelFactory.CreateStringField(settings.Description, null);
            var businessBenefits = this.configurableFieldModelFactory.CreateStringField(settings.BusinessBenefits, null);
            var consequence = this.configurableFieldModelFactory.CreateStringField(settings.Consequence, null);
            var impact = this.configurableFieldModelFactory.CreateStringField(settings.Impact, null);

            var desiredDateAndTime = this.configurableFieldModelFactory.CreateDateTimeField(settings.DesiredDate, null);
            var verified = this.configurableFieldModelFactory.CreateBooleanField(settings.Verified, false);

            var attachedFiles = this.configurableFieldModelFactory.CreateAttachedFiles(
                settings.AttachedFiles,
                temporaryId,
                Subtopic.Registration,
                new List<File>(0));

            var approvalItems = CreateApprovalItems();

            var approval = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Approval, approvalItems, null);

            var rejectExplanation = this.configurableFieldModelFactory.CreateStringField(
                settings.RejectExplanation, null);

            return new RegistrationModel(
                temporaryId,
                contacts,
                owner,
                affectedProcesses,
                affectedDepartments,
                description,
                businessBenefits,
                consequence,
                impact,
                desiredDateAndTime,
                verified,
                attachedFiles,
                approval,
                null,
                null,
                rejectExplanation);
        }

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
    }
}