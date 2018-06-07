namespace DH.Helpdesk.Domain
{
    using global::System;

    public class ContractLog : Entity
    {
        public int? Case_Id { get; set; }
        public int Contract_Id { get; set; }
        public int LogType { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual Case Case { get; set; }
    }
}
