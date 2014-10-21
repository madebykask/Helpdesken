namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    public sealed class CustomerChanges
    {
        public CustomerChanges(
                int customerId, 
                string customerName, 
                int changesInProgress, 
                int changesClosed, 
                int changesForUser)
        {
            this.ChangesForUser = changesForUser;
            this.ChangesClosed = changesClosed;
            this.ChangesInProgress = changesInProgress;
            this.CustomerName = customerName;
            this.CustomerId = customerId;
        }

        public int CustomerId { get; private set; }

        public string CustomerName { get; private set; }

        public int ChangesInProgress { get; private set; }

        public int ChangesClosed { get; private set; }

        public int ChangesForUser { get; private set; }
    }
}