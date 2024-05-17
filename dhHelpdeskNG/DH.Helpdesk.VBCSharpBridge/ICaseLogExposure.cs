using DH.Helpdesk.BusinessData.Models.Case;

namespace DH.Helpdesk.VBCSharpBridge
{
    public interface ICaseLogExposure
    {
        CaseLog GetCaseLog(int caseId);
    }
}