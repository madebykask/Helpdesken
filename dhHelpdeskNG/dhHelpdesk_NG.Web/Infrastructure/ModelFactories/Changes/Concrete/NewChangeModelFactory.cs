namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Common;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

    public sealed class NewChangeModelFactory : INewChangeModelFactory
    {
        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        private readonly IAnalyzeModelFactory analyzeModelFactory;

        private readonly IRegistrationModelFactory registrationModelFactory;

        public NewChangeModelFactory(
            ISendToDialogModelFactory sendToDialogModelFactory, 
            IAnalyzeModelFactory analyzeModelFactory, 
            IRegistrationModelFactory registrationModelFactory)
        {
            this.sendToDialogModelFactory = sendToDialogModelFactory;
            this.analyzeModelFactory = analyzeModelFactory;
            this.registrationModelFactory = registrationModelFactory;
        }

        public NewChangeModel Create(string temporatyId, ChangeOptionalData optionalData, ChangeEditSettings editSettings)
        {
            var header = CreateHeader(optionalData);
            
            var registration = this.registrationModelFactory.Create(
                temporatyId,
                editSettings.RegistrationFields,
                optionalData);

            var analyze = this.analyzeModelFactory.Create(temporatyId, editSettings.AnalyzeFields, optionalData);
            var implementation = CreateImplementation(temporatyId, optionalData);
            var evaluation = CreateEvaluation(temporatyId, optionalData);

            var inputModel = new InputModel(
                header,
                registration,
                analyze, 
                implementation,
                evaluation,
                new HistoryModel(new List<HistoryItemModel>(0)));

            return new NewChangeModel(temporatyId, inputModel);
        }

        private static ChangeHeaderModel CreateHeader(ChangeOptionalData optionalData)
        {
            var departmentList = new SelectList(optionalData.Departments, "Value", "Name");
            var statusList = new SelectList(optionalData.Statuses, "Value", "Name");
            var systemList = new SelectList(optionalData.Systems, "Value", "Name");
            var objectList = new SelectList(optionalData.Objects, "Value", "Name");
            var workingGroupList = new SelectList(optionalData.WorkingGroups, "Id", "Name");
            var administratorList = new SelectList(optionalData.Administrators, "Value", "Name");

            return new ChangeHeaderModel(
                null,
                null,
                null,
                null,
                null,
                departmentList,
                null,
                statusList,
                systemList,
                objectList,
                workingGroupList,
                administratorList,
                null,
                null,
                null,
                false);
        }

        private ImplementationModel CreateImplementation(string temporaryId, ChangeOptionalData optionalData)
        {
            var implementationStatusList = new SelectList(optionalData.ImplementationStatuses, "Value", "Name");
            var attachedFilesContainer = new AttachedFilesContainerModel(temporaryId, Subtopic.Implementation);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                optionalData.EmailGroups,
                optionalData.WorkingGroups,
                optionalData.Administrators);

            return new ImplementationModel(
                temporaryId,
                implementationStatusList,
                null,
                null,
                false,
                false,
                null,
                false,
                attachedFilesContainer,
                sendToDialog,
                false);
        }

        private EvaluationModel CreateEvaluation(string temporaryId, ChangeOptionalData optionalData)
        {
            var attachedFilesContainer = new AttachedFilesContainerModel(temporaryId, Subtopic.Evaluation);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                optionalData.EmailGroups,
                optionalData.WorkingGroups,
                optionalData.Administrators);

            return new EvaluationModel(temporaryId, null, attachedFilesContainer, sendToDialog, false);
        }
    }
}