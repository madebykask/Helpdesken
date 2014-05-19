namespace DH.Helpdesk.NewSelfService.Models
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;

    public class DefaultSearchModel
    {
        public ISearch Search { get; set; }
    }

    public class CaseSearchModel : DefaultSearchModel
    {
        public CaseSearchFilter caseSearchFilter { get; set; }
    }

}