namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Licenses;

    public interface ILicensesModelFactory
    {
        LicensesIndexModel GetIndexModel(LicensesFilterModel filter);

        LicensesContentModel GetContentModel(LicenseOverview[] licenses);

        LicenseEditModel GetEditModel(LicenseData data);

        LicenseModel GetBusinessModel(LicenseEditModel editModel);
    }
}