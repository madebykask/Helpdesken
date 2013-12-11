using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ProgramInputViewModel
    {
        public int ShowOrder { get; set; }
        public int ShowAccount { get; set; }

        public Program Program { get; set; }
        public Customer Customer { get; set; }
    }
}