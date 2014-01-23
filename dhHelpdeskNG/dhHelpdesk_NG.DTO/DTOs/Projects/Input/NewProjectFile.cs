namespace dhHelpdesk_NG.DTO.DTOs.Projects.Input
{
    using System;

    public sealed class NewProjectFile : IBusinessModelWithId
    {
        public NewProjectFile(int projectId, byte[] content, string name, DateTime createdDate)
        {
            this.ProjectId = projectId;
            this.Content = content;
            this.Name = name;
            this.CreatedDate = createdDate;
        }

        public int ProjectId { get; private set; }

        public byte[] Content { get; private set; }

        public string Name { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int Id { get; set; }
    }
}