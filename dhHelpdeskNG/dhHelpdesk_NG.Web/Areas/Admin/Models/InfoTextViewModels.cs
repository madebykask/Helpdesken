namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Common.Enums;

    public class InfoTextShowViewModel
    {
        public Customer Customer { get; set; }
        public InfoText InfoText { get; set; }
        public InfoTextTypes InfoTextType { get; set; }
    }

    public class InfoTextIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<InfoText> InfoTexts { get; set; }
        public InfoTextTypes InfoTextType { get; set; }
    }

    public class InfoTextInputViewModel
    {
        public Customer Customer { get; set; }
        public InfoText InfoText { get; set; }
        public InfoTextTypes InfoTextType { get; set; }

        public IList<SelectListItem> Languages { get; set; }

        public InfoTextShowViewModel InfoTextShowViewModel { get; set; }
    }
}