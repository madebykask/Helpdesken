namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public sealed class ProgramOverview
    {
        public ProgramOverview(string[] programs)
        {
            this.Programs = programs;
        }

        public string[] Programs { get; private set; }    
    }
}