namespace DH.Helpdesk.Domain
{
    using global::System;

    public class FAQFile : Entity
    {
        public int FAQ_Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual FAQ FAQ { get; set; }
    }
}
