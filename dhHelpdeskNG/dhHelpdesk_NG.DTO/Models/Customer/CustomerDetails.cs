using System;

namespace DH.Helpdesk.BusinessData.Models.Customer
{
    public class CustomerDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CustomerID { get; set; }
        public string CustomerNumber { get; set; }
        public Guid CustomerGUID { get; set; }
        public string HelpdeskEmail { get; set; }
        public int? CustomerGroup_Id { get; set; }
        public int Language_Id { get; set; }
        //public string Address { get; set; }
        //public string Phone { get; set; }
        //public string PostalAddress { get; set; }
        //public string PostalCode { get; set; }
        //public DateTime ChangeTime { get; set; }
        //public DateTime RegTime { get; set; }
    }
}