namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    using DH.Helpdesk.Domain; 

    public interface IEmailService
    {
        void SendEmail(string from, string to, string subject, string body, List<Field> fields, string mailMessageId = "", bool highPriority = false, List<string> files = null);
        string GetMailMessageId(string helpdeskFromAddress);
        bool IsValidEmail(string inputEmail);
    }
}