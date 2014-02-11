namespace DH.Helpdesk.Domain
{
    using global::System.Collections.Generic;
    using global::System;

    public class LinkGroup : Entity
    {
        public string LinkGroupName { get; set; }
        public int Customer_Id { get; set; }


        public Customer Customer { get; set; }
    }
}
