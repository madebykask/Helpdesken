namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Computers;

    using global::System;
    using global::System.Collections.Generic;

    public class OU : Entity
    {
        public int ADSync { get; set; }
        public int? Department_Id { get; set; }
        public int IsActive { get; set; }
        public int? Parent_OU_Id { get; set; }
        public int Show { get; set; }
        public string HomeDirectory { get; set; }
        public string Name { get; set; }
        public string OUId { get; set; }
        public string Path { get; set; }
        public string ScriptPath { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Code { get; set; }
        public string SearchKey { get; set; }
        public bool ShowInvoice { get; set; }

        //public Guid? OUGUID { get; set; }
        //public DateTime? SynchronizedDate { get; set; }

        public virtual Department Department { get; set; }
        public virtual OU Parent { get; set; }
        public virtual ICollection<ComputerUserGroup> ComputerUserGroups { get; set; }
        public virtual ICollection<OU> SubOUs { get; set; }
        public virtual ICollection<Order> Orders { get; set; } 
    }
}
