namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NewProjectFile : IBusinessModelWithId
    {
        public NewProjectFile(int projectId, byte[] content, string name, DateTime createdDate)
        {
            this.Content = content;
            this.Name = name;
            this.CreatedDate = createdDate;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int ProjectId { get; private set; }

        [NotNullAndEmptyArray]
        public byte[] Content { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}