namespace dhHelpdesk_NG.DTO.DTOs
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ItemWithEmail
    {
        public ItemWithEmail(int itemId, string email)
        {
            this.ItemId = itemId;
            this.Email = email;
        }

        [IsId]
        public int ItemId { get; private set; }

        [NotNullAndEmpty]
        public string Email { get; private set; }
    }
}
