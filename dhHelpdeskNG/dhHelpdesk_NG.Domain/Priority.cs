namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;
    using MailTemplates;

    public class Priority : Entity
    {
        public int Customer_Id { get; set; }
        public int EMailImportance { get; set; }
        public int InformUser { get; set; }
        public int IsActive { get; set; }
        public int IsDefault { get; set; }
        public int IsEmailDefault { get; set; }
        public int? MailID_Change { get; set; }
        public int SLA { get; set; }
        public int SMSNotification { get; set; }

        /// <summary>
        /// Time in hours to close case with this priority
        /// </summary>
        public int SolutionTime { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string EMailList { get; set; }
        public string FileName { get; set; }
        public string InformUserText { get; set; }
        public string LogText { get; set; }
        public string Name { get; set; }
        public string RelatedField { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? OrderNum { get; set; }
        public Guid PriorityGUID { get; set; }
        
        public virtual Customer Customer { get; set; }
        public virtual ICollection<PriorityImpactUrgency> PriorityImpactUrgencies { get; set; }
        public virtual ICollection<PriorityLanguage> PriorityLanguages { get; set; }
        //public virtual MailTemplateEntity MailTemplate { get; set; }

    }
}
