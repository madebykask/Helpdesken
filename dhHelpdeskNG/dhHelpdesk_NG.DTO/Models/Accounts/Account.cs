namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;

    public abstract class Account : BusinessModel
    {
        protected Account(
            Orderer orderer,
            User user,
            AccountInformation accountInformation,
            Contact contact,
            DeliveryInformation deliveryInformation)
        {
            this.Orderer = orderer;
            this.User = user;
            this.AccountInformation = accountInformation;
            this.Contact = contact;
            this.DeliveryInformation = deliveryInformation;
        }

        public Orderer Orderer { get; private set; }

        public User User { get; private set; }

        public AccountInformation AccountInformation { get; private set; }

        public Contact Contact { get; private set; }

        public DeliveryInformation DeliveryInformation { get; private set; }
    }
}
