namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class LogModel
    {
        public LogModel()
        {
        }

        public LogModel(ConfigurableFieldModel<LogsModel> logs, SendToDialogModel sendToDialog)
        {
            this.Logs = logs;
            this.SendToDialog = sendToDialog;
        }

        [NotNull]
        public ConfigurableFieldModel<LogsModel> Logs { get; set; }

        public string LogText { get; set; }

        public SendToDialogModel SendToDialog { get; set; }

        public string SendToEmails { get; set; }
    }
}