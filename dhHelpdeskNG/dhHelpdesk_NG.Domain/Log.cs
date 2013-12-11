using System;

namespace dhHelpdesk_NG.Domain
{
    public class Log : Entity
    {
        public decimal EquipmentPrice { get; set; }
        public int Case_Id { get; set; }
        public int CaseHistory_Id { get; set; }
        public int Charge { get; set; }
        public int Export { get; set; }
        public int FinishingType { get; set; }
        public int InformCustomer { get; set; }
        public int LogType { get; set; }
        public int User_Id { get; set; }
        public int WorkingTime { get; set; }
        public string RegUser { get; set; }
        public string Text_External { get; set; }
        public string Text_Internal { get; set; }
        public DateTime ChangeTime { get; set; }
        public DateTime ExportDate { get; set; }
        public DateTime FinishingDate { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime RegTime { get; set; }
        public Guid LogGUID { get; set; }

        public virtual Case Case { get; set; }
        public virtual CaseHistory CaseHistory { get; set; }
        public virtual FinishingCause FinishingCause { get; set; }
        public virtual User User { get; set; }
    }
}
