namespace DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit
{
    using System;

    using DH.Helpdesk.BusinessData.Models.User.Input;

    public class AccountForEdit : Account
    {
        public AccountForEdit(
            Orderer orderer,
            User user,
            AccountInformation accountInformation,
            Contact contact,
            DeliveryInformation deliveryInformation,
            Program program,
            Other other,
            int number,
            DateTime? finishingDate,
            UserOverview createdByUser,
            DateTime changedDate,
            DateTime createdDate)
            : base(orderer, user, accountInformation, contact, deliveryInformation, program, other)
        {
            this.Number = number;
            this.FinishingDate = finishingDate;
            this.CreatedByUser = createdByUser;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }

        public int Number { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public UserOverview CreatedByUser { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}