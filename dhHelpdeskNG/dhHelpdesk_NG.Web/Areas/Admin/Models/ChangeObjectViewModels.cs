using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ChangeObjectIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeObject> ChangeObjects { get; set; }
    }

    public class ChangeObjectInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeObject ChangeObject { get; set; }
    }
}