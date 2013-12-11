using System;

namespace dhHelpdesk_NG.Domain
{
    public class Problem : Entity
    {
        public int ChangedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public int ProblemNumber { get; set; }
        public int? ResponsibleUser_Id { get; set; }
        public string Description { get; set; }
        public string InventoryNumber { get; set; }
        public string Name { get; set; }
        public string ProblemNumberPrefix { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? FinishingDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual User ResponsibleUser { get; set; }
    }
}
