namespace DH.Helpdesk.BusinessData.Models.Problem.Output
{
    using System;

    public class ProblemLogOverview
    {
        public int Id { get; set; }

        public DateTime ChangedDate { get; set; }

        public string ChangedByUserName { get; set; }

        public string LogText { get; set; }
    }
}