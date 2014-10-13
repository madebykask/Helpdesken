namespace DH.Helpdesk.Mobile.Areas.Licenses.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Licenses;

    public interface ILicensesModelFactory
    {
        LicensesIndexModel GetIndexModel(LicensesFilterModel filter);

        LicensesContentModel GetContentModel(LicenseOverview[] licenses);
    }
}