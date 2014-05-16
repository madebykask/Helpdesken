namespace DH.Helpdesk.BusinessData.Models.Projects.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class NewProjectLog : INewBusinessModel
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
