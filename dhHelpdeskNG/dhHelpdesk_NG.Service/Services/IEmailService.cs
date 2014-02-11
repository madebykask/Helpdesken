namespace DH.Helpdesk.Services.Services
{
    public interface IEmailService
    {
        void SendEmail(string from, string to, string subject, string body, string mailMessageId, bool highPriority = false);
        string GetMailMessageId(string helpdeskFromAddress);
        bool IsValidEmail(string inputEmail);
    }
}