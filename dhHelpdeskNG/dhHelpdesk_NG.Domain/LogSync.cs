namespace DH.Helpdesk.Domain
{
    using global::System;

    public class LogSync : Entity
    {
        public int Customer_Id { get; set; }
        public int LogType { get; set; }
        public string LogText { get; set; }
        public DateTime LogDate { get; set; }
    }
}
