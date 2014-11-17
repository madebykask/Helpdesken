namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public sealed class LogOverview
    {
        public LogOverview(string log)
        {
            this.Log = log;
        }

        public string Log { get; private set; }
    }
}