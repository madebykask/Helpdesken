using System;

namespace dhHelpdesk_NG.DTO.DTOs.Case
{
    public class CaseLog
    {
        public int Id { get; set; }
        public decimal EquipmentPrice { get; set; }
        public int Price { get; set; }
        public int CaseId { get; set; }
        public int Charge { get; set; }
        public int FinishingType { get; set; }
        public int InformCustomer { get; set; }
        public int UserId { get; set; }
        public int WorkingTimeHour { get; set; }
        public int WorkingTimeMinute { get; set; }
        public string RegUser { get; set; }
        public string TextExternal { get; set; }
        public string TextInternal { get; set; }
        public DateTime? FinishingDate { get; set; }
        public DateTime LogDate { get; set; }
    }
}

