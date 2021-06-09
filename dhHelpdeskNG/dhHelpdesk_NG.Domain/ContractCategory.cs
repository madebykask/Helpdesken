namespace DH.Helpdesk.Domain
{

    using global::System;
    using global::System.Collections.Generic;

    public class ContractCategory : Entity
    {
        public ContractCategory()
        {
            Users = new List<User>();
        }
        public int? CaseType_Id { get; set; }
        public int Customer_Id { get; set; }
        public int? StateSecondary_Id1 { get; set; }
        public int? StateSecondary_Id2 { get; set; }
        public string Case_UserId { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual CaseType CreateCase_CaseType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual StateSecondary StateSecondary { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}