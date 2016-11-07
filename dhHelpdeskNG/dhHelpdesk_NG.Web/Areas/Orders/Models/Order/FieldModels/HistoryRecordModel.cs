namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels
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
            //List<string> log,
            List<FieldDifferencesModel> fieldsDifferences,
            List<string> emails)
        {
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            //this.Log = log;
            this.FieldsDifferences = fieldsDifferences;
            this.Emails = emails;
        }

        [LocalizedDisplay("Datum")]
        public DateTime DateAndTime { get; set; }

        [LocalizedDisplay("Anmälare")]
        public UserName RegisteredBy { get; set; }

        //[LocalizedDisplay("Logg")]
        //public List<string> Log { get; set; }

        [NotNull]
        [LocalizedDisplay("Historik")]
        public List<FieldDifferencesModel> FieldsDifferences { get; set; }

        [NotNull]
        [LocalizedDisplay("E-mail")]
        public List<string> Emails { get; set; }
    }
}