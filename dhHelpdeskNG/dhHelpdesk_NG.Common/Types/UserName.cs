namespace DH.Helpdesk.Common.Types
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

        public string GetFullName()
        {
            return string.Format("{0} {1}", this.FirstName, this.LastName);
        }
    }
}
