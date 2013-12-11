using System.ComponentModel.DataAnnotations;

namespace dhHelpdesk_NG.Domain
{
    public class UserWorkingGroup
    {
        public int User_Id { get; set; }
        public int UserRole { get; set; }
        public int WorkingGroup_Id { get; set; }

        public virtual User User { get; set; }
    }
}
