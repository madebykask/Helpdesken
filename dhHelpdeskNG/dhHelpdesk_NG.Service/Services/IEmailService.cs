namespace DH.Helpdesk.Services.Services
{
    public interface IEmailService
    {
        void SendEmail(string from, string to, string subject, string body, bool highPriority = false);
        bool isEmail(string inputEmail);
    }
}