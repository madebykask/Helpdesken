using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class OperationObjectInputViewModel
    {
        public int ShowYesNo { get; set; }
        public int StartPageShow { get; set; }
        
        public OperationObject OperationObject { get; set; }
        public Customer Customer { get; set; }
        public IList<SelectListItem> WorkingGroups { get; set; }
        public List<SelectListItem> NumberToShowOnStartPage { get; set; }
    }
}