namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.Services.Licenses;

    public class VendorsService : IVendorsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public VendorsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public VendorOverview[] GetVendors(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var vendorRepository = uow.GetRepository<Vendor>();

                var overviews = vendorRepository.GetAll()
                                .GetUsersByCustomer(customerId)
                                .MapToOverviews();

                return overviews;
            }
        }
    }
}