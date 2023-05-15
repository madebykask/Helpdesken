using System.Collections.Generic;

using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.MailTemplates;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Infrastructure.Email
{
    public interface ICaseMailer
    {
        void InformNotifierIfNeeded(
                int caseHistoryId,
                List<Field> fields,
                CaseLog log,                
                bool dontSendMailToNotfier,
                Case newCase,
                string helpdeskMailFromAdress,
                List<MailFile> files,
                MailSenders mailSenders,
                bool isCreatingCase,
                bool caseMailSetting_DontSendMail,
                string absoluterUrl,
                string extraFollowersEmails = null);

        void InformOwnerDefaultGroupIfNeeded(
                int caseHistoryId,
                List<Field> fields,
                CaseLog log,
                bool dontSendMailToNotfier,
                Case newCase,
                string helpdeskMailFromAdress,
                List<MailFile> files,
                string absoluterUrl);

        void InformAboutInternalLogIfNeeded(
                int caseHistoryId,
                List<Field> fields,
                CaseLog log,
                Case newCase,
                string helpdeskMailFromAdress,
                List<MailFile> files,
                string absoluterUrl,
                MailSenders mailSenders);
        string GetExternalLogTextHistory(Case newCase, string helpdeskMailFromAdress, CaseLog log);
    }
}