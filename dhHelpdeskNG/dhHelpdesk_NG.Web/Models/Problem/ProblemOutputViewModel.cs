namespace DH.Helpdesk.Web.Models.Problem
{
    using System.Collections.Generic;

    using DH.Helpdesk.Web.Infrastructure;

    public class ProblemOutputViewModel
    {
        public IList<ProblemOutputModel> Problems { get; set; }

        public Enums.Show Show { get; set; }
    }
}