namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Dal.NewInfrastructure;
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
            throw new System.NotImplementedException();
        }
    }
}