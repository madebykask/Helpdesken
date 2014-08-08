namespace DH.Helpdesk.Web.Models.Changes
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Domain;

    public sealed class CustomerChangesModel
    {
        public CustomerChangesModel()
        {
            
        }

        public CustomerChangesModel(
                IList<CustomerUser> customers, 
                IEnumerable<CustomerChange> changes)
        {
            this.Changes = changes;
            this.Customers = customers;
        }

        public IList<CustomerUser> Customers { get; private set; }
 
        public IEnumerable<CustomerChange> Changes { get; private set; }

        public int GetCustomerChangesCount(int customerId)
        {
            return this.Changes.Count(c => c.CustomerId == customerId);
        }
    }
}