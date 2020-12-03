namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class SystemIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Helpdesk.Domain.System> System { get; set; }
        public bool IsShowOnlyActive { get; set; }
    }
}