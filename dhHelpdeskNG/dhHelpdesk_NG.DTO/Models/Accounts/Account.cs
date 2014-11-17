namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;

    public abstract class Account : BusinessModel
    {
        protected Account(
            Orderer orderer,
            User user,
            AccountInformation accountInformation,
            DeliveryInformation deliveryInformation,
            Program program,
            Other other)
        {
            this.Orderer = orderer;
            this.User = user;
            this.AccountInformation = accountInformation;
            this.DeliveryInformation = deliveryInformation;
            this.Program = program;
            this.Other = other;
        }

        public Orderer Orderer { get; private set; }

        public User User { get; private set; }

        public AccountInformation AccountInformation { get; private set; }

        public DeliveryInformation DeliveryInformation { get; private set; }

        public Program Program { get; private set; }

        public Other Other { get; private set; }
    }
}
