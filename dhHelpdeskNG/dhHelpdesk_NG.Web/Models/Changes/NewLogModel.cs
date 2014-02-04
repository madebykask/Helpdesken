namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

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