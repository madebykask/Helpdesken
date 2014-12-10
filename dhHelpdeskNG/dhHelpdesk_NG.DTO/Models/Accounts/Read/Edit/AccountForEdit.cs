namespace DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit
{
    using System;

    using DH.Helpdesk.Common.Types;

    public class AccountForEdit : Account
    {
        public AccountForEdit(
            int id,
            int activityId,
            Orderer orderer,
            UserForEdit user,
            AccountInformation accountInformation,
            Contact contact,
            DeliveryInformation deliveryInformation,
            Program program,
            Other other,
            DateTime? finishingDate,
            UserName changedByUser,
            DateTime changedDate,
            DateTime createdDate)
            : base(orderer, accountInformation, contact, deliveryInformation, program, other)
        {
            this.Id = id;
            this.ActivityId = activityId;
            this.User = user;
            this.FinishingDate = finishingDate;
            this.ChangedByUser = changedByUser;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }

        public int ActivityId { get; private set; }

        public UserForEdit User { get; set; }

        public DateTime? FinishingDate { get; private set; }

        public UserName ChangedByUser { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}