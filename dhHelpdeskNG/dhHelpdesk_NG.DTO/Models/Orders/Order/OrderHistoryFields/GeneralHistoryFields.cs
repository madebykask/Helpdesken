namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    using System;

    public sealed class GeneralHistoryFields
    {
        public GeneralHistoryFields(
                int orderNumber, 
                string customer, 
                string administrator, 
                string domain, 
                DateTime? orderDate)
        {
            this.OrderDate = orderDate;
            this.Domain = domain;
            this.Administrator = administrator;
            this.Customer = customer;
            this.OrderNumber = orderNumber;
        }

        public int OrderNumber { get; private set; }
        
        public string Customer { get; private set; }
        
        public string Administrator { get; private set; }
        
        public string Domain { get; private set; }
        
        public DateTime? OrderDate { get; private set; }    
    }
}