namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Mobile.Models.Changes.ChangeEdit;
    using DH.Helpdesk.Mobile.Models.Changes.ChangeEdit.Contacts;

    public sealed class NewRegistrationModelFactory : INewRegistrationModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        #endregion

        #region Constructors and Destructors

        public NewRegistrationModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public RegistrationModel Create(
            string temporaryId,
            RegistrationEditSettings settings,
            ChangeEditOptions options)
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

            var owners = this.configurableFieldModelFactory.CreateSelectListField(settings.Owner, options.Owners, null, true);

            var affectedProcesses =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedProcesses,
                    options.AffectedProcesses,
                    null);

            var affectedDepartments =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedDepartments,
                    options.AffectedDepartments,
                    null);

            var description = this.configurableFieldModelFactory.CreateStringField(settings.Description, null);
            var businessBenefits = this.configurableFieldModelFactory.CreateStringField(settings.BusinessBenefits, null);
            var consequence = this.configurableFieldModelFactory.CreateStringField(settings.Consequence, null);
            var impact = this.configurableFieldModelFactory.CreateStringField(settings.Impact, null);

            var desiredDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(settings.DesiredDate, null);
            var verified = this.configurableFieldModelFactory.CreateBooleanField(settings.Verified, false);

            var attachedFiles = this.configurableFieldModelFactory.CreateAttachedFiles(
                settings.AttachedFiles,
                temporaryId,
                Subtopic.Registration,
                new List<string>(0));

            var approvalItems = CreateApprovalItems();

            var approvalResults = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Approval,
                approvalItems);

            var rejectExplanation = this.configurableFieldModelFactory.CreateStringField(
                settings.RejectExplanation,
                null);

            return new RegistrationModel(
                temporaryId,
                contacts,
                owners,
                affectedProcesses,
                affectedDepartments,
                description,
                businessBenefits,
                consequence,
                impact,
                desiredDate,
                verified,
                attachedFiles,
                approvalResults,
                null,
                null,
                rejectExplanation);
        }

        #endregion

        #region Methods

        private static SelectList CreateApprovalItems()
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

            return new SelectList(new List<object> { noneItem, approveItem, rejectItem }, "Value", "Text");
        }

        private ContactModel CreateEmptyContact(RegistrationEditSettings settings)
        {
            var name = this.configurableFieldModelFactory.CreateStringField(settings.Name, null);
            var phone = this.configurableFieldModelFactory.CreateStringField(settings.Phone, null);
            var email = this.configurableFieldModelFactory.CreateStringField(settings.Email, null);
            var company = this.configurableFieldModelFactory.CreateStringField(settings.Company, null);

            return new ContactModel(0, name, phone, email, company);
        }

        #endregion
    }
}