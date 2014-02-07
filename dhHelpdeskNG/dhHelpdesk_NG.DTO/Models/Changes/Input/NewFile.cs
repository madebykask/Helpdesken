namespace DH.Helpdesk.BusinessData.Models.Changes.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewFile : IBusinessModelWithId
    {
        public NewFile(string name, byte[] content, int changeId, Subtopic subtopic, DateTime createdDate) : 
            this(name, content, subtopic, createdDate)
        {
            this.ChangeId = changeId;
        }

        public NewFile(string name, byte[] content, Subtopic subtopic, DateTime createdDate)
        {
            this.Name = name;
            this.Content = content;
            this.Subtopic = subtopic;
            this.CreatedDate = createdDate;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [NotNullAndEmptyArray]
        public byte[] Content { get; private set; }

        [IsId]
        public int ChangeId { get; set; }

        public Subtopic Subtopic { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int Id { get; set; }
    }
}
