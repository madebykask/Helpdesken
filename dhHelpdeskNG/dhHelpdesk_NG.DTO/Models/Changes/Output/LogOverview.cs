namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
