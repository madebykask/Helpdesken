namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using DH.Helpdesk.Domain.Changes;

    public sealed class CustomerChange
    {
        public CustomerChange(
                int customerId, 
                int changeId, 
                ChangeStatusEntity changeStatus,
                int? userId)
        {
            this.ChangeStatus = changeStatus;
            this.UserId = userId;
            this.ChangeId = changeId;
            this.CustomerId = customerId;
        }

        public int CustomerId { get; private set; }

        public int ChangeId { get; private set; }

        public ChangeStatusEntity ChangeStatus { get; private set; }

        public int? UserId { get; private set; }
    }
}