namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    public sealed class CustomerChange
    {
        public CustomerChange(int customerId, int changeId)
        {
            this.ChangeId = changeId;
            this.CustomerId = customerId;
        }

        public int CustomerId { get; private set; }

        public int ChangeId { get; private set; }
    }
}