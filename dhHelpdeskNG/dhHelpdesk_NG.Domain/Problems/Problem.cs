namespace DH.Helpdesk.Domain.Problems
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;
    using global::System.Collections.Generic;

    public class Problem : Entity, ICustomerEntity, IStartPageEntity, IDatedEntity
    {
        public Problem()
        {
            this.Cases = new List<Case>();
        }

        public int? ChangedByUser_Id { get; set; }
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
        public int ShowOnStartPage { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual User ResponsibleUser { get; set; }
        public virtual IList<Case> Cases { get; set; }
    }
}
