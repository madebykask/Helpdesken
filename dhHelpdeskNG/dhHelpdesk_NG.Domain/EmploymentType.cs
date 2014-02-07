namespace DH.Helpdesk.Domain
{
    using global::System;

    public class EmploymentType : Entity
    {
        public int Status { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
