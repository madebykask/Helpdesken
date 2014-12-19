namespace DH.Helpdesk.Domain.Accounts
{
    using global::System;
    using global::System.Collections.Generic;

    public class Account : Entity
    {
        public Account()
        {
            this.Programs = new List<Program>();
            this.AccountEMailLogs = new List<AccountEMailLog>();
        }

        public byte[] AccountFile { get; set; }
        public decimal CaseNumber { get; set; }
        public int AccountActivity_Id { get; set; }
        public int? AccountType_Id { get; set; }
        public int? AccountType3 { get; set; }
        public int? AccountType4 { get; set; }
        public int? AccountType5 { get; set; }
        public int ChangedByUser_Id { get; set; }
        public int? CreatedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public int? Department_Id { get; set; }
        public int? Department_Id2 { get; set; }
        public int Deleted { get; set; }
        public int EMailType { get; set; }
        public int EmploymentType { get; set; }
        public int Export { get; set; }
        public int HomeDirectory { get; set; }
        public int? OU_Id { get; set; }
        public int Profile { get; set; }
        public string AccountFileName { get; set; }
        public string AccountFileContentType { get; set; }
        public string AccountType2 { get; set; }
        public string Activity { get; set; }
        public string ContactEMail { get; set; }
        public string ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryPhone { get; set; }
        public string DeliveryPostalAddress { get; set; }
        public string Info { get; set; }
        public string InfoOther { get; set; }
        public string InfoProduct { get; set; }
        public string InfoUser { get; set; }
        public string InventoryNumber { get; set; }
        public string Manager { get; set; }
        public string OrdererEmail { get; set; }
        public string OrdererFirstName { get; set; }
        public string OrdererId { get; set; }
        public string OrdererLastName { get; set; }
        public string OrdererPhone { get; set; }
        public string ReferenceNumber { get; set; }
        public string Responsibility { get; set; }
        public string UserEMail { get; set; }
        public string UserExtension { get; set; }
        public string UserFirstName { get; set; }
        public string UserId { get; set; }
        public string UserInitials { get; set; }
        public string UserLastName { get; set; }
        public string UserLocation { get; set; }
        public string UserPersonalIdentityNumber { get; set; }
        public string UserPhone { get; set; }
        public string UserPostalAddress { get; set; }
        public string UserRoomNumber { get; set; }
        public string UserTitle { get; set; }
        public DateTime? AccountEndDate { get; set; }
        public DateTime? AccountStartDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? FinishingDate { get; set; }

        public virtual AccountActivity AccountActivity { get; set; }
        //public virtual AccountType AccountTypeEntity2 { get; set; }
        //public virtual AccountType AccountTypeEntity3 { get; set; }
        //public virtual AccountType AccountTypeEntity4 { get; set; }

        public virtual AccountType AccountType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Department Department { get; set; }
        public virtual Department Department2 { get; set; }
        public virtual OU OU { get; set; }
        public virtual List<Program> Programs { get; set; }
        public virtual List<AccountEMailLog> AccountEMailLogs { get; set; }
    }
}
