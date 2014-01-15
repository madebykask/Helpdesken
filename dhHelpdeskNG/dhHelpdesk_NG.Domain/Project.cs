using System;

namespace dhHelpdesk_NG.Domain
{
    public class Project : Entity
    {
        public Project()
        {
            IsActive = 1;
        }

        public int Customer_Id { get; set; }
        public int? ProjectManager { get; set; }
        public int IsActive { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
