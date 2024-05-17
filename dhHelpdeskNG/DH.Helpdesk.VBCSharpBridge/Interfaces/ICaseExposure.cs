using DH.Helpdesk.Domain;
using DH.Helpdesk.VBCSharpBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.VBCSharpBridge.Interfaces
{
    public interface ICaseExposure
    {
        Case GetCaseById(int caseId);
        string GetSurveyBodyString(int customerId, int caseId, int mailtemplateId, string toEmail, string helpdeskEmail, string helpdeskAddress, ref string body);
        CaseBridge RunBusinessRules(CaseBridge caseObj);
    }
}
