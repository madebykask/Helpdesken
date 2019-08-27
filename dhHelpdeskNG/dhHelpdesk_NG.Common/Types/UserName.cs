namespace DH.Helpdesk.Common.Types
{
    public sealed class UserName
    {
        public UserName()
        {

        }

        public UserName(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string GetFullName()
        {
            return string.Format("{0} {1}", this.FirstName, this.LastName);
        }

        public string GetReversedFullName()
        {
            return string.Format("{0} {1}", this.LastName, this.FirstName);
        }
    }
}
