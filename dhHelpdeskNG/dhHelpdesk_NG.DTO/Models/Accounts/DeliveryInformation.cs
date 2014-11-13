namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    public sealed class DeliveryInformation
    {
        public DeliveryInformation(string name, string phone, string address, string postalAddress)
        {
            this.Name = name;
            this.Phone = phone;
            this.Address = address;
            this.PostalAddress = postalAddress;
        }

        public string Name { get; private set; }

        public string Phone { get; private set; }

        public string Address { get; private set; }

        public string PostalAddress { get; private set; }
    }
}