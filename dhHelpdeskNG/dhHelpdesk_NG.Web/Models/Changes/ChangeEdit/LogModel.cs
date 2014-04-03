namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
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

        public LogModel(SendToDialogModel sendToDialog)
        {
            this.SendToDialog = sendToDialog;
        }

        public ConfigurableFieldModel<LogsModel> Logs { get; set; }

        public string LogText { get; set; }

        public SendToDialogModel SendToDialog { get; set; }

        public string SendToEmails { get; set; }
    }
}