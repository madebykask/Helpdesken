namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewLogModelFactory : INewLogModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        public NewLogModelFactory(
            IConfigurableFieldModelFactory configurableFieldModelFactory,
            ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        public LogModel Create(string temporaryId, ChangeEditData editData, LogEditSettings settings)
        {
            throw new NotImplementedException();
//            var logs = this.configurableFieldModelFactory.CreateLogs(
//                settings.Logs,
//                response.Change.Id,
//                Subtopic.Log,
//                response.Logs);
//
//            var sendToDialog = this.sendToDialogModelFactory.Create(
//                editData.EmailGroups,
//                editData.WorkingGroupsWithEmails,
//                editData.Administrators);

//            return new LogModel(logs, sendToDialog);
        }
    }
}