using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class InfoTextShowViewModel
    {
        public Customer Customer { get; set; }
        public InfoText InfoText { get; set; }
    }

    public class InfoTextIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<InfoText> InfoTexts { get; set; }
    }

    public class InfoTextInputViewModel
    {
        public Customer Customer { get; set; }
        public InfoText InfoText { get; set; }
      
        public IList<SelectListItem> Languages { get; set; }

        public InfoTextShowViewModel InfoTextShowViewModel { get; set; }
    }
}