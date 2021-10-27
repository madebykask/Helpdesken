using System;

namespace DH.Helpdesk.Dal.Repositories.Cases
{
	using System.Collections.Generic;
	using DH.Helpdesk.Dal.Infrastructure;
	using DH.Helpdesk.Domain.ExtendedCaseEntity;
	using DH.Helpdesk.BusinessData.Models.Case;
	using DH.Helpdesk.Common.Enums;
	using Common.Enums.Cases;
	using DH.Helpdesk.Models.Case;
	using DH.Helpdesk.BusinessData.Models.ExtendedCase;
	using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.Language.Output;

    public interface IExtendedCaseFormRepository : IRepository<ExtendedCaseFormEntity>
	{
		Guid CreateExtendedCaseData(int formId, string userGuid);

		ExtendedCaseDataOverview GetExtendedCaseFormForCase(int caseId, int customerId);
		ExtendedCaseDataOverview GetExtendedCaseFormForSolution(int caseSolutionId, int customerId);

		ExtendedCaseDataOverview GetCaseSectionExtendedCaseFormForCase(int caseId, int customerId);
		ExtendedCaseDataOverview GetCaseSectionExtendedCaseFormForSolution(int caseSolutionId, int customerId, int sectionType);
		List<ExtendedCaseDataOverview> GetExtendedCaseFormsForSections(int caseId, int customerId);

		List<ExtendedCaseFormEntity> GetExtendedCaseFormsForCustomer(int customerId);

		List<ExtendedCaseFormFieldTranslationModel> GetExtendedCaseFormFields(int extendedCaseFormId, int languageID);
		List<ExtendedCaseFormSectionTranslationModel> GetExtendedCaseFormSections(int extendedCaseFormId, int languageID);
		int SaveExtendedCaseForm(ExtendedCaseFormJsonModel entity, string userId, List<ExtendedCaseFormTranslation> translations);
		List<CaseSolution> GetCaseSolutionsWithExtendedCaseForm(ExtendedCaseFormPayloadModel formModel);
		IList<ExtendedCaseFormEntity> GetExtendedCaseFormsCreatedByEditor(Customer customer);
		ExtendedCaseFormEntity GetExtendedCaseFormById(int extendedCaseId);
		bool DeleteExtendedCaseForm(int extendedCaseFormId);
        bool ExtendedCaseFormInCases(int extendedCaseFormId);
    }
}