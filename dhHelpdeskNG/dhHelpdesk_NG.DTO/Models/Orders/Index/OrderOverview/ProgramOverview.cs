namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public sealed class ProgramOverview
    {
        public ProgramOverview(string program)
        {
            this.Program = program;
        }

        public string Program { get; private set; }    
    }
}