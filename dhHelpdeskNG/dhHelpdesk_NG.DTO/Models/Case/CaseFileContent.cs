namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseFileContent
    {
        public int Id { get; set; }
        public int CaseNumber { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}