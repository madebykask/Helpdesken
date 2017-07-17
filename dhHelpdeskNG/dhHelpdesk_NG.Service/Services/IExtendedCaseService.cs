namespace DH.Helpdesk.Services.Services
{
    using BusinessData.Models.Case;
    using BusinessData.Models.ExtendedCase;

    public interface IExtendedCaseService
    {
        ExtendedCaseDataModel GenerateExtendedFormModel(InitExtendedForm initData, out string lastError);

		ExtendedCaseDataModel CopyExtendedCaseToCase(int extendedCaseDataID, int caseID, int userID);
		ExtendedCaseDataModel GetExtendedCaseFromCase(int id);
	}
}