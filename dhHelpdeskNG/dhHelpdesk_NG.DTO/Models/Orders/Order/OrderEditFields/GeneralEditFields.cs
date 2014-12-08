namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GeneralEditFields
    {
        public GeneralEditFields(
                string orderNumber, 
                string customer, 
                int? administratorId, 
                int? domainId, 
                DateTime? orderDate)
        {
            this.OrderDate = orderDate;
            this.DomainId = domainId;
            this.AdministratorId = administratorId;
            this.Customer = customer;
            this.OrderNumber = orderNumber;
        }

        [MinValue(0)]
        public string OrderNumber { get; private set; }
        
        public string Customer { get; private set; }
        
        [IsId]
        public int? AdministratorId { get; private set; }
        
        [IsId]
        public int? DomainId { get; private set; }
        
        public DateTime? OrderDate { get; private set; }    
    }
}