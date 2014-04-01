namespace DH.Helpdesk.BusinessData.Models.Document.Output
{
    public sealed class DocumentFileOverview
    {
        public byte[] File { get; set; }
        public int Size { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}