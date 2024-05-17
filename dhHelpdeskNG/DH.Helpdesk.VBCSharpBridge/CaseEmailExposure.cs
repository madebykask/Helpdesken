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
        private readonly IEmailService _emailService;
        private readonly IMailTemplateService _templateService;

        public CaseEmailExposure()
        {
            _caseMailer = ServiceResolver.GetCaseMailerService();
            _emailService = ServiceResolver.GetEmailService();
            _templateService = ServiceResolver.GetTemplateService();
        }

        public string GetExternalLogTextHistory(int caseId, int logId, string helpdeskAddress)
        {
            CaseLogExposure caseLogExposure = new CaseLogExposure();
            CaseLog caseLog = caseLogExposure.GetCaseLog(logId);

            CaseExposure caseExposure = new CaseExposure();
            Case caseObj = caseExposure.GetCaseById(caseId);
            //Behöver inte denna
            CaseEmailBridge history = new CaseEmailBridge();
            history.ExternalHistoryBody = _caseMailer.GetExternalLogTextHistory(caseObj, helpdeskAddress, caseLog);
            return history.ExternalHistoryBody;
        }
        public MailMessage GetMailMessage(int customerId, int caseId, int mailtemplateId, string toEmail, string helpdeskEmail, List<Field> fieldList)
        {
            CaseExposure caseExposure = new CaseExposure();
            Case caseObj = caseExposure.GetCaseById(caseId);
            MailTemplateEntity mt = _templateService.GetMailTemplate(mailtemplateId, customerId);
            return _emailService.GetMailMessage(helpdeskEmail, toEmail, "", "Hej Katta", "body", fieldList);
        }


    }
}
