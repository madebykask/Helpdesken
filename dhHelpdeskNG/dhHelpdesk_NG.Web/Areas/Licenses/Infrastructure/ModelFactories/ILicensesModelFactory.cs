namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories
{
	using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
	using DH.Helpdesk.Web.Areas.Licenses.Models.Licenses;
	using System.Collections.Generic;

	public interface ILicensesModelFactory
    {
        LicensesIndexModel GetIndexModel(LicensesFilterModel filter);

        LicensesContentModel GetContentModel(LicenseOverview[] licenses);

        LicenseEditModel GetEditModel(LicenseData data, List<string> fileUploadWhiteList);

        LicenseModel GetBusinessModel(LicenseEditModel editModel);
    }
}