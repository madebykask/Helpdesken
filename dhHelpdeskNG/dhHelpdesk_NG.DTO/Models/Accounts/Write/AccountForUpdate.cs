namespace DH.Helpdesk.BusinessData.Models.Accounts.Write
{
    using System;

    public class AccountForUpdate : AccountForWrite
    {
        public AccountForUpdate(
            int id,
            int activityId,
            Orderer orderer,
            User user,
            AccountInformation accountInformation,
            Contact contact,
            DeliveryInformation deliveryInformation,
            Program program,
            Other other,
            DateTime changedDate,
            DateTime? finishDate,
            int changedByUserId)
            : base(orderer, user, accountInformation, contact, deliveryInformation, program, other)
        {
            this.Id = id;
            this.ActivityId = activityId;
            this.ChangedDate = changedDate;
            this.ChangedByUserId = changedByUserId;
            this.FinishDate = finishDate;
        }

        public int ActivityId { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public int ChangedByUserId { get; private set; }

        public DateTime? FinishDate { get; private set; }
    }
}