namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Faq;
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;
    using global::System.Collections.Generic;

    public class WorkingGroupEntity : Entity, ICustomerEntity, IActiveEntity
    {
        public WorkingGroupEntity()
        {
            this.Calendars = new List<Calendar>();
        }

        public int IsActive { get; set; }
        public int IsDefault { get; set; }
        public int IsDefaultBulletinBoard { get; set; }
        public int IsDefaultCalendar { get; set; }
        public int IsDefaultOperationLog { get; set; }
        public int AllocateCaseMail { get; set; }
        public int Customer_Id { get; set; }
        public string Code { get; set; }
        public string EMail { get; set; }
        public string WorkingGroupName { get; set; }
        public string POP3Password { get; set; }
        public string POP3UserName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? StateSecondary_Id { get; set; }
        public int? SendExternalEmailToWGUsers { get; set; }
        public Guid? WorkingGroupGUID { get; set; }

        /// <summary>WorkingGroupId is used for communication with Extended Case. This value should be the same in all environments.
        /// </summary>
        public int WorkingGroupId { get; set; } 

        public virtual Customer Customer { get; set; }
        public virtual ICollection<BulletinBoard> BulletinBoards { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Link> Links { get; set; }
        public virtual ICollection<FaqEntity> FAQs { get; set; }
        public virtual ICollection<Calendar> Calendars { get; set; }
        public virtual ICollection<UserWorkingGroup> UserWorkingGroups { get; set; }
        public virtual StateSecondary StateSecondary { get; set; }
        public virtual ICollection<OperationLog> OperationLogs { get; set; }
    }
}
