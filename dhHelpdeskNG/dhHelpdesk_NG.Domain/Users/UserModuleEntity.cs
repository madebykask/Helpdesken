namespace DH.Helpdesk.Domain.Users
{
    public class UserModuleEntity : Entity
    {
        public int User_Id { get; set; }
        public int Module_Id { get; set; }
        public int Position { get; set; }
        public bool isVisible { get; set; }
        public int NumberOfRows { get; set; }

        public virtual User User { get; set; }
        public virtual ModuleEntity Module { get; set; }
    }
}