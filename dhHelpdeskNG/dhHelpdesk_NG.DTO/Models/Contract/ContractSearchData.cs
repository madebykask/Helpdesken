using System;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Contract
{
    public class ContractSearchItemData
    {
        public int Id { get; set; }
        public Guid ContractGUID { get; set; }
        public string ContractNumber { get; set; }
        public int Finished { get; set; }
        public int NoticeTime { get; set; }
        public DateTime? NoticeDate { get; set; }

        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }

        public int Running { get; set; }
        public int FollowUpInterval { get; set; }
        public string Info { get; set; }

        public SupplierOverview Supplier { get; set; }
        public ContractCategoryOverview ContractCategory { get; set; }
        public DepartmentOverview Department { get; set; }
        public UserOverview ResponsibleUser { get; set; }
        public UserOverview FollowUpResponsibleUser { get; set; }

        public List<ContractCaseData> Cases { get; set; }
    }

    public class ContractCaseData
    {
        public int LogId { get; set; }
        public DateTime LogCreatedDate { get; set; }

        public int? CaseId { get; set; }
        public Decimal CaseNumber { get; set; }
        public DateTime? CaseFinishingDate { get; set; }
        public DateTime? CaseApprovedDate { get; set; }
        public int CaseTypeRequireApproving { get; set; }
    }

    public class DepartmentOverview
    {
        public int Id { get; set; }
        public string Name { get; set; } // DepartmentName
    }

    public class SupplierOverview
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ContractCategoryOverview
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserOverview
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
    }
}