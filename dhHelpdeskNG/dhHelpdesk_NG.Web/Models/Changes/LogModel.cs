namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class LogModel
    {
        public LogModel(DateTime dateAndTime, string registeredBy, string text)
        {
            this.DateAndTime = dateAndTime;
            this.RegisteredBy = registeredBy;
            this.Text = text;
        }

        public DateTime DateAndTime { get; private set; }

        [NotNullAndEmpty]
        public string RegisteredBy { get; private set; }

        [NotNullAndEmpty]
        public string Text { get; private set; }
    }
}