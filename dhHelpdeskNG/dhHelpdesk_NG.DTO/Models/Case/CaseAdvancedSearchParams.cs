namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseAdvancedSearchParams
    {
        public CaseAdvancedSearchParams()
        {
            this.CustomerIds = new List<int>();
        }

        public CaseAdvancedSearchParams(List<int> customerIds)
        {
            this.CustomerIds = customerIds;
        }

        [NotNull]
        public List<int> CustomerIds { get; private set; } 
    }
}