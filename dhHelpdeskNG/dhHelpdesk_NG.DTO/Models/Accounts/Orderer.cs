namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    public sealed class Orderer
    {
        public Orderer(string id, string firstName, string lastName, string phone, string email)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.Email = email;
        }

        public string Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Phone { get; private set; }

        public string Email { get; private set; }
    }
}