namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class Log
    {
        public Log(int id, DateTime dateAndTime, string registeredBy, string text)
        {
            this.Id = id;
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.Text = text;
        }

        [IsId]
        public int Id { get; private set; }

        public DateTime DateAndTime { get; private set; }

        [NotNullAndEmpty]
        public string RegisteredBy { get; private set; }

        [NotNullAndEmpty]
        public string Text { get; private set; }
    }
}
