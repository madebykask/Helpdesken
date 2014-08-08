namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using System;

    public sealed class CustomerChange
    {
        public CustomerChange(
                int customerId, 
                int changeId, 
                DateTime? finishingDate, 
                int? userId)
        {
            this.UserId = userId;
            this.FinishingDate = finishingDate;
            this.ChangeId = changeId;
            this.CustomerId = customerId;
        }

        public int CustomerId { get; private set; }

        public int ChangeId { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public int? UserId { get; private set; }
    }
}