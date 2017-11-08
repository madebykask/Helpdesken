namespace DH.Helpdesk.BusinessData.Models.LogProgram
{
    using System;
    
    public sealed class LogProgram
    {
        public LogProgram()
        {

        }

        public int CaseId { get; set; }
        public int CustomerId { get; set; }
        public int LogType { get; set; }
        public int New_Performer_user_Id { get; set; }
        public int? UserId { get; set; }
        public string LogText { get; set; }
        public string Old_Performer_User_Id { get; set; }
        public DateTime RegTime { get; set; }  
        public string ServerNameIP { get; set; }
        public int? NumberOfUsers { get; set; }

    }
}