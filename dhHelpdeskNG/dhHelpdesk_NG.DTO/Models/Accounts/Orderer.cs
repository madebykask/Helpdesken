namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class Orderer
    {
        public Orderer(int? id, string firstName, string lastName, string phone, string email)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.Email = email;
        }

        [IsId]
        public int? Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Phone { get; private set; }

        public string Email { get; private set; }
    }
}