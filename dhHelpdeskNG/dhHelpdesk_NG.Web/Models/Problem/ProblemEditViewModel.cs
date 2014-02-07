namespace DH.Helpdesk.Web.Models.Problem
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ProblemEditViewModel
    {
        public ProblemEditViewModel()
        {
            this.Logs = new List<LogOutputModel>();

            this.Cases = new List<CaseOutputModel>();

            this.Users = new List<SelectListItem>();
        }

        public ProblemEditModel Problem { get; set; }

        public List<LogOutputModel> Logs { get; set; }

        public List<CaseOutputModel> Cases { get; set; }

        public List<SelectListItem> Users { get; set; }
    }
}