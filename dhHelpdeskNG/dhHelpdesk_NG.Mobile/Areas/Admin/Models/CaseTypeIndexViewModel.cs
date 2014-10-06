namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class CaseTypeIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<CaseType> CaseTypes { get; set; }
    }
}