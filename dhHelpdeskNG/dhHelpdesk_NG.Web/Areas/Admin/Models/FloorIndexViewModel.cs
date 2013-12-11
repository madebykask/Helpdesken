using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class FloorIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Floor> Floors { get; set; }
    }
}