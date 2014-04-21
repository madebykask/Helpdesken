namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit.Contacts;

    public sealed class NewChangeRequestFactory : INewChangeRequestFactory
    {
        #region Public Methods and Operators

        public NewChangeRequest Create(
            InputModel model,
            List<WebTemporaryFile> registrationFiles,
            OperationContext context)
        {
            var newChange = CreateNewChange(model, context);
            var newContacts = CreateNewContactCollection(model, context);
            var newFiles = CreateNewFileCollection(registrationFiles, context);

            return new NewChangeRequest(
                newChange,
                newContacts,
                model.RegistrationModel.AffectedProcessIds,
                model.RegistrationModel.AffectedDepartmentIds,
                newFiles);
        }

        #endregion

        #region Methods

        private static void CreateContactIfNeeded(ContactModel model, OperationContext context, List<Contact> contacts)
        {
            if (string.IsNullOrEmpty(model.Name.Value) && string.IsNullOrEmpty(model.Phone.Value)
                && string.IsNullOrEmpty(model.Email.Value) && string.IsNullOrEmpty(model.Company.Value))
            {
                return;
            }

            var contact = Contact.CreateNew(
                model.Name.Value,
                model.Phone.Value,
                model.Email.Value,
                model.Company.Value,
                context.DateAndTime);

            contacts.Add(contact);
        }

        private static NewChange CreateNewChange(InputModel model, OperationContext context)
        {
            var orderer = CreateNewOrdererPart(model.OrdererModel, context);
            var general = CreateNewGeneralPart(model.GeneralModel, context);
            var registration = CreateNewRegistrationPart(model.RegistrationModel, context);

            return new NewChange(context.CustomerId, context.LanguageId, orderer, general, registration);
        }

        private static List<Contact> CreateNewContactCollection(InputModel model, OperationContext context)
        {
            var contacts = new List<Contact>();

            CreateContactIfNeeded(model.RegistrationModel.Contacts.ContactOne, context, contacts);
            CreateContactIfNeeded(model.RegistrationModel.Contacts.ContactTwo, context, contacts);
            CreateContactIfNeeded(model.RegistrationModel.Contacts.ContactThree, context, contacts);
            CreateContactIfNeeded(model.RegistrationModel.Contacts.ContactFourth, context, contacts);
            CreateContactIfNeeded(model.RegistrationModel.Contacts.ContactFive, context, contacts);
            CreateContactIfNeeded(model.RegistrationModel.Contacts.ContactSix, context, contacts);

            return contacts;
        }

        private static List<NewFile> CreateNewFileCollection(List<WebTemporaryFile> registrationFiles, OperationContext context)
        {
            return
                registrationFiles.Select(
                    f => new NewFile(ChangeArea.Registration, f.Content, f.Name, context.DateAndTime)).ToList();
        }

        private static NewGeneralFields CreateNewGeneralPart(GeneralModel model, OperationContext context)
        {
            return new NewGeneralFields(
                ConfigurableFieldModel<int>.GetValueOrDefault(model.Prioritisation),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Title),
                model.StatusId,
                model.SystemId,
                model.ObjectId,
                model.WorkingGroupId,
                model.AdministratorId,
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishingDate),
                context.DateAndTime,
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.Rss));
        }

        private static NewOrdererFields CreateNewOrdererPart(OrdererModel model, OperationContext context)
        {
            return new NewOrdererFields(
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Id),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Name),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Phone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.CellPhone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Email),
                model.DepartmentId);
        }

        private static NewRegistrationFields CreateNewRegistrationPart(
            RegistrationModel model,
            OperationContext context)
        {
            DateTime? approvedDateAndTime = null;
            int? approvedByUserId = null;

            if (model.ApprovalValue == StepStatus.Approved)
            {
                approvedDateAndTime = context.DateAndTime;
                approvedByUserId = context.UserId;
            }

            return new NewRegistrationFields(
                model.OwnerId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Description),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.BusinessBenefits),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Consequence),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Impact),
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.DesiredDate),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.Verified),
                model.ApprovalValue,
                approvedDateAndTime,
                approvedByUserId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.RejectExplanation));
        }

        #endregion
    }
}