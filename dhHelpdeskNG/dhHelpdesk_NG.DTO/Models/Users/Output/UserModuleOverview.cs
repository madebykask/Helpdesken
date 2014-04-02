namespace DH.Helpdesk.BusinessData.Models.Users.Output
{
    public sealed class UserModuleOverview
    {
        public int User_Id { get; set; }
        public int Module_Id { get; set; }
        public int Position { get; set; }
        public bool isVisible { get; set; }
        public int NumberOfRows { get; set; }
        public ModuleOverview Module { get; set; }         
    }
}