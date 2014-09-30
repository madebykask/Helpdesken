namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Applications;

    public sealed class ApplicationsModelFactory : IApplicationsModelFactory
    {
        public ApplicationsIndexModel GetIndexModel()
        {
            return new ApplicationsIndexModel();
        }
    }
}