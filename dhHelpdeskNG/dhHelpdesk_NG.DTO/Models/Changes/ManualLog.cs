namespace DH.Helpdesk.BusinessData.Models.Changes
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ManualLog : BusinessModel
    {
        #region Constructors and Destructors

        public static ManualLog CreateNew(string text, List<MailAddress> emails, Subtopic subtopic)
        {
            return new ManualLog { Text = text, Emails = emails, Subtopic = subtopic };
        }

        private ManualLog()
        {
        }

        #endregion

        #region Public Properties

        [IsId]
        public int? ChangeEmailLogId { get; internal set; }

        [IsId]
        public int ChangeHistoryId { get; internal set; }

        [IsId]
        public int ChangeId { get; internal set; }

        [IsId]
        public int CreatedByUserId { get; internal set; }

        public DateTime CreatedDateAndTime { get; internal set; }

        [NotNull]
        public List<MailAddress> Emails { get; private set; }

        public Subtopic Subtopic { get; private set; }

        [NotNullAndEmpty]
        public string Text { get; private set; }

        #endregion
    }
}