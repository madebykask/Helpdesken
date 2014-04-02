namespace DH.Helpdesk.Domain.Users
{
    public class UserModuleEntity : Entity
    {
        public int User_Id { get; set; }
        public int Module_Id { get; set; }
        public int Position { get; set; }

        private bool _isVisible = true;
        public bool isVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        public int NumberOfRows { get; set; }

        public virtual User User { get; set; }
        public virtual ModuleEntity Module { get; set; }
    }
}