namespace DH.Helpdesk.Domain
{
    using global::System;

    public class InfoText : Entity
    {
        public int? Customer_Id { get; set; }
        public int? Language_Id { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Language Language { get; set; }
    }
}
