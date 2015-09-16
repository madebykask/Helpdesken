namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;
    using System.ComponentModel.DataAnnotations;
    
    public class CaseLog
    {
        public const string ChildCaseMarker = "Underärende";

        public const string ParentCaseMarker = "Huvudärende";

        public const string ParentChildCasesMarker = "Skickat till underrärenden";
        public int? CaseHistoryId { get; set; }
        public int CaseId { get; set; }
        public Guid LogGuid { get; set; }
        public bool Charge { get; set; }
        public decimal EquipmentPrice { get; set; }
        public int? FinishingType { get; set; }
        public DateTime? FinishingDate { get; set; }
        public int Id { get; set; }
        public bool SendMailAboutCaseToNotifier { get; set; }
        public bool SendMailAboutLog { get; set; }
        public DateTime LogDate { get; set; }
        public int LogType { get; set; }
        public int Price { get; set; }
        public string RegUser { get; set; }
        public string FinishingTypeName { get; set; }

        [StringLength(3000)]
        public string TextExternal { get; set; }

        [StringLength(3000)]
        public string TextInternal { get; set; }
        
        public string EmailRecepientsInternalLog { get; set; }
        public string EmailRecepientsExternalLog { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int WorkingTimeHour { get; set; }
        public int WorkingTimeMinute { get; set; }
        public bool HighPriority { get; set; }

        /// <summary>
        /// checkbox value indicates send log to parent/child cases
        /// </summary>
        public bool? SendLogToParentChildLog { get; set; }
    }
}

