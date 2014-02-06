namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Common;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

    public sealed class ChangeModelFactory : IChangeModelFactory
    {
        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        private readonly IAnalyzeModelFactory analyzeModelFactory;

        private readonly IRegistrationModelFactory registrationModelFactory;

        public ChangeModelFactory(
            ISendToDialogModelFactory sendToDialogModelFactory,
            IAnalyzeModelFactory analyzeModelFactory,
            IRegistrationModelFactory registrationModelFactory)
        {
            this.sendToDialogModelFactory = sendToDialogModelFactory;
            this.analyzeModelFactory = analyzeModelFactory;
            this.registrationModelFactory = registrationModelFactory;
        }

        public ChangeModel Create(
            ChangeAggregate change,
            ChangeOptionalData optionalData,
            ChangeEditSettings editSettings)
        {
            var header = CreateHeader(change, optionalData);
          
            var registration = this.registrationModelFactory.Create(
                change,
                editSettings.RegistrationFields,
                optionalData);
            
            var analyze = this.analyzeModelFactory.Create(change, editSettings.AnalyzeFields, optionalData);
            var implementation = this.CreateImplementation(change, optionalData);
            var evaluation = this.CreateEvaluation(change, optionalData);
            var history = CreateHistory(change);

            var inputModel = new InputModel(header, registration, analyze, implementation, evaluation, history);
            return new ChangeModel(change.Id, inputModel);
        }

        private static ChangeHeaderModel CreateHeader(ChangeAggregate change, ChangeOptionalData optionalData)
        {
            var departmentList = new SelectList(optionalData.Departments, "Value", "Name", change.Header.DepartmentId);
            var statusList = new SelectList(optionalData.Statuses, "Value", "Name", change.Header.StatusId);
            var systemList = new SelectList(optionalData.Systems, "Value", "Name", change.Header.SystemId);
            var objectList = new SelectList(optionalData.Objects, "Value", "Name", change.Header.ObjectId);
            var workingGroupList = new SelectList(
                optionalData.WorkingGroups,
                "Id",
                "Name",
                change.Header.WorkingGroupId);
            var administratorList = new SelectList(
                optionalData.Administrators,
                "Value",
                "Name",
                change.Header.AdministratorId);

            return new ChangeHeaderModel(
                change.Header.Id,
                change.Header.Name,
                change.Header.Phone,
                change.Header.CellPhone,
                change.Header.Email,
                departmentList,
                change.Header.Title,
                statusList,
                systemList,
                objectList,
                workingGroupList,
                administratorList,
                change.Header.FinishingDate,
                change.Header.CreatedDate,
                change.Header.ChangedDate,
                change.Header.Rss);
        }

        private ImplementationModel CreateImplementation(
            ChangeAggregate change,
            ChangeOptionalData optionalData)
        {
            var implementationStatusList = new SelectList(optionalData.ImplementationStatuses, "Value", "Name");

            var attachedFilesContainer =
                new AttachedFilesContainerModel(
                    change.Id.ToString(CultureInfo.InvariantCulture),
                    Subtopic.Implementation);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                optionalData.EmailGroups,
                optionalData.WorkingGroups,
                optionalData.Administrators);

            return new ImplementationModel(
                change.Id.ToString(CultureInfo.InvariantCulture),
                implementationStatusList,
                change.Implementation.RealStartDate,
                change.Implementation.FinishingDate,
                change.Implementation.BuildImplemented,
                change.Implementation.ImplementationPlanUsed,
                change.Implementation.ChangeDeviation,
                change.Implementation.RecoveryPlanUsed,
                attachedFilesContainer,
                sendToDialog,
                change.Implementation.ImplementationReady);
        }

        private EvaluationModel CreateEvaluation(ChangeAggregate change, ChangeOptionalData optionalData)
        {
            var attachedFilesContainer =
                new AttachedFilesContainerModel(change.Id.ToString(CultureInfo.InvariantCulture), Subtopic.Evaluation);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                optionalData.EmailGroups,
                optionalData.WorkingGroups,
                optionalData.Administrators);

            return new EvaluationModel(
                change.Id.ToString(CultureInfo.InvariantCulture),
                change.Evaluation.ChangeEvaluation,
                attachedFilesContainer,
                sendToDialog,
                change.Evaluation.EvaluationReady);
        }

        private static HistoryModel CreateHistory(ChangeAggregate change)
        {
            var historyItems = new List<HistoryItemModel>(change.Histories.Count);

            foreach (var history in change.Histories)
            {
                var diff =
                    history.History.Select(h => new FieldDifferenceModel(h.FieldName, h.OldValue, h.NewValue)).ToList();

                var historyItem = new HistoryItemModel(
                    history.DateAndTime,
                    history.RegisteredBy,
                    history.Log,
                    diff,
                    history.Emails);

                historyItems.Add(historyItem);
            }

            return new HistoryModel(historyItems);
        }
    }
}