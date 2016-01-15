namespace DH.Helpdesk.BusinessData.Models.Projects.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewProjectFile : INewBusinessModel
    {
        public NewProjectFile(int projectId, byte[] content, string basePath, string name, DateTime createdDate)
        {
            this.ProjectId = projectId;
            this.Content = content;
            this.BasePath = basePath;
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
        public string BasePath { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}