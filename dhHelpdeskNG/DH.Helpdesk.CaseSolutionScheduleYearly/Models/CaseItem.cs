using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.CaseSolutionScheduleYearly.Models
{
    [Serializable]
    public class CCase
    {
        public CCase()
        {
            // Sätt standardvärden
            CaseGUID = Guid.NewGuid().ToString();
            ExternalCasenumber = "";
            RegTime = DateTime.UtcNow; // Sätt vid skapande? Eller låt DB göra det?
            ChangeTime = DateTime.UtcNow;
            FinishingDate = DateTime.MinValue; // Använd MinValue för "inte satt"
            WatchDate = DateTime.MinValue;
            PlanDate = DateTime.MinValue;
            AgreedDate = DateTime.MinValue;
            RegistrationSource = 3; // Email default?
            Status = 1; // Aktiv?
            //Logs = new List<Log>(); // Initiera listan
        }

        // Properties från VB-klassens deklarationer och konstruktor
        public int Id { get; set; }
        public string CaseGUID { get; set; }
        public long Casenumber { get; set; } // Från DB efter insert
        public string ExternalCasenumber { get; set; }
        public int Customer_Id { get; set; }
        public int CaseType_Id { get; set; }
        public string CaseTypeName { get; set; } // Från JOIN
        public int ProductArea_Id { get; set; }
        public string ProductAreaName { get; set; } // Från JOIN
        public int Category_Id { get; set; }
        public string CategoryName { get; set; } // Från JOIN
        public int Priority_Id { get; set; }
        public string PriorityName { get; set; } // Från JOIN
        public string PriorityDescription { get; set; } // Från JOIN
        public int Region_Id { get; set; }
        public int Department_Id { get; set; }
        public string Department { get; set; } // Från JOIN
        public int OU_Id { get; set; }
        public string CustomerName { get; set; } // Från JOIN
        public string ReportedBy { get; set; }
        public string Persons_Name { get; set; }
        public string Persons_EMail { get; set; }
        public string Persons_Phone { get; set; }
        public string Persons_CellPhone { get; set; }
        public string Place { get; set; }
        public string UserCode { get; set; }
        public string CostCentre { get; set; }
        public string InvoiceNumber { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Miscellaneous { get; set; }
        public int WorkingGroup_Id { get; set; }
        public string CaseWorkingGroup { get; set; } // Från JOIN
        public string WorkingGroupEMail { get; set; } // Från JOIN (Case WG)
        public int WorkingGroupAllocateCaseMail { get; set; } // Från JOIN (Case WG)
        public int Performer_User_Id { get; set; }
        public string PerformerFirstName { get; set; } // Från JOIN
        public string PerformerSurName { get; set; } // Från JOIN
        public string PerformerEMail { get; set; } // Från JOIN
        public string PerformerPhone { get; set; } // Från JOIN
        public string PerformerCellPhone { get; set; } // Från JOIN
        public int PerformerWorkingGroup_Id { get; set; } // Från JOIN (Performer WG)
        public string PerformerWorkingGroup { get; set; } // Från JOIN (Performer WG)
        public int PerformerWorkingGroupAllocateCaseMail { get; set; } // Från JOIN (Performer WG)
        public string PerformerWorkingGroupEMail { get; set; } // Från JOIN (Performer WG)
        public int RegLanguage_Id { get; set; }
        public DateTime RegTime { get; set; } // Från DB
        public DateTime ChangeTime { get; set; } // Från DB
        public string InventoryNumber { get; set; }
        public DateTime FinishingDate { get; set; }
        public int AutomaticApproveTime { get; set; } // Från CaseType JOIN
        public int ExternalUpdateMail { get; set; } // Från User JOIN
        public int Status_Id { get; set; } // Status (aktivt/stängt etc)
        public int StateSecondary_Id { get; set; }
        public string StateSecondary { get; set; } // Från JOIN
        public int? StateSecondary_FinishingCause_Id { get; set; } // Från JOIN (Nullable)
        public int StateSecondary_ReminderDays { get; set; } // Från JOIN
        public int StateSecondary_AutoCloseDays { get; set; } // Från JOIN
        public int ResetOnExternalUpdate { get; set; } // Från JOIN
        public DateTime WatchDate { get; set; }
        public int RegistrationSource { get; set; } // Default 3
        public int Form_Id { get; set; } // Används i createCase
        public int HolidayHeader_Id { get; set; } // Från Department JOIN
        public int RegistrationSourceCustomer_Id { get; set; }
        // public List<Log> Logs { get; set; } // Ersätter Collection
        public string RegUserName { get; set; }
        public string ChangedName { get; set; } // Från JOIN
        public string ChangedSurName { get; set; } // Från JOIN
        public string Available { get; set; }
        public string ReferenceNumber { get; set; }
        public int IncludeInCaseStatistics { get; set; } // Från StateSecondary JOIN
        public int ExternalTime { get; set; } // Beräknat fält
        public int LeadTime { get; set; } // Beräknat fält
        public int Project_Id { get; set; }
        public int System_Id { get; set; }
        public int Urgency_Id { get; set; }
        public int Impact_Id { get; set; }
        public string VerifiedDescription { get; set; }
        public string SolutionRate { get; set; }
        public string InventoryType { get; set; }
        public string InventoryLocation { get; set; }
        public int Supplier_Id { get; set; }
        public int Cost { get; set; }
        public int OtherCost { get; set; }
        public string Currency { get; set; }
        public int Sms { get; set; }
        public string ContactBeforeAction { get; set; }
        public int Problem_Id { get; set; }
        public int Change_Id { get; set; }
        public string FinishingDescription { get; set; }
        public DateTime PlanDate { get; set; }
        public int CausingPartId { get; set; }
        public int Verified { get; set; }
        public DateTime AgreedDate { get; set; }

        // IsAbout-fält (mappas från tblCaseIsAbout JOIN)
        public string IsAbout_ReportedBy { get; set; }
        public string IsAbout_PersonsName { get; set; }
        public string IsAbout_PersonsEmail { get; set; }
        public string IsAbout_PersonsPhone { get; set; }
        public string IsAbout_PersonsCellPhone { get; set; }
        public int IsAbout_Region_Id { get; set; }
        public int IsAbout_Department_Id { get; set; }
        public int IsAbout_OU_Id { get; set; }
        public string IsAbout_Place { get; set; }
        public string IsAbout_CostCentre { get; set; }
        public string IsAbout_UserCode { get; set; }
        // Slut IsAbout

        public int CaseSolution_Id { get; set; }
        public int? ExtendedCaseFormId { get; set; } // Nullable int
        public int Status { get; set; } // 0=Deleted, 1=Active?
        public int MovedFromCustomer_Id { get; set; }
    }
}
