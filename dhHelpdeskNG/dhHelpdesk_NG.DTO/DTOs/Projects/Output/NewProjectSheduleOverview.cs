namespace dhHelpdesk_NG.DTO.DTOs.Projects.Output
{
    using System;

    public class NewProjectSheduleOverview
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public int State { get; set; }

        public int Time { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public double? CaseNumber { get; set; }
    }
}
