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
            User user,
            AccountInformation accountInformation,
            Contact contact,
            DeliveryInformation deliveryInformation,
            ProgramForRead program,
            OtherForRead other,
            DateTime? finishingDate,
            UserName changedByUser,
            DateTime changedDate,
            DateTime createdDate)
            : base(orderer, user, accountInformation, contact, deliveryInformation)
        {
            this.Id = id;
            this.ActivityId = activityId;
            this.Program = program;
            this.Other = other;
            this.FinishingDate = finishingDate;
            this.ChangedByUser = changedByUser;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }

        public int ActivityId { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public UserName ChangedByUser { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public ProgramForRead Program { get; set; }

        public OtherForRead Other { get; set; }
    }
}