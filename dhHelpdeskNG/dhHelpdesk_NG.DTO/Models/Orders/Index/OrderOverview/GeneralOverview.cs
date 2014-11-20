namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    using System;

    public sealed class GeneralOverview
    {
        public GeneralOverview(
                string orderNumber, 
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

        public string OrderNumber { get; private set; }
        
        public string Customer { get; private set; }
        
        public string Administrator { get; private set; }
        
        public string Domain { get; private set; }
        
        public DateTime? OrderDate { get; private set; }     
    }
}