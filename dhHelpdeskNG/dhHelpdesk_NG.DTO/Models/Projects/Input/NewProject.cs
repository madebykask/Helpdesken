namespace DH.Helpdesk.BusinessData.Models.Projects.Input
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class NewProject : IBusinessModelWithId
    {
        public NewProject(string name, int customerId, int? projectManager, int isActive, string description, DateTime? startDate, DateTime? endDate, DateTime createdDate)
        {
            this.Name = name;
            this.CustomerId = customerId;
            this.ProjectManagerId = projectManager;
            this.IsActive = isActive;
            this.Description = description;
            this.StartDate = startDate;
            this.EndDate = endDate;
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

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}