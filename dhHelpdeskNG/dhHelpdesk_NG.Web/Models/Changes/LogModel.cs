namespace DH.Helpdesk.Web.Models.Changes
{
    using System;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class LogModel
    {
        public LogModel(int id, DateTime dateAndTime, UserName registeredBy, string text)
        {
            this.Id = id;
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.Text = text;
        }

        [IsId]
        public int Id { get; private set; }

        [LocalizedDisplay("Date and Time")]
        public DateTime DateAndTime { get; private set; }

        [NotNull]
        [LocalizedDisplay("Registered By")]
        public UserName RegisteredBy { get; private set; }

        [NotNullAndEmpty]
        [LocalizedDisplay("Text")]
        public string Text { get; private set; }
    }
}