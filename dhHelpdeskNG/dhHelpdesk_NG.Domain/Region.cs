namespace DH.Helpdesk.Domain
{
    using global::System;

    using global::System.Collections.Generic;

    using DH.Helpdesk.Domain.Interfaces;

    public class Region : Entity, ICustomerEntity, INamedEntity, IActiveEntity
    {
        public int Customer_Id { get; set; }

        public int IsActive { get; set; }

        public int IsDefault { get; set; }

        public string Name { get; set; }

        public string SearchKey { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Code { get; set; }

        public Guid RegionGUID { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        //public DateTime? SynchronizedDate { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}
