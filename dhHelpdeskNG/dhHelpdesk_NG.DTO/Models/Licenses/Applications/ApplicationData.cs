namespace DH.Helpdesk.BusinessData.Models.Licenses.Applications
{
    public sealed class ApplicationData
    {
        public ApplicationData(ApplicationModel application)
        {
            this.Application = application;
        }

        public ApplicationModel Application { get; private set; }
    }
}