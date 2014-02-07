namespace DH.Helpdesk.Web.Models.Problem
{
    public class ProblemOutputModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProblemNumber { get; set; }

        public string Description { get; set; }

        public string ResponsibleUserName { get; set; }
    }
}