namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Vendors;
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
                                .GetByCustomer(customerId)
                                .MapToOverviews();

                return overviews;
            }
        }

        public VendorData GetVendorData(int? vendorId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var vendorRepository = uow.GetRepository<Vendor>();

                VendorModel vendor = null;
                if (vendorId.HasValue)
                {
                    vendor = vendorRepository.GetAll().MapToBusinessModel(vendorId.Value);
                }

                return new VendorData(vendor);
            }
        }

        public VendorModel GetById(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var vendorRepository = uow.GetRepository<Vendor>();

                return vendorRepository.GetAll().MapToBusinessModel(id);
            }
        }

        public int AddOrUpdate(VendorModel vendor)
        {
            throw new global::System.NotImplementedException();
        }
    }
}