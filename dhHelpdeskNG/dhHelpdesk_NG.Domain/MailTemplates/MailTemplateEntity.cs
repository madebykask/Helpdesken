﻿namespace DH.Helpdesk.Domain.MailTemplates
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;

    public class MailTemplateEntity : Entity, INulableCustomerEntity
    {
        #region Public Properties

        public virtual AccountActivity AccountActivity { get; set; }

        public int? AccountActivity_Id { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public int? Customer_Id { get; set; }

        public int IsStandard { get; set; }

        public int MailID { get; set; }

        public Guid MailTemplateGUID { get; set; }

        public virtual OrderType OrderType { get; set; }

        public int? OrderType_Id { get; set; }

        #endregion
    }
}