using System.Collections.Generic;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.Admin.Models
{

    using DH.Helpdesk.Domain;

    public class RegistrationSourceInputViewModel
    {
        public RegistrationSourceCustomer RegistrationSourceCustomer { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> SystemCode { get; set; }

    }
}