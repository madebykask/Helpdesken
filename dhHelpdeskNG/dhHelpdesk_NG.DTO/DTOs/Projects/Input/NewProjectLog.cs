namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class NewProjectLog : IBusinessModelWithId
    {
        public NewProjectLog(int projectId, string logText, int responsibleUserId)
        {
            this.ProjectId = projectId;
            this.LogText = logText;
            this.ResponsibleUserId = responsibleUserId;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int ProjectId { get; set; }

        [NotNullAndEmpty]
        public string LogText { get; set; }

        [IsId]
        public int ResponsibleUserId { get; set; }
    }
}
