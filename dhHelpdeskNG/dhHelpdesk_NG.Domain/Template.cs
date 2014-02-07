namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Template
    {
        public int Customer_Id { get; set; }
        public int Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
