namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    public class ProjectCollaboratorDto : INewEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProjectId { get; set; }
    }
}
