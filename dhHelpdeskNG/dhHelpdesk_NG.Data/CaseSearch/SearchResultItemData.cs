
using System;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Repositories
{
    public class SearchResultItemData
    {
        /*source fields: Id	Casenumber	Place	Customer_Id	Region_Id	OU_Id	UserCode	Department_Id	Persons_Name	Persons_EMail	Persons_Phone	Persons_CellPhone	FinishingDate	FinishingDescription	Caption	Description	Miscellaneous	Status	ExternalTime	RegTime	ReportedBy	ProductArea_Id	InvoiceNumber	Name	DepertmentName	DepartmentId	SearchKey	ReferenceNumber	RegistrationSourceCustomer	IsAbout_ReportedBy	IsAbout_Persons_Name	AgreedDate	Performer_User_Id	User_Id	CaseResponsibleUser_Id	tblProblem_ResponsibleUser_Id	Status_Id	Supplier_Id	StateSecondary_Id	Priority_Id	PriorityName	Priority	OrderNum	SolutionTime	WatchDate	RequireApproving	ApprovedDate	ContactBeforeAction	SMS	Available	Cost	PlanDate	WorkingGroup_Id	ChangeTime	CaseType_Id	RegistrationSource	InventoryNumber	ComputerType_Id	InventoryLocation	Category_Id	SolutionRate	System_Id	Urgency_Id	Impact_Id	Verified	VerifiedDescription	LeadTime	aggregate_Status	aggregate_SubStatus	_temporary_LeadTime	IncludeInCaseStatistics	RowNum*/
        
        //case 
        public int Id  { get; set; }
        public decimal Casenumber { get; set; }
        public int? Department_Id { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public string Persons_Name { get; set; }
        public string Persons_EMail { get; set; }
        public string Caption { get; set; }
        public int Status { get; set; }
        public string Persons_Phone { get; set; }
        public int? Priority_Id { get; set; }
        public int ContactBeforeAction { get; set; }
        public string Available { get; set; }
        public DateTime? FinishingDate { get; set; }
        public int? aggregate_SubStatus { get; set; }
        public string ReportedBy { get; set; }
        public string InventoryNumber { get; set; }
        public DateTime? PlanDate { get; set; }
        public DateTime? WatchDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Miscellaneous { get; set; }
        public int ExternalTime { get; set; }
        public int? ProductArea_Id { get; set; }
        public string Persons_CellPhone { get; set; }
        public string ComputerType_Id { get; set; }
        public string InventoryLocation { get; set; }
        public DateTime RegTime { get; set; }
        public DateTime ChangeTime { get; set; }
        public string InvoiceNumber { get; set; }
        
        public int RegistrationSource { get; set; }
        public int Cost { get; set; }
        public string UserCode { get; set; }
        public int? aggregate_Status { get; set; }
        public int SMS { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? AgreedDate { get; set; }
        public int Verified { get; set; }
        public string VerifiedDescription { get; set; }
        public string SolutionRate { get; set; }
        public string FinishingDescription { get; set; }
        public int LeadTime { get; set; }

        //customer
        public string Customer_Id { get; set; }

        public string Name { get; set; }

        //department
        public string DepertmentName { get; set; }
        public string DepartmentId { get; set; }
        public string SearchKey { get; set; }

        //isabout
        public string IsAbout_ReportedBy { get; set; }
        public string IsAbout_Persons_Name { get; set; }
        //New fields 5.5.0
        public string IsAbout_Persons_CellPhone { get; set; }
        public string IsAbout_CostCentre { get; set; }
        public string IsAbout_UserCode { get; set; }
        public string IsAbout_Persons_Email { get; set; }
        public string IsAbout_Place { get; set; }
        public string IsAbout_Persons_Phone { get; set; }

        //users
        public string Performer_User_Id { get; set; }
        public string User_Id { get; set; }
        public string CaseResponsibleUser_Id { get; set; }
        public string tblProblem_ResponsibleUser_Id { get; set; }

        public string RegistrationSourceCustomer { get; set; }
        public string Region_Id { get; set; }
        public string OU_Id { get; set; }

        public string Status_Id { get; set; }
        public string Supplier_Id { get; set; }
        public string StateSecondary_Id { get; set; }

        public string PriorityName { get; set; }
        public string Priority { get; set; }
        public int? OrderNum { get; set; }
        public int SolutionTime { get; set; }

        public string WorkingGroup_Id { get; set; }

        public string Category_Id { get; set; }
        
        //case type 
        public int CaseType_Id { get; set; }
        public int RequireApproving { get; set; }

        public string System_Id { get; set; }
        public string Urgency_Id { get; set; }
        public string Impact_Id { get; set; }

        public int _temporary_LeadTime { get; set; }
        public int? IncludeInCaseStatistics { get; set; }

        public int RowNum { get; set; }
    }
}