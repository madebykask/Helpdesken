namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class Log : Entity
    {
        public decimal EquipmentPrice { get; set; }
        public int Price { get; set; }
        public int Case_Id { get; set; }
        public int? CaseHistory_Id { get; set; }
        public int Charge { get; set; }
        public int Export { get; set; }
        public int? FinishingType { get; set; }
        public int InformCustomer { get; set; }
        public int LogType { get; set; }
        public int? User_Id { get; set; }
        public int WorkingTime { get; set; }
        public int OverTime { get; set; }
        public string RegUser { get; set; }
        public string Text_External { get; set; }
        public string Text_Internal { get; set; }
        public DateTime ChangeTime { get; set; }
        public DateTime? ExportDate { get; set; }
        public DateTime? FinishingDate { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime RegTime { get; set; }
        public Guid LogGUID { get; set; }
        public int? InvoiceRow_Id { get; set; }

        //public virtual Case Case { get; set; }
        public virtual CaseHistory CaseHistory { get; set; }
        //public virtual FinishingCause FinishingCause { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<LogFile> LogFiles { get; set; }

        public virtual Case Case { get; set; }

        public virtual FinishingCause FinishingTypeEntity { get; set; }
        public virtual InvoiceRow InvoiceRow { get; set; }

    }
}
