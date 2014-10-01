namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.Services.Licenses;

    public class ManufacturersService : IManufacturersService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ManufacturersService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public ManufacturerOverview[] GetManufacturers(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var manufacturerRepository = uow.GetRepository<Manufacturer>();

                var overviews = manufacturerRepository.GetAll()
                                .GetUsersByCustomer(customerId)
                                .MapToOverviews();

                return overviews;
            }
        }
    }
}