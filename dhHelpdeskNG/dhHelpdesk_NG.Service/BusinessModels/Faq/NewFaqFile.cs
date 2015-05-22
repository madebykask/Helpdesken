namespace DH.Helpdesk.Services.BusinessModels.Faq
{
    using System;

    public sealed class NewFaqFile
    {
        public NewFaqFile(byte[] content, string basePath, string name, DateTime createdDate)
        {
            if (content == null || content.Length == 0)
            {
                throw new ArgumentNullException("content", "Value cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Value cannot be null or empty.");
            }

            this.Content = content;
            this.BasePath = basePath;
            this.Name = name;
            this.CreatedDate = createdDate;
        }

        public byte[] Content { get; private set; }

        public string BasePath { get; private set; }

        public string Name { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}
