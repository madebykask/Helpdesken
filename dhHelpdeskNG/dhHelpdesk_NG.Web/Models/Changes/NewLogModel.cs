namespace DH.Helpdesk.Web.Models.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewLogModel
    {
        public NewLogModel()
        {
            this.RecipientEmails = new List<string>();
        }

        public string Text { get; set; }

        [NotNull]
        public List<string> RecipientEmails { get; set; }
    }
}