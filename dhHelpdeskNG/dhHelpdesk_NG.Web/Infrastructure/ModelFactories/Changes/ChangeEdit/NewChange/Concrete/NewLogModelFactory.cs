namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewLogModelFactory : INewLogModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        #endregion

        #region Constructors and Destructors

        public NewLogModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public LogModel Create(string temporaryId, LogEditSettings settings, ChangeEditOptions options)
        {
            var logsModel = this.configurableFieldModelFactory.CreateLogs(
                settings.Logs,
                options.EmailGroups,
                options.WorkingGroupsWithEmails,
                options.Administrators);

            return new LogModel(logsModel);
        }

        #endregion
    }
}