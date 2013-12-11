using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ProgramIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Program> Programs { get; set; }
    }
}