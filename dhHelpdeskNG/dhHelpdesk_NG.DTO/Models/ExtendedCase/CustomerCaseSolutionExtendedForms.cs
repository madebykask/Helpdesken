using DH.Helpdesk.Domain.ExtendedCaseEntity;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.ExtendedCase
{
    public class CustomerCaseSolutionsExtendedForm
    {
        public DH.Helpdesk.Domain.Customer Customer { get; set; }

        public IEnumerable<DH.Helpdesk.Domain.CaseSolution> CustomerCaseSolutions { get; set; }
        public IEnumerable<DH.Helpdesk.Domain.CaseSolution> CustomerCaseSolutionsWithExtendedCaseForm { get; set; }

        public ExtendedCaseFormEntity ExtendedCaseForm { get; set; }

        public ExtendedCaseFormJsonModel FormFields { get; set; }

        public IList<ExtendedCaseFieldTranslation> FieldTranslations { get; set; }

    }
}
