namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using System.Linq;

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
                /*var productRepository = uow.GetRepository<Product>();

                var query = from l in licenseRepository.GetAll()
                            join p in productRepository.GetAll() on l.Product_Id equals p.Id 
                            join d in departmentRepository.GetAll() on l.Department_Id equals d.Id into gj
                            from ld in gj.DefaultIfEmpty()
                            where p.Customer_Id == customerId
                            select new
                                       {
                                           LicenseId = l.Id,
                                           ProductName = p.Name,
                                           LicensesNumber = l.NumberOfLicenses,
                                           PurchaseDate = l.PurshaseDate,
                                           Department = ld != null ? ld.DepartmentName : null
                                       };

                var overviews = query.ToArray().Select(l => new LicenseOverview(
                                                    l.LicenseId,
                                                    l.ProductName,
                                                    l.LicensesNumber,
                                                    l.PurchaseDate,
                                                    l.Department)).ToArray();*/

                var overviews = licenseRepository.GetAll()
                                .GetCustomerLicenses(customerId)
                                .MapToOverviews(departmentRepository.GetAll());
                                

                return overviews;
            }
        }
    }
}