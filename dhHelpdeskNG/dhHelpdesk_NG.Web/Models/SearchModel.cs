namespace DH.Helpdesk.Web.Models
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;

    public class CaseSearchModel 
    {
        public CaseSearchFilter caseSearchFilter { get; set; }

        public ISearch Search { get; set; }
       
    }
}