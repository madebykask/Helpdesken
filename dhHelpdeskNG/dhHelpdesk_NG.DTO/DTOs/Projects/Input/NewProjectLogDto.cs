namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class NewProjectLogDto : IBusinessModelWithId
    {
        public NewProjectLogDto(string logText, int responsibleUserId)
        {
            this.LogText = logText;
            this.ResponsibleUserId = responsibleUserId;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int ProjectId { get; set; }

        [IsId]
        public int ResponsibleUserId { get; set; }

        [NotNullAndEmpty]
        public string LogText { get; set; }
    }
}
