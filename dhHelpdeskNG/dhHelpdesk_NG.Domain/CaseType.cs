namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;
    using global::System.Collections.Generic;

    public class CaseType : Entity, ICustomerEntity, IActiveEntity
    {
        public int AutomaticApproveTime { get; set; }
        public int Customer_Id { get; set; }
        public int? Form_Id { get; set; }
        public int IsActive { get; set; }
        public int IsDefault { get; set; }
        public int IsEMailDefault { get; set; }
        public int ITILProcess { get; set; }
        public int? Parent_CaseType_Id { get; set; }
        public int RequireApproving { get; set; }
        public int Selectable { get; set; }
        public int ShowOnExternalPage { get; set; }
        public int? User_Id { get; set; }
        public string Name { get; set; }
        public string RelatedField { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CaseTypeGUID { get; set; }
        public int ShowOnExtPageCases { get; set; }
        public int? WorkingGroup_Id { get; set; }

        public virtual CaseType ParentCaseType { get; set; }
        public virtual User Administrator { get; set; }
        public virtual WorkingGroupEntity WorkingGroup { get; set; }
        public virtual ICollection<CaseType> SubCaseTypes { get; set; }

        public virtual ICollection<CaseTypeProductArea> CaseTypeProductAreas { get; set; }
    }
}
