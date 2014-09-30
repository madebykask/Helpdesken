namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Services.Services.Licenses;

    public class ApplicationsService : IApplicationsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ApplicationsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public ApplicationOverview[] GetApplications(int customerId, bool onlyConnected)
        {
            throw new System.NotImplementedException();
        }
    }
}