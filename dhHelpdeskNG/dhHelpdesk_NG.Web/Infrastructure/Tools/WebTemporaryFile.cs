namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class WebTemporaryFile
    {
        public WebTemporaryFile(byte[] content, string name)
        {
            this.Name = name;
            this.Content = content;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }
        
        [NotNullAndEmptyArray]
        public byte[] Content { get; private set; }
    }
}