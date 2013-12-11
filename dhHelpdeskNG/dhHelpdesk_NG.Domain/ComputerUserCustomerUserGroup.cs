using System;

namespace dhHelpdesk_NG.Domain
{
    public class ComputerUserCustomerUserGroup : Entity
    {
        public int ComputerUser_Id { get; set; }
        public int ComputerUserGroup_Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
