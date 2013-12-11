using System;

namespace dhHelpdesk_NG.Domain
{
    public class ProjectFile : Entity
    {
        public int Project_Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Project Project { get; set; }
    }
}
