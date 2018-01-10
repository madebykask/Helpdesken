namespace DH.Helpdesk.BusinessData.Models.Customer
{
    public class UserCustomerOverview
    {
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CasesCount { get; set; }
    }
}