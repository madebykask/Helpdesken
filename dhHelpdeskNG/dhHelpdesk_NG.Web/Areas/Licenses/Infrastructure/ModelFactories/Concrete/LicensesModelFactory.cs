namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Licenses;

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

        public LicenseEditModel GetEditModel(LicenseData data)
        {
            return new LicenseEditModel(data);
        }
    }
}