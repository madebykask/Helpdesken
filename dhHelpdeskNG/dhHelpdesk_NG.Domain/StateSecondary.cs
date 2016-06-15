﻿namespace DH.Helpdesk.Domain
{
    using global::System;
    using DH.Helpdesk.Domain.MailTemplates;

    public class StateSecondary : Entity
    {
        public int Customer_Id { get; set; }
        public int IncludeInCaseStatistics { get; set; }
        public int IsActive { get; set; }
        public int NoMailToNotifier { get; set; }
        public int ResetOnExternalUpdate { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public int IsDefault { get; set; }
        public int? MailTemplate_Id { get; set; }
        public int? ReminderDays { get; set; }
        public int RecalculateWatchDate { get; set; }
        public Guid? StateSecondaryGUID { get; set; }
        
        public virtual Customer Customer { get; set; }
        public virtual WorkingGroupEntity WorkingGroup { get; set; }

        public virtual MailTemplateEntity MailTemplate { get; set; }
    }
}
