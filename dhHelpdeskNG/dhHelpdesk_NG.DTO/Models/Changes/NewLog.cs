namespace DH.Helpdesk.BusinessData.Models.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewLog
    {
        public NewLog(string text, List<string> emails)
        {
            this.Text = text;
            this.Emails = emails;
        }

        [NotNullAndEmpty]
        public string Text { get; private set; }

        [NotNull]
        public List<string> Emails { get; private set; }
    }
}