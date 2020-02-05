using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.ExtendedCase;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using DH.Helpdesk.Services.BusinessLogic.Cases;
using DH.Helpdesk.Services.Enums;
using OfficeOpenXml.Table.PivotTable;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services.ExtendedCase
{
    public class ExtendedCaseService: IExtendedCaseService
    {
        private readonly IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel> _entityToModelMapper;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IExtendedCaseDataRepository _extendedCaseDataRepository;
        private readonly IExtendedCaseFormRepository _extendedCaseFormRepository;
		private readonly IEntityToBusinessModelMapper<CaseSolution, CaseSolutionOverview> _caseSolutionToModelMapper;

		public ExtendedCaseService(
            IGlobalSettingService globalSettingService,
            IExtendedCaseDataRepository extendedCaseDataRepository,
            IExtendedCaseFormRepository extendedCaseFormRepository, 
            IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel> entityToModelMapper,
			IEntityToBusinessModelMapper<CaseSolution, CaseSolutionOverview> caseSolutionToModelMapper)
        {
            _globalSettingService = globalSettingService;
            _extendedCaseDataRepository = extendedCaseDataRepository;
            _extendedCaseFormRepository  = extendedCaseFormRepository;
            _entityToModelMapper = entityToModelMapper;
			_caseSolutionToModelMapper = caseSolutionToModelMapper;
        }

        public ExtendedCaseDataModel GenerateExtendedFormModel(InitExtendedForm initData, out string lastError)
        {
            lastError = "";
            
            var globalSetting = _globalSettingService.GetGlobalSettings().FirstOrDefault();
            if (globalSetting == null)
            {
                lastError = "Global setting can not be empty!";
                return null;
            }
                            
            if (string.IsNullOrEmpty(globalSetting.ExtendedCasePath))
            {
                lastError = "Target path is not specified!";
                return null;
            }

            ExtendedCaseDataModel extendedCaseData = null;                        
            if (initData.CaseId == 0)
            {
                if (!initData.CaseSolutionId.HasValue || initData.CaseSolutionId.Value == 0)
                {
                    lastError = "Template id must be specified!";
                    return null;
                }               

                var extendedCaseForm = _extendedCaseFormRepository.GetExtendedCaseFormForSolution(initData.CaseSolutionId.Value, initData.CustomerId);
                if (extendedCaseForm != null)
                {
                    extendedCaseData = _extendedCaseDataRepository.CreateTemporaryExtendedCaseData(extendedCaseForm.ExtendedCaseFormId, initData.UserName);                    
                }
            }
            else
            {
                extendedCaseData = _extendedCaseDataRepository.GetExtendedCaseDataByCaseId(initData.CaseId);                
            }

            if (extendedCaseData == null)
            {
                lastError = "Could not find or load extended case form.";
                return null;
            }

            //TODO: After refactoring needs to be changed
            extendedCaseData.FormModel.CaseId = initData.CaseId;            
            extendedCaseData.FormModel.LanguageId = initData.LanguageId;
            extendedCaseData.FormModel.Path = ExpandExtendedCasePath(globalSetting.ExtendedCasePath, extendedCaseData.ExtendedCaseFormId, initData);

            return extendedCaseData;
        }
        
        private string ExpandExtendedCasePath(string path, int formModelId, InitExtendedForm initData)
        {
            var expandedPath =
                path.Replace(ExtendedCasePathTokens.ExtendedCaseFormId, formModelId.ToString())
                    .Replace(ExtendedCasePathTokens.CustomerId, initData.CustomerId.ToString())
                    .Replace(ExtendedCasePathTokens.LanguageId, initData.LanguageId.ToString())
                    .Replace(ExtendedCasePathTokens.UserRole, initData.UserRole)
                    .Replace(ExtendedCasePathTokens.CaseStatus, initData.CaseStateSecondaryId.ToString());

            return expandedPath;
        }

        public IList<string> GetTemplateCaseBindingKeys(int formId)
        {
            var exCaseForm = _extendedCaseFormRepository.GetById(formId);
            
            var templateParser = new ExtendedCaseTemplateParser();
            var caseBindingFieldsMap = templateParser.ExtractCaseBindingFields(exCaseForm.MetaData);
            var keys = caseBindingFieldsMap.Keys.ToList();
            return keys;
        }

        public IDictionary<string, string> GetTemplateCaseBindingValues(int formId, int extendedCaseDataId)
        {
            var exCaseForm = _extendedCaseFormRepository.GetById(formId);
            
            var templateParser = new ExtendedCaseTemplateParser();
            var caseBindingFieldsMap = templateParser.ExtractCaseBindingFields(exCaseForm.MetaData);
            var extendedCaseFieldValues = _extendedCaseDataRepository.GetById(extendedCaseDataId).ExtendedCaseValues;

            var caseBindingValuesMap = new Dictionary<string, string>();
            foreach (var caseBindingKV in caseBindingFieldsMap)
            {
                var field = extendedCaseFieldValues.FirstOrDefault(x => x.FieldId == caseBindingKV.Value);
                if (field != null)
                {
                    caseBindingValuesMap.Add(caseBindingKV.Key, field.Value);
                }
            }

            return caseBindingValuesMap;
        }

        public ExtendedCaseDataModel CopyExtendedCaseToCase(int extendedCaseDataID, int caseID, string userID, int? extendedCaseFormId = null)
        {
            return _extendedCaseDataRepository.CopyExtendedCaseToCase(extendedCaseDataID, caseID, userID, extendedCaseFormId);
        }

        public ExtendedCaseDataModel GetExtendedCaseFromCase(int caseID)
        {
            return _extendedCaseDataRepository.GetExtendedCaseDataByCaseId(caseID);
        }

        public ExtendedCaseDataEntity GetExtendedCaseData(Guid uniqueId)
        {
            return _extendedCaseDataRepository.GetExtendedCaseData(uniqueId);
        }

        public int GetCaseIdByExtendedCaseGuid(Guid uniqueId)
        {
            return _extendedCaseDataRepository.GetCaseIdByExtendedCaseGuid(uniqueId);
        }

		public List<ExtendedCaseFormModel> GetExtendedCaseFormsForCustomer(int customerId)
		{
			var forms = _extendedCaseFormRepository.GetExtendedCaseFormsForCustomer(customerId)
                .Select(_entityToModelMapper.Map).ToList();
            return forms;
        }

		public List<ExtendedCaseFormWithCaseSolutionsModel> GetExtendedCaseFormsWithCaseSolutionForCustomer(int customerId)
		{

			var forms = _extendedCaseFormRepository.GetExtendedCaseFormsForCustomer(customerId)
				.Select(o => new ExtendedCaseFormWithCaseSolutionsModel
				{
					Id = o.Id,
					Name = o.Name,
					CaseSolutions = o.CaseSolutions.Select(_caseSolutionToModelMapper.Map)
				});
			return forms.ToList();
		}

		public List<ExtendedCaseFormFieldTranslationModel> GetExtendedCaseFormFields(int extendedCaseFormId, int languageID)
		{
			return _extendedCaseFormRepository.GetExtendedCaseFormFields(extendedCaseFormId, languageID);
		}
	}
}
