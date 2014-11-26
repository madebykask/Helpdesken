namespace DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview
{
    public class AccountOverview
    {
        public AccountOverview(
            int id,
            int activityId,
            Orderer orderer,
            User user,
            AccountInformation accountInformation,
            Contact contact,
            DeliveryInformation deliveryInformation,
            Program program,
            Other other)
        {
            this.Id = id;
            this.ActivityId = activityId;
            this.Orderer = orderer;
            this.User = user;
            this.AccountInformation = accountInformation;
            this.Contact = contact;
            this.DeliveryInformation = deliveryInformation;
            this.Program = program;
            this.Other = other;
        }

        public int Id { get; private set; }

        public int ActivityId { get; private set; }

        public Orderer Orderer { get; private set; }

        public User User { get; private set; }

        public AccountInformation AccountInformation { get; private set; }

        public Contact Contact { get; private set; }

        public DeliveryInformation DeliveryInformation { get; private set; }

        public Program Program { get; private set; }

        public Other Other { get; private set; }
    }
}
