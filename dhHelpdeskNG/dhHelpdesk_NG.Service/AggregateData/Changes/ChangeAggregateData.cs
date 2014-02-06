namespace dhHelpdesk_NG.Service.AggregateData.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Change;

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
