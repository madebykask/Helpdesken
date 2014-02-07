namespace DH.Helpdesk.BusinessData.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ItemWithEmails
    {
        public ItemWithEmails(int itemId, List<string> emails)
        {
            this.ItemId = itemId;
            this.Emails = emails;
        }

        [IsId]
        public int ItemId { get; private set; }

        [NotNull]
        public List<string> Emails { get; private set; }
    }
}
