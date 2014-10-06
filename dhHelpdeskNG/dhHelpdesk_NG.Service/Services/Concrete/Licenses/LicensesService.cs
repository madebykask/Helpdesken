namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
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
    }
}