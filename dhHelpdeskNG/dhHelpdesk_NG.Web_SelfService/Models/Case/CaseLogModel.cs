using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.SelfService.Models.Case
{
    using Log = DH.Helpdesk.Domain.Log;

    public class CaseLogModel 
    {
        public CaseLogModel()
        { 
        }

        public CaseLogModel(int caseId, List<Log> caseLogs)
        {
            this.CaseId = caseId;
            this.CaseLogs = caseLogs;
        }

        public int CaseId { get; set; }
        public List<Log> CaseLogs { get; set; }

        public Log GetLastInteral()
        {
            return CaseLogs?.Where(l => !string.IsNullOrEmpty(l.Text_Internal.Trim()))
                .OrderBy(l => l.RegTime)
                .LastOrDefault();
        }
    }
}
