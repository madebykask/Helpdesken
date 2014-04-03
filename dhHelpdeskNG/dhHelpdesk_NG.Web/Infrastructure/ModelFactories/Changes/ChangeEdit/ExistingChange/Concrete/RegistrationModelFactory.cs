namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class RegistrationModelFactory : IRegistrationModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public RegistrationModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public RegistrationModel Create(
            FindChangeResponse response, ChangeEditData editData, RegistrationEditSettings settings)
        {
            var contacts = new List<ContactModel>(6);

            for (var i = 0; i < response.Contacts.Count && i < 6; i++)
            {
                var contact = response.Contacts[i];

                var name = this.configurableFieldModelFactory.CreateStringField(settings.Name, contact.Name);
                var phone = this.configurableFieldModelFactory.CreateStringField(settings.Phone, contact.Phone);
                var email = this.configurableFieldModelFactory.CreateStringField(settings.Email, contact.Email);
                var company = this.configurableFieldModelFactory.CreateStringField(settings.Company, contact.Company);

                contacts.Add(new ContactModel(contact.Id, name, phone, email, company));
            }

            for (var i = contacts.Count; i < 6; i++)
            {
                var name = this.configurableFieldModelFactory.CreateStringField(settings.Name, null);
                var phone = this.configurableFieldModelFactory.CreateStringField(settings.Phone, null);
                var email = this.configurableFieldModelFactory.CreateStringField(settings.Email, null);
                var company = this.configurableFieldModelFactory.CreateStringField(settings.Company, null);

                contacts.Add(new ContactModel(0, name, phone, email, company));
            }

            var contactsModel = new ContactsModel(
                contacts[0],
                contacts[1],
                contacts[2],
                contacts[3],
                contacts[4],
                contacts[5]);

            var textId = response.Change.Id.ToString(CultureInfo.InvariantCulture);
            var registration = response.Change.Registration;

            var owner = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Owner, editData.Owners, registration.OwnerId);

            var affectedProcesses =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedProcesses,
                    editData.AffectedProcesses,
                    response.AffectedProcessIds.Cast<object>().ToList());

            var affectedDepartments =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedDepartments,
                    editData.Departments,
                    response.AffectedDepartmentIds.Cast<object>().ToList());

            var description = this.configurableFieldModelFactory.CreateStringField(
                settings.Description, registration.Description);

            var businessBenefits = this.configurableFieldModelFactory.CreateStringField(
                settings.BusinessBenefits, response.Change.Registration.BusinessBenefits);

            var consequence = this.configurableFieldModelFactory.CreateStringField(
                settings.Consequence, response.Change.Registration.Consequence);

            var impact = this.configurableFieldModelFactory.CreateStringField(
                settings.Impact, response.Change.Registration.Impact);

            var desiredDate = this.configurableFieldModelFactory.CreateDateTimeField(
                settings.DesiredDate, response.Change.Registration.DesiredDate);

            var verified = this.configurableFieldModelFactory.CreateBooleanField(
                settings.Verified, response.Change.Registration.Verified);

            var attachedFiles = this.configurableFieldModelFactory.CreateAttachedFiles(
                settings.AttachedFiles, textId, Subtopic.Registration, response.Files);

            var approvalItems = CreateApprovalItems();

            var approval = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Approval, approvalItems, registration.Approval);

            var rejectExplanation = this.configurableFieldModelFactory.CreateStringField(
                settings.RejectExplanation, response.Change.Registration.RejectExplanation);

            return new RegistrationModel(
                textId,
                contactsModel,
                owner,
                affectedProcesses,
                affectedDepartments,
                description,
                businessBenefits,
                consequence,
                impact,
                desiredDate,
                verified,
                attachedFiles,
                approval,
                response.Change.Registration.ApprovedDateAndTime,
                response.Change.Registration.ApprovedByUser,
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