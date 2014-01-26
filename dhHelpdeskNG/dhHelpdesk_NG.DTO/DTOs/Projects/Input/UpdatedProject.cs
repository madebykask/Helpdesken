namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class UpdatedProject : IBusinessModelWithId
    {
        public UpdatedProject(int id, string name, int? projectManagerId, int isActive, string description, DateTime? endDate, DateTime changeDate)
        {
            this.Id = id;
            this.Name = name;
            this.ProjectManagerId = projectManagerId;
            this.IsActive = isActive;
            this.Description = description;
            this.EndDate = endDate;
            this.ChangeDate = changeDate;
        }

        public int Id { get; set; }

        [NotNullAndEmpty]
        public string Name { get; set; }

        [IsId]
        public int? ProjectManagerId { get; set; }

        [MinValue(0)]
        [MaxValue(1)]
        public int IsActive { get; set; }

        public string Description { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}