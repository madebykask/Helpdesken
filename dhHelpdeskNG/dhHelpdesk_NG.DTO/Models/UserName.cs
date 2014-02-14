namespace DH.Helpdesk.BusinessData.Models
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserName
    {
        public UserName(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        [NotNullAndEmpty]
        public string FirstName { get; private set; }

        [NotNullAndEmpty]
        public string LastName { get; private set; }
    }
}
