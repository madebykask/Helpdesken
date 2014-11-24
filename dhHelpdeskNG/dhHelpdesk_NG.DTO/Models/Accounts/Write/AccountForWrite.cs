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
            ProgramForWrite program,
            OtherForWrite other)
            : base(orderer, user, accountInformation, contact, deliveryInformation, program, other)
        {
        }
    }
}