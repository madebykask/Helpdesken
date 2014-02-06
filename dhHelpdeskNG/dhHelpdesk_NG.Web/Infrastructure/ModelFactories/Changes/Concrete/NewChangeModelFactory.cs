namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

    public sealed class NewChangeModelFactory : INewChangeModelFactory
    {
        private readonly IAnalyzeModelFactory analyzeModelFactory;

        private readonly IRegistrationModelFactory registrationModelFactory;

        private readonly IImplementationModelFactory implementationModelFactory;

        private readonly IEvaluationModelFactory evaluationModelFactory;

        public NewChangeModelFactory(
            IAnalyzeModelFactory analyzeModelFactory, 
            IRegistrationModelFactory registrationModelFactory, 
            IImplementationModelFactory implementationModelFactory,
            IEvaluationModelFactory evaluationModelFactory)
        {
            this.analyzeModelFactory = analyzeModelFactory;
            this.registrationModelFactory = registrationModelFactory;
            this.implementationModelFactory = implementationModelFactory;
            this.evaluationModelFactory = evaluationModelFactory;
        }

        public NewChangeModel Create(string temporatyId, ChangeEditOptions optionalData, ChangeEditSettings editSettings)
        {
            var header = CreateHeader(optionalData);
            
            var registration = this.registrationModelFactory.Create(
                temporatyId,
                editSettings.RegistrationFields,
                optionalData);

            var analyze = this.analyzeModelFactory.Create(temporatyId, editSettings.AnalyzeFields, optionalData);
            
            var implementation = this.implementationModelFactory.Create(
                temporatyId,
                editSettings.ImplementationFields,
                optionalData);

            var evaluation = this.evaluationModelFactory.CreateEvaluation(
                temporatyId,
                editSettings.EvaluationFields,
                optionalData);

            var inputModel = new InputModel(
                header,
                registration,
                analyze, 
                implementation,
                evaluation,
                new HistoryModel(new List<HistoryItemModel>(0)));

            return new NewChangeModel(temporatyId, inputModel);
        }

        private static ChangeHeaderModel CreateHeader(ChangeEditOptions optionalData)
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
    }
}