namespace DH.Helpdesk.BusinessData.Models.Customer
{
    using DH.Helpdesk.Domain;

    public sealed class UserCustomer
    {
        public UserCustomer(
                int userId,
                CustomerUser customer, 
                CustomerSettings settings, 
                int active)
        {
            this.Settings = settings;
            this.Customer = customer;
            this.UserId = userId;
            this.Active = active;
        }

        public int UserId { get; private set; }

        public CustomerUser Customer { get; private set; }

        public CustomerSettings Settings { get; private set; }

        public int Active { get; private set; }

    }
}