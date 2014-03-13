namespace DH.Helpdesk.Web.Models.Changes
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Web.LocalizedAttributes;

    public sealed class HistoryModel
    {
        public HistoryModel(
            DateTime dateAndTime,
            UserName registeredBy,
            string log,
            List<FieldDifferencesModel> histories,
            List<string> emails)
        {
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.Log = log;
            this.Histories = histories;
            this.Emails = emails;
        }

        [LocalizedDisplay("Date and Time")]
        public DateTime DateAndTime { get; private set; }

        [LocalizedDisplay("Registered By")]
        public UserName RegisteredBy { get; private set; }

        [LocalizedDisplay("Log")]
        public string Log { get; private set; }

        [LocalizedDisplay("History")]
        public List<FieldDifferencesModel> Histories { get; private set; }

        [LocalizedDisplay("E-mail")]
        public List<string> Emails { get; private set; }
    }
}