namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    using System;

    public sealed class NewProjectFileDto : IBusinessModelWithId
    {
        public NewProjectFileDto(byte[] content, string name, int faqId, DateTime createdDate)
        {
            this.Content = content;
            this.Name = name;
            this.ProjectId = faqId;
            this.CreatedDate = createdDate;
        }

        public byte[] Content { get; private set; }

        public string Name { get; private set; }

        public int ProjectId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int Id { get; set; }
    }
}