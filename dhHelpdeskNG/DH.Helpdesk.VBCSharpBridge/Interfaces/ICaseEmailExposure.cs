
namespace DH.Helpdesk.VBCSharpBridge
{
    public interface ICaseEmailExposure
    {
        string GetExternalLogTextHistory(int caseId, int logId, string helpdeskAddress);
    }
}