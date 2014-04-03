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
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    using LogModel = DH.Helpdesk.Web.Models.Changes.ChangeEdit.LogModel;

    public sealed class UpdateChangeRequestFactory : IUpdateChangeRequestFactory
    {
        #region Public Methods and Operators

        private void CreateContactIfNeeded(int changeId, DateTime date, ContactModel model, List<Contact> contacts)
        {
            if (!string.IsNullOrEmpty(model.Name.Value)
               || !string.IsNullOrEmpty(model.Phone.Value)
               || !string.IsNullOrEmpty(model.Email.Value)
               || !string.IsNullOrEmpty(model.Company.Value))
            {
                Contact contact;

                if (model.Id == 0)
                {
                    contact = Contact.CreateNew(
                        model.Name.Value,
                        model.Phone.Value,
                        model.Email.Value,
                        model.Company.Value,
                        date);
                }
                else
                {
                    contact = Contact.CreateUpdated(
                        model.Id,
                        changeId,
                        model.Name.Value,
                        model.Phone.Value,
                        model.Email.Value,
                        model.Company.Value,
                        date);
                }

                contacts.Add(contact);
            }
        }

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
            int currentUserId,
            int currentCustomerId,
            int currentLanguageId,
            DateTime changedDateAndTime)
        {
            var updatedChange = CreateUpdatedChange(model, currentUserId, changedDateAndTime);

            var contacts = new List<Contact>();
            this.CreateContactIfNeeded(int.Parse(model.ChangeId), DateTime.Now, model.Registration.Contacts.ContactOne, contacts);
            this.CreateContactIfNeeded(int.Parse(model.ChangeId), DateTime.Now, model.Registration.Contacts.ContactTwo, contacts);
            this.CreateContactIfNeeded(int.Parse(model.ChangeId), DateTime.Now, model.Registration.Contacts.ContactThree, contacts);
            this.CreateContactIfNeeded(int.Parse(model.ChangeId), DateTime.Now, model.Registration.Contacts.ContactFourth, contacts);
            this.CreateContactIfNeeded(int.Parse(model.ChangeId), DateTime.Now, model.Registration.Contacts.ContactFive, contacts);
            this.CreateContactIfNeeded(int.Parse(model.ChangeId), DateTime.Now, model.Registration.Contacts.ContactSix, contacts);

            var deletedFiles = CreateDeletedFiles(
                deletedRegistrationFiles,
                deletedAnalyzeFiles,
                deletedImplementationFiles,
                deletedEvaluationFiles);

            var newFiles = CreateNewFiles(
                newRegistrationFiles,
                newAnalyzeFiles,
                newImplementationFiles,
                newEvaluationFiles,
                changedDateAndTime);

            var newLogs = new List<ManualLog>();
            var analLog = CreateAnalyzeNewLog(model.Analyze);
            if (analLog != null)
            {
                newLogs.Add(analLog);
            }
            var logLog = CreateLogNewLog(model.Log);
            if (logLog != null)
            {
                newLogs.Add(logLog);
            }

            var operationContext = new OperationContext
                                   {
                                       CustomerId = currentCustomerId,
                                       DateAndTime = changedDateAndTime,
                                       LanguageId = currentLanguageId,
                                       UserId = currentUserId
                                   };

            return new UpdateChangeRequest(
                operationContext,
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

        private static ManualLog CreateAnalyzeNewLog(AnalyzeModel model)
        {
            if (string.IsNullOrEmpty(model.LogText))
            {
                return null;
            }

            var emails = !string.IsNullOrEmpty(model.SendToEmails)
                ? model.SendToEmails.Split(Environment.NewLine).Select(e => new MailAddress(e)).ToList()
                : new List<MailAddress>(0);

            return ManualLog.CreateNew(model.LogText, emails, Subtopic.Analyze);
        }

        private static UpdatedAnalyzeFields CreateAnalyzePart(
            AnalyzeModel model,
            int currentUserId,
            DateTime changedDateAndTime)
        {
            if (model == null)
            {
                return null;
            }

            DateTime? approvedDateAndTime = null;
            int? approvedByUserId = null;

            if (model.ApprovalValue == StepStatus.Approved)
            {
                approvedDateAndTime = changedDateAndTime;
                approvedByUserId = currentUserId;
            }

            return new UpdatedAnalyzeFields(
                model.CategoryId,
                model.PriorityId,
                model.ResponsibleId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Solution),
                ConfigurableFieldModel<int>.GetValueOrDefault(model.Cost),
                ConfigurableFieldModel<int>.GetValueOrDefault(model.YearlyCost),
                model.CurrencyId,
                ConfigurableFieldModel<int>.GetValueOrDefault(model.EstimatedTimeInHours),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Risk),
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.StartDate),
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishDate),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.HasImplementationPlan),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.HasRecoveryPlan),
                model.ApprovalValue,
                approvedDateAndTime,
                approvedByUserId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.RejectExplanation));
        }

        private static List<DeletedFile> CreateDeletedFiles(
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

        private static UpdatedEvaluationFields CreateEvaluationPart(EvaluationModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new UpdatedEvaluationFields(
                ConfigurableFieldModel<string>.GetValueOrDefault(model.ChangeEvaluation),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.EvaluationReady));
        }

        private static UpdatedGeneralFields CreateGeneralPart(GeneralModel model, DateTime changedDateAndTime)
        {
            if (model == null)
            {
                return null;
            }

            return new UpdatedGeneralFields(
                ConfigurableFieldModel<int>.GetValueOrDefault(model.Priority),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Title),
                model.StatusId,
                model.SystemId,
                model.ObjectId,
                model.WorkingGroupId,
                model.AdministratorId,
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishingDate),
                changedDateAndTime,
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.Rss));
        }

        private static UpdatedImplementationFields CreateImplementationPart(ImplementationModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new UpdatedImplementationFields(
                model.StatusId,
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.RealStartDate),
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishingDate),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.BuildImplemented),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.ImplementationPlanUsed),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Deviation),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.RecoveryPlanUsed),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.ImplementationReady));
        }

        private static ManualLog CreateLogNewLog(LogModel model)
        {
            if (string.IsNullOrEmpty(model.LogText))
            {
                return null;
            }

            List<MailAddress> emails;

            if (string.IsNullOrEmpty(model.SendToEmails))
            {
                emails = new List<MailAddress>(0);
            }
            else
            {
                emails = model.SendToEmails.Split(Environment.NewLine).Select(e => new MailAddress(e)).ToList();
            }

            return ManualLog.CreateNew(model.LogText, emails, Subtopic.Log);
        }

        private static List<NewFile> CreateNewFiles(
            List<WebTemporaryFile> newRegistrationFiles,
            List<WebTemporaryFile> newAnalyzeFiles,
            List<WebTemporaryFile> newImplementationFiles,
            List<WebTemporaryFile> newEvaluationFiles,
            DateTime changedDateAndTime)
        {
            var newFiles = new List<NewFile>();

            newFiles.AddRange(
                newRegistrationFiles.Select(
                    f => new NewFile(Subtopic.Registration, f.Content, f.Name, changedDateAndTime)));

            newFiles.AddRange(
                newAnalyzeFiles.Select(f => new NewFile(Subtopic.Analyze, f.Content, f.Name, changedDateAndTime)));

            newFiles.AddRange(
                newImplementationFiles.Select(
                    f => new NewFile(Subtopic.Implementation, f.Content, f.Name, changedDateAndTime)));

            newFiles.AddRange(
                newEvaluationFiles.Select(f => new NewFile(Subtopic.Evaluation, f.Content, f.Name, changedDateAndTime)));

            return newFiles;
        }

        private static UpdatedOrdererFields CreateOrdererPart(OrdererModel model)
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
            int currentUserId,
            DateTime changedDateAndTime)
        {
            if (model == null)
            {
                return null;
            }

            DateTime? approvedDateAndTime = null;
            int? approvedByUserId = null;

            if (model.ApprovalValue == StepStatus.Approved)
            {
                approvedDateAndTime = changedDateAndTime;
                approvedByUserId = currentUserId;
            }

            return new UpdatedRegistrationFields(
                model.OwnerId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Description),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.BusinessBenefits),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Consequence),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Impact),
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.DesiredDateAndTime),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.Verified),
                model.ApprovalValue,
                approvedDateAndTime,
                approvedByUserId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.RejectExplanation));
        }

        private static UpdatedChange CreateUpdatedChange(
            InputModel model,
            int currentUserId,
            DateTime changedDateAndTime)
        {
            var id = int.Parse(model.ChangeId);

            var orderer = CreateOrdererPart(model.Orderer);
            var general = CreateGeneralPart(model.General, changedDateAndTime);
            var registration = CreateRegistrationPart(model.Registration, currentUserId, changedDateAndTime);
            var analyze = CreateAnalyzePart(model.Analyze, currentUserId, changedDateAndTime);
            var implementation = CreateImplementationPart(model.Implementation);
            var evaluation = CreateEvaluationPart(model.Evaluation);

            return new UpdatedChange(id, orderer, general, registration, analyze, implementation, evaluation);
        }

        #endregion
    }
}