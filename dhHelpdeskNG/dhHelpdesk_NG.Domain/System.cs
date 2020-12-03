namespace DH.Helpdesk.Domain
{
    using global::System;

    public class System : Entity
    {
        public int Customer_Id { get; set; }
        public int Priority { get; set; }
        public int? Supplier_Id { get; set; }
        public int? SystemOwnerUser_Id { get; set; }
        public int? SystemResponsibleUser_Id { get; set; }
        public int? Urgency_Id { get; set; }
        public int? ViceSystemResponsibleUser_Id { get; set; }
        public string ViceSystemResponsibleUserId { get; set; }
        public int? OS_Id { get; set; }
        public string Info { get; set; }
        public string Owner { get; set; }
        public string SystemAdministratorName { get; set; }
        public string SystemAdministratorPhone { get; set; }
        public string SystemName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DocumentPath { get; set; }
        public int? Domain_Id { get; set; }
        public string ContactName { get; set; }
        public string ContactEMail { get; set; }
        public string ContactPhone { get; set; }
        public string SystemOwnerUserId { get; set; }
        public string SystemResponsibleUserId { get; set; }
        public string Version { get; set; }
        public string License { get; set; }
        public int Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Urgency Urgency { get; set; }
        public virtual User SystemOwnerUser { get; set; }
        public virtual User SystemResponsibleUser { get; set; }
        public virtual User ViceSystemResponsibleUser { get; set; }
        public virtual Domain Domain { get; set; }
    }
}
