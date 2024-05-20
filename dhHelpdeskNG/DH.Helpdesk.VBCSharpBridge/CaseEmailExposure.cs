using DH.Helpdesk.Services.Infrastructure.Email;
using DH.Helpdesk.Domain;
using DH.Helpdesk.VBCSharpBridge.Interfaces;
using DH.Helpdesk.VBCSharpBridge.Models;
using DH.Helpdesk.VBCSharpBridge.Resolver;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.BusinessData.Models.Case;
using System.Net.Mail;
using System.Collections.Generic;
using System.Reflection;
using DH.Helpdesk.BusinessData.Models.MailTemplates;
using DH.Helpdesk.Domain.MailTemplates;
using DH.Helpdesk.BusinessData.Models.Feedback;

namespace DH.Helpdesk.VBCSharpBridge
{

    public class CaseEmailExposure : ICaseEmailExposure
    {
        private readonly ICaseMailer _caseMailer;

        public CaseEmailExposure()
        {
            _caseMailer = ServiceResolver.GetCaseMailerService();
        }

        public string GetExternalLogTextHistory(int caseId, int logId, string helpdeskAddress)
        {
            CaseLogExposure caseLogExposure = new CaseLogExposure();
            CaseLog caseLog = caseLogExposure.GetCaseLog(logId);

            CaseExposure caseExposure = new CaseExposure();
            Case caseObj = caseExposure.GetCaseById(caseId);

            CaseEmailBridge caseEmailBridge = new CaseEmailBridge();
            caseEmailBridge.ExternalHistoryBody = _caseMailer.GetExternalLogTextHistory(caseObj, helpdeskAddress, caseLog);
            return caseEmailBridge.ExternalHistoryBody;
        }

    }
}
