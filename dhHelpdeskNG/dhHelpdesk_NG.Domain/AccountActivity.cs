namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class AccountActivity : Entity
    {
        public int? AccountActivityGroup_Id { get; set; }
        public int? CreateCase_CaseType_Id { get; set; }
        public int? Customer_Id { get; set; }
        public int? Document_Id { get; set; }
        public int SetAccountFinishingDate { get; set; }
        public string AccountInfo { get; set; }
        public string ContactInfo { get; set; }
        public string DeliveryInfo { get; set; }
        public string Description { get; set; }
        public string EMail { get; set; }
        public string Name { get; set; }
        public string OrdererInfo { get; set; }
        public string ProgramInfo { get; set; }
        public string URL { get; set; }
        public string UserInfo { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual AccountActivityGroup AccountActivityGroup { get; set; }
        public virtual Case CreateCase { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Document Document { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
