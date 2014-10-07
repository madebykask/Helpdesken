namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses;
    using DH.Helpdesk.Services.Services.Licenses;

    public class LicensesService : ILicensesService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public LicensesService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public LicenseOverview[] GetLicenses(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseRepository = uow.GetRepository<License>();
                var departmentRepository = uow.GetRepository<Department>();

                var overviews = licenseRepository.GetAll()
                                .GetCustomerLicenses(customerId)
                                .MapToOverviews(departmentRepository.GetAll());                                

                return overviews;
            }
        }

        public LicenseData GetLicenseData(int? licenseId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseRepository = uow.GetRepository<License>();

                LicenseModel license = null;
                if (licenseId.HasValue)
                {
                    license = licenseRepository.GetAll().MapToBusinessModel(licenseId.Value);
                }

                return new LicenseData(license);
            }
        }

        public LicenseModel GetById(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseRepository = uow.GetRepository<License>();

                return licenseRepository.GetAll().MapToBusinessModel(id);
            }
        }

        public int AddOrUpdate(LicenseModel license)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseRepository = uow.GetRepository<License>();
                License entity;
                if (license.IsNew())
                {
                    entity = new License();
                    LicenseMapper.MapToEntity(license, entity);
                    entity.CreatedDate = DateTime.Now;
                    entity.ChangedDate = DateTime.Now;
                    licenseRepository.Add(entity);
                }
                else
                {
                    entity = licenseRepository.GetById(license.Id);
                    LicenseMapper.MapToEntity(license, entity);
                    entity.ChangedDate = DateTime.Now;
                    licenseRepository.Update(entity);
                }

                uow.Save();
                return entity.Id;
            }
        }

        public void Delete(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<License>();

                repository.DeleteById(id);

                uow.Save();
            }
        }
    }
}