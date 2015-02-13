namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogOverview
    {
        public LogOverview(int historyId, string text)
        {
            this.HistoryId = historyId;
            this.Text = text;
        }

        [IsId]
        public int HistoryId { get; private set; }

        public string Text { get; private set; }
    }
}