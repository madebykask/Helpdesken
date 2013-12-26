namespace dhHelpdesk_NG.Web.Models.Problem.Output
{
    using System.Collections.Generic;

    public class ProblemOutputModel
    {
        public ProblemOutputModel()
        {
            this.Logs = new List<LogOutputModel>();

            this.Cases = new List<CaseOutputModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int ProblemNumber { get; set; }

        public string Description { get; set; }

        public string ResponsibleUser { get; set; }

        public List<LogOutputModel> Logs { get; set; }

        public List<CaseOutputModel> Cases { get; set; }
    }
}