using DH.Helpdesk.BusinessData.Models.Language.Output;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.ExtendedCase
{
    public class ExtendedFormViewModels
    {
        public DH.Helpdesk.Domain.Customer Customer { get; set; }

        public IEnumerable<DH.Helpdesk.Domain.CaseSolution> CustomerCaseSolutions { get; set; }
        public IEnumerable<DH.Helpdesk.Domain.CaseSolution> CustomerCaseSolutionsWithExtendedCaseForm { get; set; }

        public ExtendedCaseFormEntity ExtendedCaseForm { get; set; }

        public ExtendedCaseFormJsonModel FormFields { get; set; }

        public IList<ExtendedCaseFieldTranslation> FieldTranslations { get; set; }

        public IList<LanguageOverview> ActiveLanguages { get; set; }

        public bool ExtendedCaseFormInCases { get; set; }

    }
}
