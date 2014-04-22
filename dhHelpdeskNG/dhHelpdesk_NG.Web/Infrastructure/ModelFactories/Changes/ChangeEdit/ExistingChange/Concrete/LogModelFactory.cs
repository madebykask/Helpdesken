namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class LogModelFactory : ILogModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        #endregion

        #region Constructors and Destructors

        public LogModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public LogModel Create(FindChangeResponse response)
        {
            var logs = this.configurableFieldModelFactory.CreateLogs(
                response.EditSettings.Log.Logs,
                response.EditData.Change.Id,
                Subtopic.Log,
                response.EditData.Logs,
                response.EditOptions.EmailGroups,
                response.EditOptions.WorkingGroupsWithEmails,
                response.EditOptions.AdministratorsWithEmails);

            return new LogModel(logs);
        }

        #endregion
    }
}