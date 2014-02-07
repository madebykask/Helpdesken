namespace DH.Helpdesk.BusinessData.Models.Projects.Input
{
    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class NewProjectCollaborator : INewBusinessModel
    {
        public NewProjectCollaborator(int userId, int projectId)
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
