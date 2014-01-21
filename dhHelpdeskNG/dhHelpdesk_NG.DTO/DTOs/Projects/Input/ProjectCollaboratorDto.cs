namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    public class ProjectCollaboratorDto : IBusinessModelWithId
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProjectId { get; set; }
    }
}
