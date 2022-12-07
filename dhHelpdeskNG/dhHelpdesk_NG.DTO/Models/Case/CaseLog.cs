using DH.Helpdesk.BusinessData.Models.Invoice;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;
    using System.ComponentModel.DataAnnotations;
    
    public class CaseLog
    {
        public const string ChildCaseMarker = "Underärende";

        public const string ParentCaseMarker = "Huvudärende";

        public const string ParentChildCasesMarker = "Skickat till underrärenden";

        public const string ChildParentCasesMarker = "Skickat till huvudärende";

        public int? CaseHistoryId { get; set; }
        public int CaseId { get; set; }
        public Guid LogGuid { get; set; }
        public bool Charge { get; set; }
        public decimal EquipmentPrice { get; set; }
        public int? FinishingType { get; set; }
        public DateTime? FinishingDate { get; set; }
        public int Id { get; set; }
        public bool SendMailAboutCaseToNotifier { get; set; }
        public bool SendMailAboutCaseToPerformer { get; set; }
        public bool AutoCheckPerformerCheckbox { get; set; }
        public bool AutoCheckNotifyerCheckbox { get; set; }
        public bool SendMailAboutLog { get; set; }
        public DateTime LogDate { get; set; }
        public int LogType { get; set; }
        public int Price { get; set; }
        public string RegUser { get; set; }
        public string FinishingTypeName { get; set; }

        //[StringLength(3000)]
        public string TextExternal { get; set; }

        //[StringLength(3000)]
        public string TextInternal { get; set; }
        
        public string EmailRecepientsInternalLogTo { get; set; }
        public string EmailRecepientsInternalLogCc { get; set; }

        public string EmailRecepientsExternalLog { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int WorkingTime { get; set; }
        public int Overtime { get; set; }
        public bool HighPriority { get; set; }

        //TODO: remove when this business model will not be used as viewmodel
        public int WorkingTimeHour
        {
            get { return CalculateWorkingTimeHour(WorkingTime); }
            set { WorkingTime = value * 60 + WorkingTimeMinute; }
        }

        public int WorkingTimeMinute
        {
            get { return CalculateWorkingTimeMinute(WorkingTime); }
            set { WorkingTime = value + WorkingTimeHour * 60; }
        }

        public int OvertimeHour
        {
            get { return CalculateWorkingTimeHour(Overtime); }
            set { Overtime = value * 60 + OvertimeMinute; }
        }

        public int OvertimeMinute
        {
            get { return CalculateWorkingTimeMinute(Overtime); }
            set { Overtime = value + OvertimeHour * 60; }
        }

        private int CalculateWorkingTimeHour(int workingTime)
        {
            return workingTime >= 60 ? (int)Math.Round((double)(workingTime / 60), 0) : 0;
        }

        private int CalculateWorkingTimeMinute(int workingTime)
        {
            return workingTime >= 60 ? (int)workingTime % 60 : workingTime;
        }

        /// <summary>
        /// checkbox value indicates send log to parent/child cases
        /// </summary>
        public bool? SendLogToParentChildLog { get; set; }
        public InvoiceRow InvoiceRow { get; set; }

        public int? OldLog_Id { get; set; }
    }
}

