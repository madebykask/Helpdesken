namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class ChangeModelFactory : IChangeModelFactory
    {
        //        private readonly IAnalyzeModelFactory analyzeModelFactory;
        //
        //        private readonly IRegistrationModelFactory registrationModelFactory;
        //
        //        private readonly IImplementationModelFactory implementationModelFactory;
        //
        //        private readonly IEvaluationModelFactory evaluationModelFactory;
        //
        //        public ChangeModelFactory(
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
        //        public ChangeModel Create(
        //            ChangeAggregate change,
        //            ChangeEditData editData,
        //            ChangeEditSettings editSettings)
        //        {
        //            var header = CreateHeader(change, editData);
        //          
        //            var registration = this.registrationModelFactory.Create(
        //                change,
        //                editSettings.Registration,
        //                editData);
        //            
        //            var analyze = this.analyzeModelFactory.Create(change, editSettings.Analyze, editData);
        //            
        //            var implementation = this.implementationModelFactory.Create(
        //                change,
        //                editSettings.Implementation,
        //                editData);
        //
        //            var evaluation = this.evaluationModelFactory.CreateEvaluation(
        //                change,
        //                editSettings.Evaluation,
        //                editData);
        //            
        //            var history = CreateHistory(change);
        //
        //            var inputModel = new InputModel(header, registration, analyze, implementation, evaluation, history);
        //            return new ChangeModel(change.Id, inputModel);
        //        }
        //
        //        private static ChangeHeaderModel CreateHeader(ChangeAggregate change, ChangeEditData editData)
        //        {
        //            var departmentList = new SelectList(editData.Departments, "Value", "Name", change.Header.DepartmentId);
        //            var statusList = new SelectList(editData.Statuses, "Value", "Name", change.Header.StatusId);
        //            var systemList = new SelectList(editData.Systems, "Value", "Name", change.Header.SystemId);
        //            var objectList = new SelectList(editData.Objects, "Value", "Name", change.Header.ObjectId);
        //            var workingGroupList = new SelectList(
        //                editData.WorkingGroupsWithEmails,
        //                "Id",
        //                "Name",
        //                change.Header.WorkingGroupId);
        //            var administratorList = new SelectList(
        //                editData.Administrators,
        //                "Value",
        //                "Name",
        //                change.Header.AdministratorId);
        //
        //            return new ChangeHeaderModel(
        //                change.Header.Id,
        //                change.Header.Name,
        //                change.Header.Phone,
        //                change.Header.CellPhone,
        //                change.Header.Email,
        //                departmentList,
        //                change.Header.Title,
        //                statusList,
        //                systemList,
        //                objectList,
        //                workingGroupList,
        //                administratorList,
        //                change.Header.FinishingDate,
        //                change.Header.CreatedDate,
        //                change.Header.ChangedDate,
        //                change.Header.Rss);
        //        }
        //
        //        private static HistoriesModel CreateHistory(ChangeAggregate change)
        //        {
        //            var historyItems = new List<HistoryModel>(change.Histories.Count);
        //
        //            foreach (var history in change.Histories)
        //            {
        //                var diff =
        //                    history.History.Select(h => new FieldDifferencesModel(h.FieldName, h.OldValue, h.NewValue)).ToList();
        //
        //                var historyItem = new HistoryModel(
        //                    history.DateAndTime,
        //                    history.RegisteredBy,
        //                    history.Log,
        //                    diff,
        //                    history.Emails);
        //
        //                historyItems.Add(historyItem);
        //            }
        //
        //            return new HistoriesModel(historyItems);
        //        }

        #region Public Methods and Operators

        public InputModel Create(FindChangeResponse change, ChangeEditData editData, ChangeEditSettings editSettings)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}