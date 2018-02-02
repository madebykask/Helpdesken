namespace DH.Helpdesk.Domain
{
    public class UserWorkingGroup
    {
        public int User_Id { get; set; }

        /// <summary>
        /// Users persmission in working group. 
        ///     0 - no access
        ///     1 - Readonly access
        ///     2 - Can act as Case Administrator 
        /// see WorkingGroupUserPermission.cs for constants
        /// </summary>
        public int UserRole { get; set; }

        public int WorkingGroup_Id { get; set; }

        public bool IsMemberOfGroup { get; set; }

        public int IsDefault { get; set; }

        public virtual User User { get; set; }
    }
}
