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
                IEnumerable<CustomerChange> changes, 
                int userId)
        {
            this.UserId = userId;
            this.Changes = changes;
            this.Customers = customers;
        }

        public IList<CustomerUser> Customers { get; private set; }
 
        public IEnumerable<CustomerChange> Changes { get; private set; }

        public int UserId { get; private set; }

        public int GetInProgressCount(int customerId)
        {
            return this.Changes.Count(c => c.CustomerId == customerId && (c.ChangeStatus == null || c.ChangeStatus.CompletionStatus == 0));
        }

        public int GetClosedCount(int customerId)
        {
            return this.Changes.Count(c => c.CustomerId == customerId && c.ChangeStatus != null && c.ChangeStatus.CompletionStatus != 0);
        }

        public int GetUserCount(int customerId)
        {
            return this.Changes.Count(c => c.CustomerId == customerId && c.UserId == this.UserId);
        }
    }
}