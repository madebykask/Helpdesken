namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public sealed class LogOverview
    {
        public LogOverview(string[] logs)
        {
            this.Logs = logs;
        }

        public string[] Logs { get; private set; }
    }
}