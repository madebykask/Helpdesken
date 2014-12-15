namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class AccountModel
    {
        public AccountModel()
        {
        }

        public AccountModel(
            Orderer orderer,
            User user,
            AccountInformation accountInformation,
            Contact contact,
            DeliveryInformation deliveryInformation,
            Program program,
            Other other)
        {
            this.Orderer = orderer;
            this.User = user;
            this.AccountInformation = accountInformation;
            this.Contact = contact;
            this.DeliveryInformation = deliveryInformation;
            this.Program = program;
            this.Other = other;
        }

        public int Id { get; set; }

        public int ActivityTypeId { get; set; }

        public string ActivityName { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        public UserName ChangedByUserName { get; set; }

        public bool IsCreateCase { get; set; }

        public DateTime? FinishDate { get; set; }

        public HeadersFieldSettings Headers { get; set; }

        [NotNull]
        public Orderer Orderer { get; set; }

        [NotNull]
        public User User { get; set; }

        [NotNull]
        public AccountInformation AccountInformation { get; set; }

        [NotNull]
        public Contact Contact { get; set; }

        [NotNull]
        public DeliveryInformation DeliveryInformation { get; set; }

        [NotNull]
        public Program Program { get; set; }

        [NotNull]
        public Other Other { get; set; }
    }
}