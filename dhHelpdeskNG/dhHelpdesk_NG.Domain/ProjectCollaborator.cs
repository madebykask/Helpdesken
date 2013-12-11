using System;

namespace dhHelpdesk_NG.Domain
{
    public class ProjectCollaborator : Entity
    {
        public int Project_Id { get; set; }
        public int User_Id { get; set; }

        public virtual Project Project { get; set; }
    }
}
