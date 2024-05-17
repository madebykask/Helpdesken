using DH.Helpdesk.Domain;
using System.Collections.Generic;
using System.Net.Mail;

namespace DH.Helpdesk.VBCSharpBridge
{
    public interface ICaseEmailExposure
    {
        string GetExternalLogTextHistory(int caseId, int logId, string helpdeskAddress);
        MailMessage GetMailMessage(int customerId, int caseId, int mailtemplateId, string toEmail, string helpdeskEmail, List<Field> fieldList);
    }
}