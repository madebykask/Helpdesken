namespace DH.Helpdesk.BusinessData.Models.Accounts.Write
{
    using System;

    public class AccountForInsert : AccountForWrite
    {
        public AccountForInsert(
            Orderer orderer,
            User user,
            AccountInformation accountInformation,
            DeliveryInformation deliveryInformation,
            ProgramForWrite program,
            OtherForWrite other,
            int activityId,
            int customerId,
            DateTime createdDate,
            int createdByUserId)
            : base(orderer, user, accountInformation, deliveryInformation, program, other)
        {
            this.ActivityId = activityId;
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
            this.CreatedByUserId = createdByUserId;
        }

        public int ActivityId { get; private set; }

        public int CustomerId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int CreatedByUserId { get; private set; }
    }
}