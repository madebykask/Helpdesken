namespace DH.Helpdesk.BusinessData.Models.Projects.Output
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;

    public class ProjectOverview
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CustomerId { get; set; }

        public int? ProjectManagerId { get; set; }

        public string ProjectManagerName { get; set; }

        public int IsActive { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
