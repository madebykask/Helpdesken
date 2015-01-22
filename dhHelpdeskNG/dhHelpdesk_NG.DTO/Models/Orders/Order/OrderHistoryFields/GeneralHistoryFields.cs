namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    using System;

    using DH.Helpdesk.Common.Types;

    public sealed class GeneralHistoryFields
    {
        public GeneralHistoryFields(
                int? administratorId,
                UserName administrator, 
                int? domainId,
                string domain, 
                DateTime? orderDate)
        {
            this.AdministratorId = administratorId;
            this.DomainId = domainId;
            this.OrderDate = orderDate;
            this.Domain = domain;
            this.Administrator = administrator;
        }
        
        public int? AdministratorId { get; private set; }
        
        public UserName Administrator { get; private set; }
        
        public int? DomainId { get; private set; }

        public string Domain { get; private set; }
        
        public DateTime? OrderDate { get; private set; }    
    }
}