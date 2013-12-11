using System;

namespace dhHelpdesk_NG.Domain
{
    public class UsersPasswordHistory : Entity
    {
        public int User_Id { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
