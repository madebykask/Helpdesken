namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class LogOverview
    {
        public LogOverview(int historyId, string text)
        {
            this.Text = text;
            this.HistoryId = historyId;
        }

        [IsId]
        public int HistoryId { get; private set; }

        public string Text { get; private set; }
    }
}
