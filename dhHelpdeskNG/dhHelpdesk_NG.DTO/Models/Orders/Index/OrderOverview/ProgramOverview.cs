namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public sealed class ProgramOverview
    {
        public ProgramOverview(string[] programs, string infoProduct)
        {
            Programs = programs;
            InfoProduct = infoProduct;
        }

        public string[] Programs { get; private set; }    

        public string InfoProduct { get; private set; }
    }
}