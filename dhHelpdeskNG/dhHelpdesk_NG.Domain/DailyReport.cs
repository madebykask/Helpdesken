namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;

    public class DailyReport : Entity, ICustomerEntity, IDatedEntity
    {
        public int Customer_Id { get; set; }
        public int DailyReportSubject_Id { get; set; }
        public int MailSent { get; set; }
        public int User_Id { get; set; }
        public string DailyReportText { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual DailyReportSubject DailyReportSubject { get; set; }
        public virtual User User { get; set; }
    }
}
