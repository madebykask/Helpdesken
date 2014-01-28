namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class NewChangeFile : IBusinessModelWithId
    {
        public NewChangeFile(string name, byte[] content, int changeId, Subtopic subtopic, DateTime createdDate)
        {
            this.Name = name;
            this.Content = content;
            this.ChangeId = changeId;
            this.Subtopic = subtopic;
            this.CreatedDate = createdDate;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [NotNullAndEmptyArray]
        public byte[] Content { get; private set; }

        [IsId]
        public int ChangeId { get; private set; }

        [NotNullAndEmpty]
        public Subtopic Subtopic { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int Id { get; set; }
    }
}
