namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;

    public abstract class Account : BusinessModel
    {
        protected Account(
            Orderer orderer,
            AccountInformation accountInformation,
            Contact contact,
            DeliveryInformation deliveryInformation,
            Program program,
            Other other)
        {
            this.Orderer = orderer;
            this.AccountInformation = accountInformation;
            this.Contact = contact;
            this.DeliveryInformation = deliveryInformation;
            this.Program = program;
            this.Other = other;
        }

        public Orderer Orderer { get; private set; }

        public AccountInformation AccountInformation { get; private set; }

        public Contact Contact { get; private set; }

        public DeliveryInformation DeliveryInformation { get; private set; }

        public Program Program { get; set; }

        public Other Other { get; set; }
    }
}
