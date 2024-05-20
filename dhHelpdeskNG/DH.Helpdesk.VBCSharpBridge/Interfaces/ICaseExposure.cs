using DH.Helpdesk.Domain;
using DH.Helpdesk.VBCSharpBridge.Models;

namespace DH.Helpdesk.VBCSharpBridge.Interfaces
{
    public interface ICaseExposure
    {
        Case GetCaseById(int caseId);
        string GetSurveyBodyString(int customerId, int caseId, int mailtemplateId, string toEmail, string helpdeskEmail, string port, string helpdeskAddress, ref string body);
        CaseBridge RunBusinessRules(CaseBridge caseObj);
    }
}
