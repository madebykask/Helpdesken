namespace DH.Helpdesk.Web.Models.Changes
{
    using System;

    using DH.Helpdesk.Common.Types;
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
        public int Id { get; set; }

        [LocalizedDisplay("Date and Time")]
        public DateTime DateAndTime { get; set; }

        [NotNull]
        [LocalizedDisplay("Registered By")]
        public UserName RegisteredBy { get; set; }

        [NotNullAndEmpty]
        [LocalizedDisplay("Text")]
        public string Text { get; set; }
    }
}