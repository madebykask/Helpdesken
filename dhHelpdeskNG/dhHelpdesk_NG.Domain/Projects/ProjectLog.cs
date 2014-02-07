namespace DH.Helpdesk.Domain.Projects
{
    using global::System;

    public class ProjectLog : Entity
    {
        public int Project_Id { get; set; }
        public int User_Id { get; set; }
        public string LogText { get; set; }
        public DateTime ChangeDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LogDate { get; set; }

        public virtual Project Project { get; set; }

        public virtual User User { get; set; }
    }
}
