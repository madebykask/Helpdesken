namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Dal.NewInfrastructure;
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
            throw new System.NotImplementedException();
        }
    }
}