namespace DH.Helpdesk.BusinessData.Models.Projects.Output
{
    using System;

    public class ProjectLogOverview
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string LogText { get; set; }

        public string ResponsibleUser { get; set; }

        public DateTime ChangedDate { get; set; }
    }
}
