namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class LogsModel
    {
        public LogsModel(string changeId, Subtopic subtopic) : this(changeId, subtopic, new List<LogModel>(0))
        {
        }

        public LogsModel(string changeId, Subtopic subtopic, List<LogModel> logs)
        {
            this.ChangeId = changeId;
            this.Subtopic = subtopic;
            this.Logs = logs;
        }

        public string ChangeId { get; set; }

        public Subtopic Subtopic { get; set; }

        [NotNull]
        public List<LogModel> Logs { get; set; }
    }
}