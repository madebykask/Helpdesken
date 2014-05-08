namespace DH.Helpdesk.Services.Infrastructure.Email
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;

    public interface ICaseMailer
    {
        void InformNotifierIfNeeded(
                int caseHistoryId,
                List<Field> fields,
                CaseLog log,                
                bool dontSendMailToNotfier,
                Case newCase,
                string helpdeskMailFromAdress,
                List<string> files);

        void InformOwnerDefaultGroupIfNeeded(
                int caseHistoryId,
                List<Field> fields,
                CaseLog log,
                bool dontSendMailToNotfier,
                Case newCase,
                string helpdeskMailFromAdress,
                List<string> files);

        void InformAboutInternalLogIfNeeded(
                int caseHistoryId,
                List<Field> fields,
                CaseLog log,
                Case newCase,
                string helpdeskMailFromAdress,
                List<string> files);
    }
}