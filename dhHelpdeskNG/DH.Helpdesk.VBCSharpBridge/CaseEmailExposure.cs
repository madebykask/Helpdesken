using DH.Helpdesk.Services.Infrastructure.Email;
using DH.Helpdesk.Domain;
using DH.Helpdesk.VBCSharpBridge.Resolver;
using DH.Helpdesk.BusinessData.Models.Case;

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
