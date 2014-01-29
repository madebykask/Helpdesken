using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    using dhHelpdesk_NG.Domain.Changes;

    public class ChangeObjectIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeObjectEntity> ChangeObjects { get; set; }
    }

    public class ChangeObjectInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeObjectEntity ChangeObject { get; set; }
    }
}