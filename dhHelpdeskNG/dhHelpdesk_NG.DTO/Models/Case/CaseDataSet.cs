using System.Linq;
namespace DH.Helpdesk.BusinessData.Models.Case
{
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Cases;
    using System.Collections.Generic;
    public class CaseHelperDataSets
    {
        public CaseHelperDataSets()
        {
        
        }

        public IList<CaseType> CaseTypeQuery { get; set; }
        public IList<ProductArea> ProductAreaQuery { get; set; }
        public IList<FinishingCause> ClosingReasonQuery { get; set; }        
        public IList<OU> OrganizationUnitQuery { get; set; }
    }
}
