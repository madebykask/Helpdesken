namespace DH.Helpdesk.Domain
{
    using global::System;

    public class DailyReport : Entity
    {
        public int Customer_Id { get; set; }
        public int DailyReportSubject_Id { get; set; }
        public int MailSent { get; set; }
        public int UserId { get; set; }
        public string DailyReportText { get; set; }
        public DateTime ChangeDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual DailyReportSubject DailyReportSubject { get; set; }
        public virtual User User { get; set; }
    }
}
