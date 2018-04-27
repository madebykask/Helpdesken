
namespace DH.Helpdesk.Domain
{
    public class GlobalSetting
    {
        public int Id { get; set; }
        public int DBType { get; set; }
        public int DefaultLanguage_Id { get; set; }
        public int FullTextSearch { get; set; }
        public int GlobalStartPage { get; set; }
        public int LockCaseToWorkingGroup { get; set; }
        public int LoginOption { get; set; }
        public int OrderNumber { get; set; }
        public int PDFPrint { get; set; }
        public int ServerPort { get; set; }
        public string ApplicationName { get; set; }
        public string AttachedFileFolder { get; set; }
        public string VirtualFileFolder { get; set; }
        public string DBVersion { get; set; }
        public string HelpdeskDBVersion { get; set; }
        public string EMailBodyEncoding { get; set; }
        public string PDFPrintPassword { get; set; }
        public string PDFPrintUserName { get; set; }
        public string ServerName { get; set; }
        public string SMTPServer { get; set; }
        public string ExternalSite { get; set; }
        public int CaseLockTimer { get; set; }
        public int CaseLockBufferTime { get; set; }
        public int CaseLockExtendTime { get; set; }
		public string InvoiceFileFolder { get; set; }
        public string ExtendedCasePath { get; set; }
        public int MultiCustomersSearch { get; set; }
		public bool PerformanceLogActive { get; set; }
		public int PerformanceLogFrequency { get; set; }
		public int PerformanceLogSettingsCache { get; set; }
		public virtual Language DefaultLanguage { get; set; }
    }
}
