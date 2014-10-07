namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers;
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
                                .GetByCustomer(customerId)
                                .MapToOverviews();

                return overviews;
            }
        }

        public ManufacturerData GetManufacturerData(int? manufacturerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var manufacturerRepository = uow.GetRepository<Manufacturer>();

                ManufacturerModel manufacturer = null;
                if (manufacturerId.HasValue)
                {
                    manufacturer = manufacturerRepository.GetAll().MapToBusinessModel(manufacturerId.Value);
                }

                return new ManufacturerData(manufacturer);
            }
        }

        public ManufacturerModel GetById(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var manufacturerRepository = uow.GetRepository<Manufacturer>();

                return manufacturerRepository.GetAll().MapToBusinessModel(id);
            }
        }

        public int AddOrUpdate(ManufacturerModel manufacturer)
        {
            throw new global::System.NotImplementedException();
        }
    }
}