using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class OperationObjectIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<OperationObject> OperationObjects { get; set; }
    }
}