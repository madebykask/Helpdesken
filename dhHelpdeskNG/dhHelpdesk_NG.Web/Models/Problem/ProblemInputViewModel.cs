namespace dhHelpdesk_NG.Web.Models.Problem
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Models.Problem.Output;

    public class ProblemInputViewModel
    {
        public IList<ProblemOutputModel> Problems { get; set; }

        public IList<SelectListItem> Customers { get; set; }

        public Enums.Show Show { get; set; }
    }
}