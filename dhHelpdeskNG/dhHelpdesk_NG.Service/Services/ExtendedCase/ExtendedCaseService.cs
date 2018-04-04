using System;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.ExtendedCase;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using DH.Helpdesk.Services.Enums;
using OfficeOpenXml.Table.PivotTable;

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
            extendedCaseData.FormModel.Path = ExpandExtendedCasePath(globalSetting.ExtendedCasePath, extendedCaseData.FormModel.Id, initData);

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
    }
}
