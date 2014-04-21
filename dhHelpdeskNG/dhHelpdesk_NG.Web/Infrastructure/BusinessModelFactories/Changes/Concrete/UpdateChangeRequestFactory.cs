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
            OperationContext context)
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

            var newLogs = CreateNewLogCollection(model);

            return new UpdateChangeRequest(
                context,
                updatedChange,
                contacts,
                model.RegistrationModel.AffectedProcessIds,
                model.RegistrationModel.AffectedDepartmentIds,
                model.AnalyzeModel.RelatedChangeIds,
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

        private static List<Contact> CreateContactCollection(InputModel model, OperationContext context)
        {
            var changeId = int.Parse(model.Id);
            var contacts = new List<Contact>();

            CreateContactIfNeeded(
                model.RegistrationModel.Contacts.ContactOne,
                context,
                changeId,
                contacts);

            CreateContactIfNeeded(
                model.RegistrationModel.Contacts.ContactTwo,
                context,
                changeId,
                contacts);
            
            CreateContactIfNeeded(
                model.RegistrationModel.Contacts.ContactThree,
                context,
                changeId,
                contacts);
            
            CreateContactIfNeeded(
                model.RegistrationModel.Contacts.ContactFourth,
                context,
                changeId,
                contacts);
            
            CreateContactIfNeeded(
                model.RegistrationModel.Contacts.ContactFive,
                context,
                changeId,
                contacts);

            CreateContactIfNeeded(
                model.RegistrationModel.Contacts.ContactSix,
                context,
                changeId,
                contacts);

            return contacts;
        }

        private static void CreateContactIfNeeded(
            ContactModel model,
            OperationContext context,
            int changeId,
            List<Contact> contacts)
        {
            if (string.IsNullOrEmpty(model.Name.Value) && string.IsNullOrEmpty(model.Phone.Value)
                && string.IsNullOrEmpty(model.Email.Value) && string.IsNullOrEmpty(model.Company.Value))
            {
                return;
            }

            Contact contact;

            if (model.Id == 0)
            {
                contact = Contact.CreateNew(
                    model.Name.Value,
                    model.Phone.Value,
                    model.Email.Value,
                    model.Company.Value,
                    context.DateAndTime);
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

            deletedFiles.AddRange(deletedRegistrationFiles.Select(f => new DeletedFile(ChangeArea.Registration, f)));
            deletedFiles.AddRange(deletedAnalyzeFiles.Select(f => new DeletedFile(ChangeArea.Analyze, f)));
            deletedFiles.AddRange(deletedImplementationFiles.Select(f => new DeletedFile(ChangeArea.Implementation, f)));
            deletedFiles.AddRange(deletedEvaluationFiles.Select(f => new DeletedFile(ChangeArea.Evaluation, f)));

            return deletedFiles;
        }

        private static UpdatedEvaluationFields CreateEvaluationPart(EvaluationModel model, OperationContext context)
        {
            if (model == null)
            {
                return null;
            }

            return new UpdatedEvaluationFields(
                ConfigurableFieldModel<string>.GetValueOrDefault(model.ChangeEvaluation),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.EvaluationReady));
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
                model.WorkingGroupId,
                model.AdministratorId,
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishingDate),
                context.DateAndTime,
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

            return new UpdatedImplementationFields(
                model.ImplementationStatusId,
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.RealStartDate),
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishingDate),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.BuildImplemented),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.ImplementationPlanUsed),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.ChangeDeviation),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.RecoveryPlanUsed),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.ImplementationReady));
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
                    f => new NewFile(ChangeArea.Registration, f.Content, f.Name, context.DateAndTime)));

            newFiles.AddRange(
                newAnalyzeFiles.Select(f => new NewFile(ChangeArea.Analyze, f.Content, f.Name, context.DateAndTime)));

            newFiles.AddRange(
                newImplementationFiles.Select(
                    f => new NewFile(ChangeArea.Implementation, f.Content, f.Name, context.DateAndTime)));

            newFiles.AddRange(
                newEvaluationFiles.Select(
                    f => new NewFile(ChangeArea.Evaluation, f.Content, f.Name, context.DateAndTime)));

            return newFiles;
        }

        private static List<ManualLog> CreateNewLogCollection(InputModel model)
        {
            var newLogs = new List<ManualLog>();

            CreateNewLogIfNeeded(model.AnalyzeModel.Logs.Value, ChangeArea.Analyze, newLogs);
            CreateNewLogIfNeeded(model.ImplementationModel.Logs.Value, ChangeArea.Implementation, newLogs);
            CreateNewLogIfNeeded(model.Evaluation.Logs.Value, ChangeArea.Evaluation, newLogs);
            CreateNewLogIfNeeded(model.Log.Logs.Value, ChangeArea.Log, newLogs);

            return newLogs;
        }

        private static void CreateNewLogIfNeeded(LogsModel model, ChangeArea area, List<ManualLog> logs)
        {
            if (string.IsNullOrEmpty(model.Text))
            {
                return;
            }

            var emails = model.Emails.Split(Environment.NewLine).Select(e => new MailAddress(e)).ToList();
            var newLog = ManualLog.CreateNew(model.Text, emails, area);
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

            return new UpdatedRegistrationFields(
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

        private static UpdatedChange CreateUpdatedChange(InputModel model, OperationContext context)
        {
            var id = int.Parse(model.Id);

            var orderer = CreateOrdererPart(model.OrdererModel, context);
            var general = CreateGeneralPart(model.GeneralModel, context);
            var registration = CreateRegistrationPart(model.RegistrationModel, context);
            var analyze = CreateAnalyzePart(model.AnalyzeModel, context);
            var implementation = CreateImplementationPart(model.ImplementationModel, context);
            var evaluation = CreateEvaluationPart(model.Evaluation, context);

            return new UpdatedChange(id, orderer, general, registration, analyze, implementation, evaluation);
        }

        #endregion
    }
}