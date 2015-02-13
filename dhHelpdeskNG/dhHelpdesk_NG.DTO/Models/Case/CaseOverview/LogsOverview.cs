namespace DH.Helpdesk.BusinessData.Models.Case.CaseOverview
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogsOverview
    {
        public LogsOverview(List<LogOverview> logs)
        {
            this.Logs = logs;
        }

        [NotNull]
        public List<LogOverview> Logs { get; private set; }
    }
}