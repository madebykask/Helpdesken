namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class WebTemporaryFile
    {
        public WebTemporaryFile(byte[] content, string name)
        {
            this.Content = content;
            this.Name = name;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }
        
        [NotNull]
        public byte[] Content { get; private set; }
    }
}