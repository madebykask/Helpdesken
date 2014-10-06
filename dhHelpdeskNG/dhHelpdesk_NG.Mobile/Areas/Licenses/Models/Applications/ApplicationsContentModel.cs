namespace DH.Helpdesk.Web.Areas.Licenses.Models.Applications
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;

    public sealed class ApplicationsContentModel
    {
        public ApplicationsContentModel(ApplicationOverview[] applications)
        {
            this.Applications = applications;
        }

        public ApplicationOverview[] Applications { get; private set; }
    }
}