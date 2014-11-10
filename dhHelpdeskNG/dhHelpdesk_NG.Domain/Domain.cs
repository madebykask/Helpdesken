namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class Domain : Entity
    {
        public int Customer_Id { get; set; }
        public string Base { get; set; }
        public string Filter { get; set; }
        public string FileFolder { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
