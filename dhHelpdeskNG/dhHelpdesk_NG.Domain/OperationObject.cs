namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Printers;

    using global::System;
    using global::System.Collections.Generic;

    public class OperationObject : Entity
    {
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public int ShowOnStartPage { get; set; }
        public int ShowPI { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual WorkingGroupEntity WorkingGroup { get; set; }
        public virtual ICollection<Printer> Printers { get; set; }
    }
}
