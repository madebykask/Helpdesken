using System.ComponentModel.DataAnnotations;

namespace dhHelpdesk_NG.Domain
{
    public class ReportCustomer
    {
        public int Customer_Id { get; set; }
        public int Report_Id { get; set; }
        public int ShowOnPage { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
