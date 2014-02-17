namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Requests.Changes;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class UpdateChangeRequestFactory : IUpdateChangeRequestFactory
    {
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
            DateTime changedDateAndTime)
        {
            var updatedChange = CreateUpdatedChange(model, currentUserId, changedDateAndTime);

            var deletedFiles = CreateDeletedFiles(
                deletedRegistrationFiles, deletedAnalyzeFiles, deletedImplementationFiles, deletedEvaluationFiles);

            var newFiles = CreateNewFiles(
                newRegistrationFiles, newAnalyzeFiles, newImplementationFiles, newEvaluationFiles, changedDateAndTime);

            return new UpdateChangeRequest(
                updatedChange,
                model.Registration.AffectedProcessIds,
                model.Registration.AffectedDepartmentIds,
                model.Analyze.RelatedChangeIds,
                deletedFiles,
                newFiles,
                deletedLogIds);
        }

        private static UpdatedChange CreateUpdatedChange(
            InputModel model, int currentUserId, DateTime changedDateAndTime)
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
            return new UpdatedOrdererFields(
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Id),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Name),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Phone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.CellPhone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Email),
                model.DepartmentId);
        }

        private static UpdatedGeneralFields CreateGeneralPart(GeneralModel model, DateTime changedDateAndTime)
        {
            return new UpdatedGeneralFields(
                ConfigurableFieldModel<int>.GetValueOrDefault(model.Priority),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Title),
                model.StatusId,
                model.StatusId,
                model.SystemId,
                model.WorkingGroupId,
                model.AdministratorId,
                ConfigurableFieldModel<DateTime?>.GetValueOrDefault(model.FinishingDate),
                changedDateAndTime,
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.Rss));
        }

        private static UpdatedRegistrationFields CreateRegistrationPart(
            RegistrationModel model, int currentUserId, DateTime changedDateAndTime)
        {
            DateTime? approvedDateAndTime = null;
            int? approvedByUserId = null;

            if (model.ApprovalValue == RegistrationApprovalResult.Approved)
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

        private static UpdatedAnalyzeFields CreateAnalyzePart(
            AnalyzeModel model, int currentUserId, DateTime changedDateAndTime)
        {
            DateTime? approvedDateAndTime = null;
            int? approvedByUserId = null;

            if (model.ApprovalValue == AnalyzeApprovalResult.Approved)
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

        private static UpdatedImplementationFields CreateImplementationPart(ImplementationModel model)
        {
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

        private static UpdatedEvaluationFields CreateEvaluationPart(EvaluationModel model)
        {
            return new UpdatedEvaluationFields(
                ConfigurableFieldModel<string>.GetValueOrDefault(model.ChangeEvaluation),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.EvaluationReady));
        }
    }
}