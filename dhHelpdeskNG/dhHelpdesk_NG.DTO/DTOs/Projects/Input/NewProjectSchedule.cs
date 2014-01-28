namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class NewProjectSchedule : IBusinessModelWithId
    {
        public NewProjectSchedule(int projectId, int? userId, string name, int position, int time, string description, DateTime? startDate, DateTime? finishDate, decimal? caseNumber, DateTime createdDate)
        {
            this.UserId = userId;
            this.ProjectId = projectId;
            this.Name = name;
            this.Position = position;
            this.Time = time;
            this.Description = description;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.CaseNumber = caseNumber;
            this.CreatedDate = createdDate;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int ProjectId { get; set; }

        [IsId]
        public int? UserId { get; set; }

        [NotNullAndEmpty]
        public string Name { get; set; }

        [MinValue(0)]
        [MaxValue(99)]
        public int Position { get; set; }

        [MinValue(0)]
        [MaxValue(3)]
        public int State { get; set; }

        [MinValue(0)]
        public int Time { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public decimal? CaseNumber { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
