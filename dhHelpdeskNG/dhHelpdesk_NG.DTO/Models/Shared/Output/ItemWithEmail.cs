namespace DH.Helpdesk.BusinessData.Models.Shared.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ItemWithEmail
    {
        public ItemWithEmail()
        {

        }

        public ItemWithEmail(int itemId, string email)
        {
            this.ItemId = itemId;
            this.Email = email;
        }

        [IsId]
        public int ItemId { get; set; }

        [NotNullAndEmpty]
        public string Email { get; set; }
    }
}
