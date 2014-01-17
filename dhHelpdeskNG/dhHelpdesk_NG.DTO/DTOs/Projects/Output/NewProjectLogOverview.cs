namespace dhHelpdesk_NG.DTO.DTOs.Projects.Output
{
    using System;

    public class NewProjectLogOverview
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string LogText { get; set; }

        public string ResponsibleUser { get; set; }

        public DateTime ChangedDate { get; set; }
    }
}
