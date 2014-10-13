namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using System;

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

        public ManufacturerData GetManufacturerData(int customerId, int? manufacturerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var manufacturerRepository = uow.GetRepository<Manufacturer>();

                ManufacturerModel manufacturer;
                if (manufacturerId.HasValue)
                {
                    manufacturer = manufacturerRepository.GetAll().MapToBusinessModel(manufacturerId.Value);
                }
                else
                {
                    manufacturer = ManufacturerModel.CreateDefault(customerId);
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
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var manufacturerRepository = uow.GetRepository<Manufacturer>();
                Manufacturer entity;
                if (manufacturer.IsNew())
                {
                    entity = new Manufacturer();
                    ManufacturerMapper.MapToEntity(manufacturer, entity);
                    entity.CreatedDate = DateTime.Now;
                    entity.ChangedDate = DateTime.Now;
                    manufacturerRepository.Add(entity);
                }
                else
                {
                    entity = manufacturerRepository.GetById(manufacturer.Id);
                    ManufacturerMapper.MapToEntity(manufacturer, entity);
                    entity.ChangedDate = DateTime.Now;
                    manufacturerRepository.Update(entity);
                }

                uow.Save();
                return entity.Id;
            }
        }

        public void Delete(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<Manufacturer>();

                repository.DeleteById(id);

                uow.Save();
            }
        }
    }
}