namespace DH.Helpdesk.Domain.Projects
{
    public class ProjectCollaborator : Entity
    {
        public int Project_Id { get; set; }
        public int User_Id { get; set; }

        public virtual Project Project { get; set; }

        public virtual User User { get; set; }
    }
}
