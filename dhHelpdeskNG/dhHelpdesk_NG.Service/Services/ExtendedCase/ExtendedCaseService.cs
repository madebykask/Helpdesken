using System;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.ExtendedCase;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Domain.ExtendedCaseEntity;

namespace DH.Helpdesk.Services.Services.ExtendedCase
{
    public class ExtendedCaseService: IExtendedCaseService
    {
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IExtendedCaseDataRepository _extendedCaseDataRepository;
        private readonly IExtendedCaseFormRepository _extendedCaseFormRepository;

        public ExtendedCaseService(
            ICaseSolutionService caseSolutionService,
            IGlobalSettingService globalSettingService,
            IExtendedCaseDataRepository extendedCaseDataRepository,
            IExtendedCaseFormRepository extendedCaseFormRepository)
        {
            _caseSolutionService = caseSolutionService;
            _globalSettingService = globalSettingService;
            _extendedCaseDataRepository = extendedCaseDataRepository;
            _extendedCaseFormRepository  = extendedCaseFormRepository;
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

                var extendedCaseForm = _extendedCaseFormRepository.GetExtendedCaseFormForCaseSolution(initData.CaseSolutionId.Value);
                if (extendedCaseForm != null)
                {
                    extendedCaseData = _extendedCaseDataRepository.CreateTemporaryExtendedCaseData(extendedCaseForm.Id, initData.UserName);                    
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
            extendedCaseData.FormModel.Path = globalSetting.ExtendedCasePath.Replace("[ExtendedCaseFormId]", extendedCaseData.FormModel.Id.ToString());

            return extendedCaseData;
        }

		public ExtendedCaseDataModel CopyExtendedCaseToCase(int extendedCaseDataID, int caseID, string userID)
		{
			return _extendedCaseDataRepository.CopyExtendedCaseToCase(extendedCaseDataID, caseID, userID);
		}

		public ExtendedCaseDataModel GetExtendedCaseFromCase(int caseID)
		{
			return _extendedCaseDataRepository.GetExtendedCaseDataByCaseId(caseID);
		}
	}


}
