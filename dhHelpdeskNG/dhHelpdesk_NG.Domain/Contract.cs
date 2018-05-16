namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class Contract : Entity
    {
        public int ChangedByUser_Id { get; set; }
        public int ContractCategory_Id { get; set; }
        public int? Department_Id { get; set; }
        public int Finished { get; set; }
        public int FollowUpInterval { get; set; }
        public int? FollowUpResponsibleUser_Id { get; set; }
        public int? ResponsibleUser_Id { get; set; }
        public int Running { get; set; }
        public int? Supplier_Id { get; set; }
        public int NoticeTime { get; set; }
        public string ContractNumber { get; set; }
        public string Info { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? NoticeDate { get; set; }
        public Guid ContractGUID { get; set; }

        public virtual ContractCategory ContractCategory { get; set; }
        public virtual Department Department { get; set; }
        public virtual User FollowUpResponsibleUser { get; set; }
        public virtual User ResponsibleUser { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ContractLog> ContractLogs { get; set; }
    }
}
