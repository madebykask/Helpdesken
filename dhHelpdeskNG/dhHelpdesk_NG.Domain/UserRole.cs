using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class UserRole : Entity
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<User> Users { get; set; } 
    }

    public class UsersUserRole
    {
        public int User_Id { get; set; }
        public int UserRole_Id { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
