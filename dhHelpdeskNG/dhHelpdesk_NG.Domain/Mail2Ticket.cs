namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Domain.Interfaces;
    using DH.Helpdesk.Domain.Problems;

    using global::System;
    using global::System.Collections.Generic;

    public class Mail2Ticket : Entity
    {
        public int? Case_Id { get; set; }
        public int? Log_Id { get; set; }
        public string EMailAddress { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Case Case { get; set; }
       
    }
}
