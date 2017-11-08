namespace DH.Helpdesk.Domain
{
    using global::System;

    public class LogProgramEntity : Entity
    {
        public int Case_Id { get; set; }
        public int Customer_Id { get; set; }
        public int Log_Type { get; set; }
        public int New_Performer_user_Id { get; set; }
        public int? User_Id { get; set; }
        public string LogText { get; set; }
        public string Old_Performer_User_Id { get; set; }
        public DateTime RegTime { get; set; }
        public string ServerNameIP { get; set; }
        public int? NumberOfUsers { get; set; }

        public virtual User User { get; set; }
    }
}
