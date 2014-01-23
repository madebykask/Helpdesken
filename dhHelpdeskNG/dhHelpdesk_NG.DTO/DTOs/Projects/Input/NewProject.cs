namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class NewProject : IBusinessModelWithId
    {
        public NewProject(string name, int customerId, int? projectManager, int isActive, string description, DateTime? finishDate, DateTime createdDate)
        {
            this.Name = name;
            this.CustomerId = customerId;
            this.ProjectManagerId = projectManager;
            this.IsActive = isActive;
            this.Description = description;
            this.FinishDate = finishDate;
            this.CreatedDate = createdDate;
        }

        public int Id { get; set; }

        [NotNullAndEmpty]
        public string Name { get; set; }

        [IsId]
        public int CustomerId { get; set; }

        [IsId]
        public int? ProjectManagerId { get; set; }

        [MinValue(0)]
        [MaxValue(1)]
        public int IsActive { get; set; }

        public string Description { get; set; }

        public DateTime? FinishDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}