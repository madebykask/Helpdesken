using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Services.Services
{
    using BusinessData.Models.Case;
    using BusinessData.Models.ExtendedCase;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public interface IExtendedCaseService
    {
        ExtendedCaseDataModel GenerateExtendedFormModel(InitExtendedForm initData, out string lastError);

        ExtendedCaseDataModel CopyExtendedCaseToCase(int extendedCaseDataID, int caseID, string userID, int? extendedCaseFormId = null);

        ExtendedCaseDataModel GetExtendedCaseFromCase(int id);

        int GetCaseIdByExtendedCaseGuid(Guid uniqueId);

        IList<string> GetTemplateCaseBindingKeys(int formId);
        IDictionary<string, string> GetTemplateCaseBindingValues(int formId, int extendedCaseDataId);

        List<ExtendedCaseFormEntity> GetExtendedCaseFormsForCustomer(int customerId);

        List<ExtendedCaseFormWithCaseSolutionsModel> GetExtendedCaseFormsWithCaseSolutionForCustomer(int customerId);

        List<ExtendedCaseFormFieldTranslationModel> GetExtendedCaseFormFields(int extendedCaseFormId, int languageID);
        List<ExtendedCaseFormSectionTranslationModel> GetExtendedCaseFormSections(int extendedCaseFormId, int languageID);

        int SaveExtendedCaseForm(ExtendedCaseFormPayloadModel entity, string userId);
        List<CaseSolution> GetCaseSolutionsWithExtendedCaseForm(ExtendedCaseFormPayloadModel formModel);
        IList<ExtendedCaseFormEntity> GetExtendedCaseFormsCreatedByEditor(Customer customer, bool showActive);
        ExtendedCaseFormEntity GetExtendedCaseFormById(int extendedCaseId);
        bool DeleteExtendedCaseForm(int extendedCaseFormId);
    }
}