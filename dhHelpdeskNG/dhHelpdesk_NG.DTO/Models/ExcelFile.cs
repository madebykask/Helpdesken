namespace DH.Helpdesk.BusinessData.Models
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ExcelFile
    {
        public ExcelFile(byte[] content, string name)
        {
            this.Content = content;
            this.Name = name;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [NotNullAndEmptyArray]
        public byte[] Content { get; private set; }
    }
}