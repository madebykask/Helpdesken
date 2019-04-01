namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
    public sealed class CustomerCasesStatus
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int MyCases { get; set; }
        public int InProgress { get; set; }
        public int NewToday { get; set; }
        public int ClosedToday { get; set; }
    }
  
}