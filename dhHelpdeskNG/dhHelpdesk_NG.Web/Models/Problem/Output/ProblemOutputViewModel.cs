namespace dhHelpdesk_NG.Web.Models.Problem.Output
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Web.Infrastructure;

    public class ProblemOutputViewModel
    {
        public IList<ProblemOutputModel> Problems { get; set; }

        public IList<SelectListItem> Customers { get; set; }

        public Enums.Show Show { get; set; }
    }
}