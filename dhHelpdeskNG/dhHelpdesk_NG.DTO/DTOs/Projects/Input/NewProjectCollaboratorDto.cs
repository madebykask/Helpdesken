namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class NewProjectCollaboratorDto : IBusinessModelWithId
    {
        public NewProjectCollaboratorDto(int userId, int projectId)
        {
            this.UserId = userId;
            this.ProjectId = projectId;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int UserId { get; set; }

        [IsId]
        public int ProjectId { get; set; }
    }
}
