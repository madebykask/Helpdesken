namespace DH.Helpdesk.Mobile.Models.Problem
{
    using System.Collections.Generic;

    using DH.Helpdesk.Mobile.Infrastructure;

    public class ProblemOutputViewModel
    {
        public IList<ProblemOutputModel> Problems { get; set; }

        public Enums.Show Show { get; set; }
    }
}