namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class StatusIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Status> Statuses { get; set; }
    }
}