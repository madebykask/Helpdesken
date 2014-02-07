namespace DH.Helpdesk.BusinessData.Models.Changes.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Changes;

    public sealed class NewLog : IBusinessModelWithId
    {
        public NewLog(
            int changeId,
            Subtopic subtopic,
            DateTime createdDate,
            int registeredByUserId,
            string text)
        {
            this.ChangeId = changeId;
            this.Subtopic = subtopic;
            this.Text = text;
            this.RegisteredByUserId = registeredByUserId;
            this.CreatedDate = createdDate;
        }

        public int Id { get; set; }

        public int ChangeId { get; private set; }

        public Subtopic Subtopic { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int RegisteredByUserId { get; private set; }

        public string Text { get; private set; }
    }
}
