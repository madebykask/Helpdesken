namespace DH.Helpdesk.BusinessData.Models.Accounts.Write
{
    using System;

    public class AccountForUpdate : AccountForWrite
    {
        public AccountForUpdate(
            int id,
            Orderer orderer,
            User user,
            AccountInformation accountInformation,
            Contact contact,
            DeliveryInformation deliveryInformation,
            ProgramForWrite program,
            OtherForWrite other,
            DateTime changedDate,
            int changedByUserId)
            : base(orderer, user, accountInformation, contact, deliveryInformation, program, other)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
            this.ChangedByUserId = changedByUserId;
        }

        public DateTime ChangedDate { get; private set; }

        public int ChangedByUserId { get; private set; }
    }
}