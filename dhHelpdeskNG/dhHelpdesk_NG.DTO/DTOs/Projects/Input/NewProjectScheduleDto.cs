namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class NewProjectScheduleDto : INewEntity
    {
        public NewProjectScheduleDto(int userId, string name, int position, int time, string description, DateTime? starDate, DateTime? finishDate, double? caseNumber)
        {
            this.UserId = userId;
            this.Name = name;
            this.Position = position;
            this.Time = time;
            this.Description = description;
            this.StarDate = starDate;
            this.FinishDate = finishDate;
            this.CaseNumber = caseNumber;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int ProjectId { get; set; }

        [IsId]
        public int UserId { get; set; }

        [NotNullAndEmpty]
        public string Name { get; set; }

        [MinValue(0)]
        [MaxValue(99)]
        public int Position { get; set; }

        [MinValue(0)]
        public int Time { get; set; }

        public string Description { get; set; }

        public DateTime? StarDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public double? CaseNumber { get; set; }
    }
}
