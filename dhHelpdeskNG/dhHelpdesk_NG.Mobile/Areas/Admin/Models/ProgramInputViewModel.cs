namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using DH.Helpdesk.Domain;

    public class ProgramInputViewModel
    {
        public int ShowOrder { get; set; }
        public int ShowAccount { get; set; }

        public Program Program { get; set; }
        public Customer Customer { get; set; }
    }
}