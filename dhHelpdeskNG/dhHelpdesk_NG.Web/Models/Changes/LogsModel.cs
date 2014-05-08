namespace DH.Helpdesk.Web.Models.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class LogsModel
    {
        #region Constructors and Destructors

        public LogsModel()
        {
            this.Logs = new List<LogModel>();
        }

        public LogsModel(SendToDialogModel sendToDialog)
        {
            this.SendToDialog = sendToDialog;
        }

        public LogsModel(int changeId, Subtopic area, List<LogModel> logs, SendToDialogModel sendToDialog)
        {
            this.ChangeId = changeId;
            this.Area = area;
            this.Logs = logs;
            this.SendToDialog = sendToDialog;
        }

        #endregion

        #region Public Properties

        public Subtopic Area { get; set; }

        [IsId]
        public int ChangeId { get; set; }

        public string Emails { get; set; }

        [NotNull]
        public List<LogModel> Logs { get; set; }

        public SendToDialogModel SendToDialog { get; set; }

        public string Text { get; set; }

        #endregion
    }
}