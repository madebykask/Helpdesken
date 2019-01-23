using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit.Contacts;

    public sealed class NewChangeRequestFactory : INewChangeRequestFactory
    {
        #region Public Methods and Operators

        public NewChangeRequest Create(
            InputModel model,
            List<WebTemporaryFile> registrationFiles,
            OperationContext context,
            IEmailService emailService)
        {
            var newChange = CreateNewChange(model, context);
            var newContacts = CreateNewContactCollection(model, context);
            var newFiles = CreateNewFileCollection(registrationFiles, context);
            var newLogs = CreateNewLogCollection(model, emailService);

            return new NewChangeRequest(
                newChange,
                newContacts,
                model.Registration.AffectedProcessIds,
                model.Registration.AffectedDepartmentIds,
                newFiles,
                newLogs,
                context);
        }

        #endregion

        #region Methods

        private static void CreateContactIfNeeded(ContactModel model, OperationContext context, List<Contact> contacts)
        {
            if ((model.Name == null || string.IsNullOrEmpty(model.Name.Value))
                && (model.Phone == null || string.IsNullOrEmpty(model.Phone.Value))
                && (model.Email == null || string.IsNullOrEmpty(model.Email.Value))
                && (model.Company == null || string.IsNullOrEmpty(model.Company.Value)))
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
            var orderer = CreateNewOrdererPart(model.Orderer, context);
            var general = CreateNewGeneralPart(model.General, context);
            var registration = CreateNewRegistrationPart(model.Registration, context);

            return new NewChange(context.CustomerId, context.LanguageId, orderer, general, registration);
        }

        private static List<Contact> CreateNewContactCollection(InputModel model, OperationContext context)
        {
            var contacts = new List<Contact>();

            if (model == null || model.Registration == null || model.Registration.Contacts == null)
            {
                return contacts;
            }

            CreateContactIfNeeded(model.Registration.Contacts.ContactOne, context, contacts);
            CreateContactIfNeeded(model.Registration.Contacts.ContactTwo, context, contacts);
            CreateContactIfNeeded(model.Registration.Contacts.ContactThree, context, contacts);
            CreateContactIfNeeded(model.Registration.Contacts.ContactFourth, context, contacts);
            CreateContactIfNeeded(model.Registration.Contacts.ContactFive, context, contacts);
            CreateContactIfNeeded(model.Registration.Contacts.ContactSix, context, contacts);

            return contacts;
        }

        private static List<NewFile> CreateNewFileCollection(
            List<WebTemporaryFile> registrationFiles,
            OperationContext context)
        {
            return
                registrationFiles.Select(
                    f => new NewFile(Subtopic.Registration, f.Content, f.Name, context.DateAndTime)).ToList();
        }

        private static NewGeneralFields CreateNewGeneralPart(GeneralModel model, OperationContext context)
        {
            if (model == null)
            {
                return null;
            }

            return new NewGeneralFields(
                ConfigurableFieldModel<int>.GetValueOrDefault(model.Prioritisation),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Title),
                model.StatusId,
                model.SystemId,
                model.ObjectId,
                model.InventoryDialog != null ? model.InventoryDialog.Value.SelectedInventories : new List<string>(),
                model.WorkingGroupId,
                model.AdministratorId,
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishingDate),
                context.DateAndTime,
                context.UserId,
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.Rss));
        }

        private static List<ManualLog> CreateNewLogCollection(InputModel model, IEmailService emailService)
        {
            var newLogs = new List<ManualLog>();

            if (model.Log != null && model.Log.Logs != null)
                CreateNewLogIfNeeded(model.Log.Logs.Value, Subtopic.Log, newLogs, emailService);

            return newLogs;
        }

        private static void CreateNewLogIfNeeded(
                        LogsModel model, 
                        Subtopic area, 
                        List<ManualLog> logs,
                        IEmailService emailService)
        {
            if (string.IsNullOrEmpty(model.Text))
            {
                return;
            }

            var emails = string.IsNullOrEmpty(model.Emails)
                ? new List<MailAddress>(0)
                : model.Emails.Split(Environment.NewLine)
                            .Where(emailService.IsValidEmail)
                            .Select(e => new MailAddress(e))
                            .ToList();

            var newLog = ManualLog.CreateNew(model.Text, emails, area);
            logs.Add(newLog);
        }

        private static NewOrdererFields CreateNewOrdererPart(OrdererModel model, OperationContext context)
        {
            if (model == null)
            {
                return null;
            }

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