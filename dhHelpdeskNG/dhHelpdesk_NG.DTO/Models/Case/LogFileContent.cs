namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class LogFileContent
    {
        public int Id { get; set; }
        public int LogId { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
		public string Path { get; internal set; }
	}
}