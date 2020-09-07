namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Server
{
    public class DocumentFile
    {
        public DocumentFile()
        {
        }

        public DocumentFile(string fileName, byte[] content)
        {
            FileName = fileName;
            Content = content;
        }

        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}