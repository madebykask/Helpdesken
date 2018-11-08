using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Common.Models.CaseSearch
{
    public class CaseSearchModel 
    {
        public CaseSearchFilter CaseSearchFilter { get; set; }

        public ISearch Search { get; set; }
       
    }
}