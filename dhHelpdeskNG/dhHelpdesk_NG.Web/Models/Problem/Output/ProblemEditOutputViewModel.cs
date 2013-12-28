namespace dhHelpdesk_NG.Web.Models.Problem.Output
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ProblemEditOutputViewModel
    {
        public ProblemEditOutputModel Problem { get; set; }

        public List<SelectListItem> Users { get; set; }
    }
}