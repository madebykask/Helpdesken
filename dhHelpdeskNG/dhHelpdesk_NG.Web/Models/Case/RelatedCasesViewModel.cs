namespace DH.Helpdesk.Web.Models.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class RelatedCasesViewModel
    {
        public RelatedCasesViewModel(
            List<RelatedCase> relatedCases, 
            int customerId)
        {
            this.CustomerId = customerId;
            this.RelatedCases = relatedCases;
        }

        [NotNull]
        public List<RelatedCase> RelatedCases { get; private set; }  

        [IsId]
        public int CustomerId { get; private set; }
    }
}