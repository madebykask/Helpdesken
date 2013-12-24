namespace dhHelpdesk_NG.Web.Models.Problem
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Web.Models.Problem.Input;

    public class ProblemInputViewModel
    {
        public IList<ProblemInputModel> Problems { get; set; }

        public IList<SelectListItem> Customers { get; set; }

        public IList<SelectListItem> ProblemStateses { get; set; }
    }
}