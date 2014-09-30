namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Dal.NewInfrastructure;
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
            throw new System.NotImplementedException();
        }
    }
}