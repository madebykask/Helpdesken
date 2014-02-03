using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class EmailGroupIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<EmailGroupEntity> EmailGroups { get; set; }
    }

    public class EmailGroupInputViewModel
    {
        public Customer Customer { get; set; }
        public EmailGroupEntity EmailGroup { get; set; }
    }
}