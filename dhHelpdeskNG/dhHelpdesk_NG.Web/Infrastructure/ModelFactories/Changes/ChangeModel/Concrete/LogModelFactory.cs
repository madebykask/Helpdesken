namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class LogModelFactory : ILogModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        #endregion

        #region Constructors and Destructors

        public LogModelFactory(
            IConfigurableFieldModelFactory configurableFieldModelFactory,
            ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public LogModel Create(FindChangeResponse response, ChangeEditData editData, LogEditSettings settings)
        {
            var logs = this.configurableFieldModelFactory.CreateLogs(
                settings.Logs,
                response.Change.Id,
                Subtopic.Log,
                response.Logs);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                editData.EmailGroups,
                editData.WorkingGroupsWithEmails,
                editData.Administrators);

            return new LogModel(logs, sendToDialog);
        }

        #endregion
    }
}