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
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit.Contacts;

    public sealed class UpdateChangeRequestFactory : IUpdateChangeRequestFactory
    {
        #region Public Methods and Operators

        public UpdateChangeRequest Create(
            InputModel model,
            List<string> deletedRegistrationFiles,
            List<string> deletedAnalyzeFiles,
            List<string> deletedImplementationFiles,
            List<string> deletedEvaluationFiles,
            List<int> deletedLogIds,
            List<WebTemporaryFile> newRegistrationFiles,
            List<WebTemporaryFile> newAnalyzeFiles,
            List<WebTemporaryFile> newImplementationFiles,
            List<WebTemporaryFile> newEvaluationFiles,
            OperationContext context,
            IEmailService emailService)
        {
            var updatedChange = CreateUpdatedChange(model, context);
            var contacts = CreateContactCollection(model, context);

            var deletedFiles = CreateDeletedFileCollection(
                deletedRegistrationFiles,
                deletedAnalyzeFiles,
                deletedImplementationFiles,
                deletedEvaluationFiles);

            var newFiles = CreateNewFileCollection(
                newRegistrationFiles,
                newAnalyzeFiles,
                newImplementationFiles,
                newEvaluationFiles,
                context);

            var newLogs = CreateNewLogCollection(model, emailService);

            return new UpdateChangeRequest(
                context,
                updatedChange,
                contacts,
                model.Registration.AffectedProcessIds,
                model.Registration.AffectedDepartmentIds,
                model.Analyze.RelatedChangeIds,
                deletedFiles,
                newFiles,
                deletedLogIds,
                newLogs);
        }

        #endregion

        #region Methods

        private static UpdatedAnalyzeFields CreateAnalyzePart(AnalyzeModel model, OperationContext context)
        {
            if (model == null)
            {
                return null;
            }

            DateTime? approvedDateAndTime = null;
            int? approvedByUserId = null;

            if (model.ApprovalValue == StepStatus.Approved)
            {
                approvedDateAndTime = context.DateAndTime;
                approvedByUserId = context.UserId;
            }

            var estimatedTimeInHours = ConfigurableFieldModel<int>.GetValueOrDefault(model.EstimatedTimeInHours);
            var risk = ConfigurableFieldModel<string>.GetValueOrDefault(model.Risk);

            DateTime? startDateAndTime = null;

            if (model.StartDateAndTime != null)
            {
                var startDate =
                    ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.StartDateAndTime.Container.Date);

                if (startDate.HasValue)
                {
                    var startTime =
                        ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.StartDateAndTime.Container.Time);

                    if (startTime.HasValue)
                    {
                        startDateAndTime = new DateTime(
                            startDate.Value.Year,
                            startDate.Value.Month,
                            startDate.Value.Day,
                            startTime.Value.Hour,
                            startTime.Value.Minute,
                            0);
                    }
                    else
                    {
                        startDateAndTime = new DateTime(
                            startDate.Value.Year,
                            startDate.Value.Month,
                            startDate.Value.Day);
                    }
                }
            }

            DateTime? finishDateAndTime = null;

            if (model.FinishDateAndTime != null)
            {
                var finishDate =
                    ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishDateAndTime.Container.Date);

                if (finishDate.HasValue)
                {
                    var finishTime =
                        ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishDateAndTime.Container.Time);

                    if (finishTime.HasValue)
                    {
                        finishDateAndTime = new DateTime(
                            finishDate.Value.Year,
                            finishDate.Value.Month,
                            finishDate.Value.Day,
                            finishTime.Value.Hour,
                            finishTime.Value.Minute,
                            0);
                    }
                    else
                    {
                        finishDateAndTime = new DateTime(
                            finishDate.Value.Year,
                            finishDate.Value.Month,
                            finishDate.Value.Day);
                    }
                }
            }

            var hasImplementationPlan = ConfigurableFieldModel<bool>.GetValueOrDefault(model.HasImplementationPlan);
            var hasRecoveryPlan = ConfigurableFieldModel<bool>.GetValueOrDefault(model.HasRecoveryPlan);
            var rejectExplanation = ConfigurableFieldModel<string>.GetValueOrDefault(model.RejectExplanation);
            var logNote = model.Logs != null && model.Logs.Value != null ? 
                        model.Logs.Value.Text : string.Empty;

            return new UpdatedAnalyzeFields(
                model.CategoryId,
                model.PriorityId,
                model.ResponsibleId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Solution),
                ConfigurableFieldModel<int>.GetValueOrDefault(model.Cost),
                ConfigurableFieldModel<int>.GetValueOrDefault(model.YearlyCost),
                model.CurrencyId,
                estimatedTimeInHours,
                risk,
                startDateAndTime,
                finishDateAndTime,
                hasImplementationPlan,
                hasRecoveryPlan,
                model.ApprovalValue,
                approvedDateAndTime,
                approvedByUserId,
                rejectExplanation,
                logNote);
        }

        private static List<Contact> CreateContactCollection(InputModel model, OperationContext context)
        {
            var contacts = new List<Contact>();

            if (model == null ||
                model.Registration == null ||
                model.Registration.Contacts == null)
            {
                return contacts;
            }

            var changeId = int.Parse(model.Id);

            CreateContactIfNeeded(model.Registration.Contacts.ContactOne, context, changeId, contacts);

            CreateContactIfNeeded(model.Registration.Contacts.ContactTwo, context, changeId, contacts);

            CreateContactIfNeeded(model.Registration.Contacts.ContactThree, context, changeId, contacts);

            CreateContactIfNeeded(model.Registration.Contacts.ContactFourth, context, changeId, contacts);

            CreateContactIfNeeded(model.Registration.Contacts.ContactFive, context, changeId, contacts);

            CreateContactIfNeeded(model.Registration.Contacts.ContactSix, context, changeId, contacts);

            return contacts;
        }

        private static void CreateContactIfNeeded(
            ContactModel model,
            OperationContext context,
            int changeId,
            List<Contact> contacts)
        {
            if (model == null)
            {
                return;
            }

            if ((model.Name == null || string.IsNullOrEmpty(model.Name.Value))
                && (model.Phone == null || string.IsNullOrEmpty(model.Phone.Value))
                && (model.Email == null || string.IsNullOrEmpty(model.Email.Value))
                && (model.Company == null || string.IsNullOrEmpty(model.Company.Value)))
            {
                return;
            }

            Contact contact;
            var name = model.Name != null ? model.Name.Value : string.Empty;
            var phone = model.Phone != null ? model.Phone.Value : string.Empty;
            var email = model.Email != null ? model.Email.Value : string.Empty;
            var company = model.Company != null ? model.Company.Value : string.Empty;

            if (model.Id == 0)
            {
                contact = Contact.CreateNew(
                    name,
                    phone,
                    email,
                    company,
                    context.DateAndTime);
            }
            else
            {
                contact = Contact.CreateUpdated(
                    model.Id,
                    changeId,
                    name,
                    phone,
                    email,
                    company,
                    context.DateAndTime);
            }

            contacts.Add(contact);
        }

        private static List<DeletedFile> CreateDeletedFileCollection(
            List<string> deletedRegistrationFiles,
            List<string> deletedAnalyzeFiles,
            List<string> deletedImplementationFiles,
            List<string> deletedEvaluationFiles)
        {
            var deletedFiles = new List<DeletedFile>();

            deletedFiles.AddRange(deletedRegistrationFiles.Select(f => new DeletedFile(Subtopic.Registration, f)));
            deletedFiles.AddRange(deletedAnalyzeFiles.Select(f => new DeletedFile(Subtopic.Analyze, f)));
            deletedFiles.AddRange(deletedImplementationFiles.Select(f => new DeletedFile(Subtopic.Implementation, f)));
            deletedFiles.AddRange(deletedEvaluationFiles.Select(f => new DeletedFile(Subtopic.Evaluation, f)));

            return deletedFiles;
        }

        private static UpdatedEvaluationFields CreateEvaluationPart(EvaluationModel model, OperationContext context)
        {
            if (model == null)
            {
                return null;
            }

            var logNote = model.Logs != null && model.Logs.Value != null ?
            model.Logs.Value.Text : string.Empty;

            return new UpdatedEvaluationFields(
                ConfigurableFieldModel<string>.GetValueOrDefault(model.ChangeEvaluation),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.EvaluationReady),
                logNote);
        }

        private static UpdatedGeneralFields CreateGeneralPart(GeneralModel model, OperationContext context)
        {
            if (model == null)
            {
                return null;
            }

            return new UpdatedGeneralFields(
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

        private static UpdatedImplementationFields CreateImplementationPart(
            ImplementationModel model,
            OperationContext context)
        {
            if (model == null)
            {
                return null;
            }

            var logNote = model.Logs != null && model.Logs.Value != null ?
                        model.Logs.Value.Text : string.Empty;

            return new UpdatedImplementationFields(
                model.ImplementationStatusId,
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.RealStartDate),
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishingDate),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.BuildImplemented),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.ImplementationPlanUsed),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.ChangeDeviation),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.RecoveryPlanUsed),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.ImplementationReady),
                logNote);
        }

        private static List<NewFile> CreateNewFileCollection(
            List<WebTemporaryFile> newRegistrationFiles,
            List<WebTemporaryFile> newAnalyzeFiles,
            List<WebTemporaryFile> newImplementationFiles,
            List<WebTemporaryFile> newEvaluationFiles,
            OperationContext context)
        {
            var newFiles = new List<NewFile>();

            newFiles.AddRange(
                newRegistrationFiles.Select(
                    f => new NewFile(Subtopic.Registration, f.Content, f.Name, context.DateAndTime)));

            newFiles.AddRange(
                newAnalyzeFiles.Select(f => new NewFile(Subtopic.Analyze, f.Content, f.Name, context.DateAndTime)));

            newFiles.AddRange(
                newImplementationFiles.Select(
                    f => new NewFile(Subtopic.Implementation, f.Content, f.Name, context.DateAndTime)));

            newFiles.AddRange(
                newEvaluationFiles.Select(f => new NewFile(Subtopic.Evaluation, f.Content, f.Name, context.DateAndTime)));

            return newFiles;
        }

        private static List<ManualLog> CreateNewLogCollection(
                                        InputModel inputModel,
                                        IEmailService emailService)
        {
            var newLogs = new List<ManualLog>();

            if (inputModel.Analyze != null)
            {
                CreateNewLogIfNeeded(inputModel.Analyze.Logs, Subtopic.Analyze, newLogs, emailService);

                if (inputModel.Analyze.InviteToCab != null)
                {
                    var logText = inputModel.Analyze.Logs != null && inputModel.Analyze.Logs.Value != null
                                      ? inputModel.Analyze.Logs.Value.Text
                                      : string.Empty;

                    CreateInviteToCabLogIfNeeded(
                                        logText,
                                        inputModel.Analyze.InviteToCab.Emails,
                                        newLogs,
                                        emailService);
                }
            }

            if (inputModel.Implementation != null)
            {
                CreateNewLogIfNeeded(inputModel.Implementation.Logs, Subtopic.Implementation, newLogs, emailService);
            }

            if (inputModel.Evaluation != null)
            {
                CreateNewLogIfNeeded(inputModel.Evaluation.Logs, Subtopic.Evaluation, newLogs, emailService);
            }

            if (inputModel.Log != null)
            {
                CreateNewLogIfNeeded(inputModel.Log.Logs, Subtopic.Log, newLogs, emailService);
            }

            return newLogs;
        }

        private static void CreateInviteToCabLogIfNeeded(
                        string logText,
                        string emailsString,
                        List<ManualLog> logs,
                        IEmailService emailService)
        {
            if (string.IsNullOrEmpty(logText))
            {
                return;
            }

            var emails = string.IsNullOrEmpty(emailsString)
                ? new List<MailAddress>(0)
                : emailsString.Split(Environment.NewLine)
                                .Where(emailService.IsValidEmail)
                                .Select(e => new MailAddress(e))
                                .ToList();

            var newLog = ManualLog.CreateNew(logText, emails, Subtopic.InviteToCab);
            logs.Add(newLog);
        }

        private static void CreateNewLogIfNeeded(
            ConfigurableFieldModel<LogsModel> logField,
            Subtopic subtopic,
            List<ManualLog> logs,
            IEmailService emailService)
        {
            if (logField == null || string.IsNullOrEmpty(logField.Value.Text))
            {
                return;
            }

            var emails = string.IsNullOrEmpty(logField.Value.Emails)
                ? new List<MailAddress>(0)
                : logField.Value.Emails.Split(Environment.NewLine)
                                .Where(emailService.IsValidEmail)
                                .Select(e => new MailAddress(e))
                                .ToList();

            var newLog = ManualLog.CreateNew(logField.Value.Text, emails, subtopic);
            logs.Add(newLog);
        }

        private static UpdatedOrdererFields CreateOrdererPart(OrdererModel model, OperationContext context)
        {
            if (model == null)
            {
                return null;
            }

            return new UpdatedOrdererFields(
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Id),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Name),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Phone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.CellPhone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Email),
                model.DepartmentId);
        }

        private static UpdatedRegistrationFields CreateRegistrationPart(
            RegistrationModel model,
            OperationContext context)
        {
            if (model == null)
            {
                return null;
            }

            DateTime? approvedDateAndTime = null;
            int? approvedByUserId = null;

            if (model.ApprovalValue == StepStatus.Approved)
            {
                approvedDateAndTime = context.DateAndTime;
                approvedByUserId = context.UserId;
            }

            var contacts = model.Contacts != null ? model.Contacts.CloneContacts() : new List<Contact>();

            return new UpdatedRegistrationFields(
                contacts,
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

        private static UpdatedLogFields CreateUpdatedLogFields(Models.Changes.ChangeEdit.LogModel model)
        {
            var logNote = string.Empty;

            if (model != null &&
                model.Logs != null &&
                model.Logs.Value != null)
            {
                logNote = model.Logs.Value.Text;
            }

            return new UpdatedLogFields(logNote);
        }

        private static UpdatedChange CreateUpdatedChange(InputModel model, OperationContext context)
        {
            var id = int.Parse(model.Id);

            var orderer = CreateOrdererPart(model.Orderer, context);
            var general = CreateGeneralPart(model.General, context);
            var registration = CreateRegistrationPart(model.Registration, context);
            var analyze = CreateAnalyzePart(model.Analyze, context);
            var implementation = CreateImplementationPart(model.Implementation, context);
            var evaluation = CreateEvaluationPart(model.Evaluation, context);
            var log = CreateUpdatedLogFields(model.Log);

            return new UpdatedChange(id, orderer, general, registration, analyze, implementation, evaluation, log);
        }

        #endregion
    }
}