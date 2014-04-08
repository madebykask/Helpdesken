namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class HistoriesDifference
    {
        public HistoriesDifference(
            DateTime dateAndTime,
            UserName registeredBy,
            string log,
            List<FieldDifference> history,
            List<string> emails)
        {
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.Log = log;
            this.History = history;
            this.Emails = emails;
        }

        public DateTime DateAndTime { get; private set; }

        [NotNull]
        public UserName RegisteredBy { get; private set; }

        public string Log { get; private set; }

        [NotNull]
        public List<FieldDifference> History { get; private set; }

        [NotNull]
        public List<string> Emails { get; private set; }
    }
}