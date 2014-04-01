using System;

namespace DH.Helpdesk.BusinessData.Models.Problem.Output
{
    public class ProblemInfoOverview
    {
        public int Customer_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProblemNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}