namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public interface IEmailService
    {
        #region Public Methods and Operators

        string GetMailMessageId(string helpdeskFromAddress);

        bool IsValidEmail(string inputEmail);

        void SendEmail(string from, List<string> recipients, string subject, string body);

        void SendEmail(
            string from,
            string to,
            string subject,
            string body,
            List<Field> fields,
            string mailMessageId = "",
            bool highPriority = false,
            List<string> files = null);

        #endregion
    }
}