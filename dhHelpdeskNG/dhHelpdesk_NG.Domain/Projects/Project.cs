namespace dhHelpdesk_NG.Domain.Projects
{
    using global::System;

    public class Project : Entity
    {
        public Project()
        {
            this.IsActive = 1;
        }

        public int Customer_Id { get; set; }
        public int? ProjectManager { get; set; }
        public int IsActive { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? FinishDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
