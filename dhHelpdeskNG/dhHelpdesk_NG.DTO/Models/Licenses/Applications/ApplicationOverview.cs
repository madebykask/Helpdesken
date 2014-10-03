namespace DH.Helpdesk.BusinessData.Models.Licenses.Applications
{
    public sealed class ApplicationOverview
    {
        public ApplicationOverview(
                int applicationId, 
                string applicationName, 
                string productName, 
                int installationsNumber)
        {
            this.InstallationsNumber = installationsNumber;
            this.ProductName = productName;
            this.ApplicationName = applicationName;
            this.ApplicationId = applicationId;
        }

        public int ApplicationId { get; private set; }

        public string ApplicationName { get; private set; }

        public string ProductName { get; private set; }

        public int InstallationsNumber { get; private set; }
    }
}