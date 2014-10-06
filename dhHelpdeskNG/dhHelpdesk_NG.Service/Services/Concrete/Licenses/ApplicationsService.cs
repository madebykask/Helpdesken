namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses;
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
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var applicationRepository = uow.GetRepository<Application>();
                var productRepository = uow.GetRepository<Product>();

                var overviews = applicationRepository.GetAll()
                                .GetOnlyConnectedCustomerApplications(customerId, onlyConnected)
                                .MapToOverviews(productRepository.GetAll());

                return overviews;
            }
        }

        public ApplicationData GetApplicationData(int? applicationId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var applicationRepository = uow.GetRepository<Application>();
 
                ApplicationModel application = null;
                if (applicationId.HasValue)
                {
                    
                }

                return new ApplicationData(application);                
            }
        }
    }
}