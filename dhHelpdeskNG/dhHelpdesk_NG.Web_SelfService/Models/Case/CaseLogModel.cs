using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DH.Helpdesk.SelfService.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models;

    using Log = DH.Helpdesk.Domain.Log;

    

    public class CaseLogModel 
    {
        public CaseLogModel()
        { 
        }

        public CaseLogModel(int caseId, List<Log> caseLogs)
        {
            this.CaseId = caseId;
            this.CaseLogs = CaseLogs;
        }

        public int CaseId { get; set; }

        public List<Log> CaseLogs { get; set; }       
                
    }
  
}
