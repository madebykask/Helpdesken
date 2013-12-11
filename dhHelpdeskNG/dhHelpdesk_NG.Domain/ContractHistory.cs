using System;

namespace dhHelpdesk_NG.Domain
{
    public class ContractHistory : Entity
    {
        public int Contract_Id { get; set; }
        public int ContractCategory_Id { get; set; }
        public int CreatedByUser_Id { get; set; }
        public int Department_Id { get; set; }
        public int Finished { get; set; }
        public int FollowUpInterval { get; set; }
        public int FollowUpResponsibleUser_Id { get; set; }
        public int ResponsibleUser_Id { get; set; }
        public int Running { get; set; }
        public int Supplier_Id { get; set; }
        public string ContractNumber { get; set; }
        public string Info { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime NoticeDate { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual ContractCategory ContractCategory { get; set; }
        public virtual Department Department { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual User FollowUpResponsibleUser { get; set; }
        public virtual User ResponsibleUser { get; set; }
    }
}
