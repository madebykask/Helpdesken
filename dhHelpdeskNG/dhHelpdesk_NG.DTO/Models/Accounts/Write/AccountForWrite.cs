namespace DH.Helpdesk.BusinessData.Models.Accounts.Write
{
    public abstract class AccountForWrite : Account
    {
        protected AccountForWrite(
            Orderer orderer,
            User user,
            AccountInformation accountInformation,
            Contact contact,
            DeliveryInformation deliveryInformation,
            Program program,
            Other other)
            : base(orderer, accountInformation, contact, deliveryInformation, program, other)
        {
            this.User = user;
        }

        public User User { get; set; }
    }
}
