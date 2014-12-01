namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;

    public class Domain : Entity, ICustomerEntity
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
