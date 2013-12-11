using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Models
{
    public class ProblemInputViewModel
    {
        public Problem Problem { get; set; }

        public IList<SelectListItem> Customers { get; set; }
        public IList<SelectListItem> Users { get; set; }
    }
}