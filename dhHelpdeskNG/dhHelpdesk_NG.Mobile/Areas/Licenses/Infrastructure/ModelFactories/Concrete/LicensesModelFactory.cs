namespace DH.Helpdesk.Mobile.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Licenses;

    public sealed class LicensesModelFactory : ILicensesModelFactory
    {
        public LicensesIndexModel GetIndexModel(LicensesFilterModel filter)
        {
            return new LicensesIndexModel();
        }

        public LicensesContentModel GetContentModel(LicenseOverview[] licenses)
        {
            return new LicensesContentModel(licenses);
        }
    }
}