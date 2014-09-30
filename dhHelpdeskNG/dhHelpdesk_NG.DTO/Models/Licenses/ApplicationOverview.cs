namespace DH.Helpdesk.BusinessData.Models.Licenses
{
    public sealed class ApplicationOverview
    {
        public int ApplicationId { get; set; }

        public string ApplicationName { get; set; }

        public string ProductName { get; set; }

        public int InstallationsNumber { get; set; }
    }
}