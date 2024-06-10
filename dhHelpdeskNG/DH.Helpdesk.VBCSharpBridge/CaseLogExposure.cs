using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.VBCSharpBridge.Resolver;

namespace DH.Helpdesk.VBCSharpBridge
{
    public class CaseLogExposure : ICaseLogExposure
    {
        private readonly ILogService _caseLogService;

        public CaseLogExposure()
        {
            _caseLogService = ServiceResolver.GetCaseLogService();
        }

        public CaseLog GetCaseLog(int caseId)
        {
            return _caseLogService.GetLogById(caseId);
        }
    }
}
