namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class HistoryItemModel
    {
        public HistoryItemModel(DateTime dateAndTime, string registeredBy, string log, List<FieldDifferenceModel> history, List<string> emails)
        {
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.Log = log;
            this.History = history;
            this.Emails = emails;
        }

        [LocalizedDisplay("Date and Time")]
        public DateTime DateAndTime { get; private set; }

        [LocalizedDisplay("Registered By")]
        public string RegisteredBy { get; private set; }

        [LocalizedDisplay("Log")]
        public string Log { get; private set; }

        [LocalizedDisplay("History")]
        public List<FieldDifferenceModel> History { get; private set; }

        [LocalizedDisplay("E-mail")]
        public List<string> Emails { get; private set; }
    }
}