namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class OperationObjectIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<OperationObject> OperationObjects { get; set; }
    }
}