namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class EmailLog : BusinessModel
    {
        #region Constructors and Destructors

        private EmailLog()
        {
        }

        #endregion

        #region Public Properties

        public int OrderId { get; private set; }

        public DateTime CreatedDateAndTime { get; private set; }

        public List<MailAddress> Emails { get; private set; }

        [IsId]
        public int HistoryId { get; private set; }

        public int MailId { get; private set; }

        public string MessageId { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static EmailLog CreateNew(
            int orderId,
            int historyId,
            List<MailAddress> emails,
            int mailId,
            string messageId,
            DateTime createdDateAndTime)
        {
            return new EmailLog
            {
                OrderId = orderId,
                HistoryId = historyId,
                Emails = emails,
                MailId = mailId,
                MessageId = messageId,
                CreatedDateAndTime = createdDateAndTime
            };
        }

        #endregion
    }
}