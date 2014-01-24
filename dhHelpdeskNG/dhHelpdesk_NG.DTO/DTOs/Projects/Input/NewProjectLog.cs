namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class NewProjectLog : IBusinessModelWithId
    {
        public NewProjectLog(int projectId, string logText, int responsibleUserId, DateTime createdDate)
        {
            this.ProjectId = projectId;
            this.LogText = logText;
            this.ResponsibleUserId = responsibleUserId;
            this.CreatedDate = createdDate;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int ProjectId { get; set; }

        [NotNullAndEmpty]
        public string LogText { get; set; }

        [IsId]
        public int ResponsibleUserId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
