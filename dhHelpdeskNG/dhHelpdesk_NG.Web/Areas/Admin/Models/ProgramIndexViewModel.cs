namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class ProgramIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Program> Programs { get; set; }
        public bool IsShowOnlyActive { get; set; }
    }
}