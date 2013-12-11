using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs.Case;

namespace dhHelpdesk_NG.Web.Models
{
    public class DefaultSearchModel
    {
        public ISearch Search { get; set; }
    }

    public class CaseSearchModel : DefaultSearchModel
    {
        public CaseSearchFilter caseSearchFilter { get; set; }
    }

}