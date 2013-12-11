using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class StandardTextIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<StandardText> StandardTexts { get; set; }
    }

    public class StandardTextInputViewModel
    {
        public Customer Customer { get; set; }
        public StandardText StandardText { get; set; }
    }
}