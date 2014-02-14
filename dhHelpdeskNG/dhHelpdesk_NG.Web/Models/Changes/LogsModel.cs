namespace DH.Helpdesk.Web.Models.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogsModel
    {
        public LogsModel(int changeId, Subtopic subtopic, List<LogModel> logs)
        {
            this.ChangeId = changeId;
            this.Subtopic = subtopic;
            this.Logs = logs;
        }

        [IsId]
        public int ChangeId { get; private set; }

        public Subtopic Subtopic { get; private set; }

        [NotNull]
        public List<LogModel> Logs { get; private set; }
    }
}