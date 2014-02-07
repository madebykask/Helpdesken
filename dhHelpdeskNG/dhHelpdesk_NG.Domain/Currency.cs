namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Currency : Entity
    {
        public string Code { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
