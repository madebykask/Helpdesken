namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class HistoryRecordModel
    {
        public HistoryRecordModel(
            DateTime dateAndTime,
            UserName registeredBy,
            string log,
            List<FieldDifferencesModel> fieldsDifferences,
            List<string> emails)
        {
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.Log = log;
            this.FieldsDifferences = fieldsDifferences;
            this.Emails = emails;
        }

        [LocalizedDisplay("Date and Time")]
        public DateTime DateAndTime { get; set; }

        [LocalizedDisplay("Registered By")]
        public UserName RegisteredBy { get; set; }

        [LocalizedDisplay("Log")]
        public string Log { get; set; }

        [NotNull]
        [LocalizedDisplay("History")]
        public List<FieldDifferencesModel> FieldsDifferences { get; set; }

        [NotNull]
        [LocalizedDisplay("E-mail")]
        public List<string> Emails { get; set; }
    }
}