using System;

namespace dhHelpdesk_NG.DTO.DTOs
{
    public class LogResults
    {
        public int Id { get; set; }        
        public DateTime LogDate { get; set; }
        public int LogType { get; set; }
        public DateTime RegTime { get; set; }
        public string Text_External { get; set; }
        public string Text_Internal { get; set; }
        public DateTime FinishingDate { get; set; }        
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public int User_Id { get; set; }
        public string RegUser { get; set; }
        public int InformCustomer { get; set; }
        public int FinishingType { get; set; }
        public int WorkingTime { get; set; }
        public decimal EquipmentPrice { get; set; }
        public int LogClass { get; set; }
        public int ShowOnCase { get; set; }
        public int CaseHistory_Id { get; set; }
        public int Case_Id { get; set; }
        
    }
}
