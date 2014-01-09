namespace dhHelpdesk_NG.Web.Models.Problem
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Web.Infrastructure;

    public class ProblemOutputViewModel
    {
        public IList<ProblemOutputModel> Problems { get; set; }

        public Enums.Show Show { get; set; }
    }
}