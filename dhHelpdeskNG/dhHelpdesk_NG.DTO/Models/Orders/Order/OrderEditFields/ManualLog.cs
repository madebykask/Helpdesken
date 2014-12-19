namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ManualLog : BusinessModel
    {
        #region Constructors and Destructors

        private ManualLog()
        {
        }

        #endregion

        public static ManualLog CreateNew(string text, List<MailAddress> emails, Subtopic subtopic)
        {
            return new ManualLog { Text = text, Emails = emails, Subtopic = subtopic };
        }

        #region Public Properties

        [IsId]
        public int? OrderEmailLogId { get; internal set; }

        [IsId]
        public int OrderHistoryId { get; internal set; }

        [IsId]
        public int OrderId { get; internal set; }

        [IsId]
        public int CreatedByUserId { get; internal set; }

        public DateTime CreatedDateAndTime { get; internal set; }

        [NotNull]
        public List<MailAddress> Emails { get; private set; }

        public Subtopic Subtopic { get; private set; }

        public string Text { get; private set; }

        #endregion
    }
}