namespace DH.Helpdesk.BusinessData.Models.Changes
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewLog
    {
        #region Constructors and Destructors

        public NewLog(Subtopic subtopic, string text, List<string> emails)
        {
            this.Subtopic = subtopic;
            this.Text = text;
            this.Emails = emails;
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
        public List<string> Emails { get; private set; }

        public Subtopic Subtopic { get; private set; }

        [NotNullAndEmpty]
        public string Text { get; private set; }

        #endregion
    }
}