namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit.Contacts;

    public sealed class RegistrationModelFactory : IRegistrationModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        #endregion

        #region Constructors and Destructors

        public RegistrationModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public RegistrationModel Create(FindChangeResponse response)
        {
            var settings = response.EditSettings.Registration;
            var fields = response.EditData.Change.Registration;
            var options = response.EditOptions;

            var contactModels = new List<ContactModel>(6);

            for (var i = 0; i < response.EditData.Contacts.Count && i < 6; i++)
            {
                var contactModel = response.EditData.Contacts[i];

                var name = this.configurableFieldModelFactory.CreateStringField(settings.Name, contactModel.Name);
                var phone = this.configurableFieldModelFactory.CreateStringField(settings.Phone, contactModel.Phone);
                var email = this.configurableFieldModelFactory.CreateStringField(settings.Email, contactModel.Email);

                var company = this.configurableFieldModelFactory.CreateStringField(
                    settings.Company,
                    contactModel.Company);

                contactModels.Add(new ContactModel(contactModel.Id, name, phone, email, company));
            }

            for (var i = contactModels.Count; i < 6; i++)
            {
                var name = this.configurableFieldModelFactory.CreateStringField(settings.Name, null);
                var phone = this.configurableFieldModelFactory.CreateStringField(settings.Phone, null);
                var email = this.configurableFieldModelFactory.CreateStringField(settings.Email, null);
                var company = this.configurableFieldModelFactory.CreateStringField(settings.Company, null);

                contactModels.Add(new ContactModel(0, name, phone, email, company));
            }

            var contactsModel = new ContactsModel(
                contactModels[0],
                contactModels[1],
                contactModels[2],
                contactModels[3],
                contactModels[4],
                contactModels[5]);

            var textId = response.EditData.Change.Id.ToString(CultureInfo.InvariantCulture);

            var owners = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Owner,
                options.Owners,
                fields.OwnerId.ToString());

            var affectedProcesses =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedProcesses,
                    options.AffectedProcesses,
                    response.EditData.AffectedProcessIds.Select(i => i.ToString(CultureInfo.InvariantCulture)).ToList());

            var affectedDepartments =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedDepartments,
                    options.Departments,
                    response.EditData.AffectedDepartmentIds.Cast<string>().ToList());

            var description = this.configurableFieldModelFactory.CreateStringField(
                settings.Description,
                fields.Description);

            var businessBenefits = this.configurableFieldModelFactory.CreateStringField(
                settings.BusinessBenefits,
                fields.BusinessBenefits);

            var consequence = this.configurableFieldModelFactory.CreateStringField(
                settings.Consequence,
                fields.Consequence);

            var impact = this.configurableFieldModelFactory.CreateStringField(settings.Impact, fields.Impact);

            var desiredDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(
                settings.DesiredDate,
                fields.DesiredDate);

            var verified = this.configurableFieldModelFactory.CreateBooleanField(settings.Verified, fields.Verified);

            var attachedFiles = this.configurableFieldModelFactory.CreateAttachedFiles(
                settings.AttachedFiles,
                textId,
                ChangeArea.Registration,
                response.EditData.Files.Where(f => f.Subtopic == ChangeArea.Registration).Select(f => f.Name).ToList());

            var approvalItems = CreateApprovalItems();
            var approval = new SelectList(approvalItems, "Value", "Text", fields.Approval);

            var approvalResults = this.configurableFieldModelFactory.CreateSelectListField(settings.Approval, approval);

            var rejectExplanation = this.configurableFieldModelFactory.CreateStringField(
                settings.RejectExplanation,
                fields.RejectExplanation);

            return new RegistrationModel(
                textId,
                contactsModel,
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
                fields.ApprovedDateAndTime,
                fields.ApprovedByUser,
                rejectExplanation);
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