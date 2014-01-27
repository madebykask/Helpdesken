namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class EmailLogOverview
    {
        public EmailLogOverview(int historyId, string email)
        {
            this.HistoryId = historyId;
            this.Email = email;
        }

        [IsId]
        public int HistoryId { get; private set; }

        [NotNull]
        public string Email { get; private set; }
    }
}
