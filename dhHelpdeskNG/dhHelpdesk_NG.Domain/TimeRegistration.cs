namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Projects;

    using global::System;

    public class TimeRegistration : Entity
    {
        public double InternalOutlay { get; set; }
        public double InvoiceTime { get; set; }
        public double OverTime1 { get; set; }
        public double OverTime2 { get; set; }
        public double RegisteredTime { get; set; }
        public double TravelTime { get; set; }
        public int AuthorizeByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public int InternalMileage { get; set; }
        public int InvoiceByUser_id { get; set; }
        public int Mileage { get; set; }
        public int OrderNumber { get; set; }
        public int Project_Id { get; set; }
        public int ProjectSchedule_Id { get; set; }
        public int Status { get; set; }
        public int User_Id { get; set; }
        public string Contact { get; set; }
        public string InternalText { get; set; }
        public string InvoiceText { get; set; }
        public DateTime AuthorizeDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime ReadyDate { get; set; }
        public DateTime RegisteredDate { get; set; }
        public Guid TimeRegistrationGUID { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Project Project { get; set; }
        public virtual ProjectSchedule ProjectSchedule { get; set; }
    }
}
