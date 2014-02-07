namespace DH.Helpdesk.Services.AggregateData.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeAggregateData
    {
        public ChangeAggregateData(
            Change change,
            List<Contact> contacts,
            List<History> histories,
            List<LogOverview> logs,
            List<EmailLogOverview> emailLogs)
        {
            this.Change = change;
            this.Contacts = contacts;
            this.Histories = histories;
            this.Logs = logs;
            this.EmailLogs = emailLogs;
        }

        [NotNull]
        public Change Change { get; private set; }

        [NotNull]
        public List<Contact> Contacts { get; private set; }

        [NotNull]
        public List<History> Histories { get; private set; }

        [NotNull]
        public List<LogOverview> Logs { get; private set; }

        [NotNull]
        public List<EmailLogOverview> EmailLogs { get; private set; }
    }
}
