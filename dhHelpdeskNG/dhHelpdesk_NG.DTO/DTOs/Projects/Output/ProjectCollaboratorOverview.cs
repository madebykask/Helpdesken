namespace dhHelpdesk_NG.DTO.DTOs.Projects.Output
{
    public class ProjectCollaboratorOverview
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}
