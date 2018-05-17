namespace DH.Helpdesk.BusinessData.Models.Email
{
    public interface IEmailSearchScope
    {
        bool SearchInWorkingGrs { get; }
        bool SearchInInitiators { get; }
        bool SearchInAdmins { get; }
        bool SearchInUsers { get; }
        bool SearchInEmailGrs { get; }
    }

    public class EmailSearchScope : IEmailSearchScope
    {
        public bool SearchInWorkingGrs { get; set; } 
        public bool SearchInInitiators { get; set; }
        public bool SearchInAdmins { get; set; }
        public bool SearchInUsers { get; set; }
        public bool SearchInEmailGrs { get; set; }
    }
}