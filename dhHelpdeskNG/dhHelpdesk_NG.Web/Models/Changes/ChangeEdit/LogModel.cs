namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogModel
    {
        #region Constructors and Destructors

        public LogModel()
        {
        }

        public LogModel(ConfigurableFieldModel<LogsModel> logs)
        {
            this.Logs = logs;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<LogsModel> Logs { get; set; }

        #endregion
    }
}