namespace DH.Helpdesk.Common.Types
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserIdentity
    {
        public UserIdentity()
        {            
        }
        
        public string Domain { get; set; }

        public string UserId { get; set; }

        public string EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string Email { get; set; }

        public string Phone { get; set; }

    }
}
