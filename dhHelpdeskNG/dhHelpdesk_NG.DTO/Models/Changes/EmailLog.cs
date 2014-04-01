namespace DH.Helpdesk.BusinessData.Models.Changes
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

        public DateTime CreatedDateAndTime { get; set; }

        public List<MailAddress> Emails { get; set; }

        [IsId]
        public int HistoryId { get; set; }

        public int MailId { get; set; }

        public string MessageId { get; set; }

        #endregion

        #region Public Methods and Operators

        public static EmailLog CreateNew(
            int historyId,
            List<MailAddress> emails,
            int mailId,
            string messageId,
            DateTime createdDateAndTime)
        {
            return new EmailLog
                   {
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