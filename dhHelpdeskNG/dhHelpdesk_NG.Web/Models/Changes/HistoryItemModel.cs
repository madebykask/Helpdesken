namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System;

    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class HistoryItemModel
    {
        public HistoryItemModel(DateTime dateAndTime, string registeredBy, string log, string history, string email)
        {
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.Log = log;
            this.History = history;
            this.Email = email;
        }

        [LocalizedDisplay("Date and Time")]
        public DateTime DateAndTime { get; private set; }

        [LocalizedDisplay("Registered By")]
        public string RegisteredBy { get; private set; }

        [LocalizedDisplay("Log")]
        public string Log { get; private set; }

        [LocalizedDisplay("History")]
        public string History { get; private set; }

        [LocalizedDisplay("E-mail")]
        public string Email { get; private set; }
    }
}