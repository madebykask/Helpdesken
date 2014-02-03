namespace dhHelpdesk_NG.DTO.DTOs
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

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
