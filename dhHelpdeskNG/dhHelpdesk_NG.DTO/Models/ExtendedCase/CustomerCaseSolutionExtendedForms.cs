using DH.Helpdesk.Domain.ExtendedCaseEntity;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.ExtendedCase
{
    public class CustomerCaseSolutionsExtendedForm
    {
        public DH.Helpdesk.Domain.Customer Customer { get; set; }

        public IEnumerable<DH.Helpdesk.Domain.CaseSolution> CustomerCaseSolutions { get; set; }

        public ExtendedCaseFormEntity ExtendedCaseForm { get; set; }
    }
}
