namespace dhHelpdesk_NG.Web.Infrastructure.Tools
{
    using System;

    public sealed class WebTemporaryFile
    {
        public WebTemporaryFile(byte[] content, string name)
        {
            if (content == null || content.Length == 0)
            {
                throw new ArgumentNullException("content", "Value cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Value cannot be null or empty.");
            }

            this.Name = name;
            this.Content = content;
        }

        public string Name { get; private set; }

        public byte[] Content { get; private set; }
    }
}