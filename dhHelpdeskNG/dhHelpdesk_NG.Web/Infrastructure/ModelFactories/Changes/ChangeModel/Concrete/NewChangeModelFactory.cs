namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewChangeModelFactory : INewChangeModelFactory
    {
        //        private readonly IAnalyzeModelFactory analyzeModelFactory;
        //
        //        private readonly IRegistrationModelFactory registrationModelFactory;
        //
        //        private readonly IImplementationModelFactory implementationModelFactory;
        //
        //        private readonly IEvaluationModelFactory evaluationModelFactory;
        //
        //        public NewChangeModelFactory(
        //            IAnalyzeModelFactory analyzeModelFactory, 
        //            IRegistrationModelFactory registrationModelFactory, 
        //            IImplementationModelFactory implementationModelFactory,
        //            IEvaluationModelFactory evaluationModelFactory)
        //        {
        //            this.analyzeModelFactory = analyzeModelFactory;
        //            this.registrationModelFactory = registrationModelFactory;
        //            this.implementationModelFactory = implementationModelFactory;
        //            this.evaluationModelFactory = evaluationModelFactory;
        //        }
        //
        //        public InputModel Create(string temporatyId, ChangeEditData editData, ChangeEditSettings editSettings)
        //        {
        //            var header = CreateHeader(editData);
        //            
        //            var registration = this.registrationModelFactory.Create(
        //                temporatyId,
        //                editSettings.Registration,
        //                editData);
        //
        //            var analyze = this.analyzeModelFactory.Create(temporatyId, editData, editSettings.Analyze);
        //            
        //            var implementation = this.implementationModelFactory.Create(
        //                temporatyId,
        //                editSettings.Implementation,
        //                editData);
        //
        //            var evaluation = this.evaluationModelFactory.CreateEvaluation(
        //                temporatyId,
        //                editSettings.Evaluation,
        //                editData);
        //
        //            var inputModel = new InputModel(
        //                temporatyId,
        //                true,
        //                
        //                header,
        //                registration,
        //                analyze, 
        //                implementation,
        //                evaluation,
        //                new HistoriesModel(new List<HistoryModel>(0)));
        //
        //            return new NewChangeModel(temporatyId, inputModel);
        //        }
        //
        //        private static ChangeHeaderModel CreateHeader(ChangeEditData editData)
        //        {
        //            var departmentList = new SelectList(editData.Departments, "Value", "Name");
        //            var statusList = new SelectList(editData.Statuses, "Value", "Name");
        //            var systemList = new SelectList(editData.Systems, "Value", "Name");
        //            var objectList = new SelectList(editData.Objects, "Value", "Name");
        //            var workingGroupList = new SelectList(editData.WorkingGroupsWithEmails, "Id", "Name");
        //            var administratorList = new SelectList(editData.Administrators, "Value", "Name");
        //
        //            return new ChangeHeaderModel(
        //                null,
        //                null,
        //                null,
        //                null,
        //                null,
        //                departmentList,
        //                null,
        //                statusList,
        //                systemList,
        //                objectList,
        //                workingGroupList,
        //                administratorList,
        //                null,
        //                null,
        //                null,
        //                false);
        //        }

        #region Public Methods and Operators

        public InputModel Create(string temporatyId, ChangeEditData editData, ChangeEditSettings editSettings)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}