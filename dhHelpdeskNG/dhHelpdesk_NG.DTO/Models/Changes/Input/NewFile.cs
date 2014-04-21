namespace DH.Helpdesk.BusinessData.Models.Changes.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewFile : INewBusinessModel
    {
        public NewFile(ChangeArea subtopic, byte[] content, string name, DateTime createdDate)
        {
            this.Subtopic = subtopic;
            this.Content = content;
            this.Name = name;
            this.CreatedDate = createdDate;
        }

        public int Id { get; set; }

        public ChangeArea Subtopic { get; private set; }

        [NotNullAndEmptyArray]
        public byte[] Content { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [IsId]
        internal int ChangeId { get; set; }

        public DateTime CreatedDate { get; private set; }
    }
}
