namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class Log
    {
        public Log(int id, Subtopic subtopic, DateTime dateAndTime, UserName registeredBy, string text)
        {
            this.Id = id;
            this.Subtopic = subtopic;
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.Text = text;
        }

        [IsId]
        public int Id { get; private set; }

        public Subtopic Subtopic { get; private set; }

        public DateTime DateAndTime { get; private set; }

        [NotNull]
        public UserName RegisteredBy { get; private set; }

        [NotNullAndEmpty]
        public string Text { get; private set; }
    }
}
