namespace DH.Helpdesk.Common.Types
{
    public sealed class UserName
    {
        public UserName(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string GetFullName()
        {
            return string.Format("{0} {1}", this.FirstName, this.LastName);
        }
    }
}
