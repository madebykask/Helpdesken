using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class StatusInputViewModel
    {
        public Status Status { get; set; }
        public Customer Customer { get; set; }
    }
}