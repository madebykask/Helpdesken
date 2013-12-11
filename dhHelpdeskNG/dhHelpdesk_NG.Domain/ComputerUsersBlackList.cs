using System;

namespace dhHelpdesk_NG.Domain
{
    public class ComputerUsersBlackList : Entity
    {
        public string User_Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
