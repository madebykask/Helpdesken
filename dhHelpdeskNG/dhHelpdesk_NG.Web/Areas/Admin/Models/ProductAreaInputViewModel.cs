using DH.Helpdesk.BusinessData.Models.Case.Output;

namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class ProductAreaInputViewModel
    {
        public ProductArea ProductArea { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> MailTemplates { get; set; }
        public IList<SelectListItem> WorkingGroups { get; set; }
        public IList<SelectListItem> Priorities { get; set; }
        public IList<SelectListItem> WgAvailable { get; set; }
        public IList<SelectListItem> WgSelected { get; set; }
        public IList<CaseTypeOverview> CaseTypes { get; set; }
        public int CaseType_Id { get; set; }
        public string ParentPath_CaseType { get; set; }

        public bool CanAddChild { get; set; }
    }
}