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
using DH.Helpdesk.Common.Tools;
using System.Text.RegularExpressions;
using DH.Helpdesk.BusinessData.Models.Language.Output;

namespace DH.Helpdesk.Services.Services.ExtendedCase
{
    public class ExtendedCaseService : IExtendedCaseService
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
            _extendedCaseFormRepository = extendedCaseFormRepository;
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

        public List<ExtendedCaseFormEntity> GetExtendedCaseFormsForCustomer(int customerId)
        {
            var forms = _extendedCaseFormRepository.GetExtendedCaseFormsForCustomer(customerId).ToList();
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

        public List<ExtendedCaseFormSectionTranslationModel> GetExtendedCaseFormSections(int extendedCaseFormId, int languageID)
        {
            return _extendedCaseFormRepository.GetExtendedCaseFormSections(extendedCaseFormId, languageID);
        }

        public int SaveExtendedCaseForm(ExtendedCaseFormPayloadModel payload, string userId)
        {
            List<SectionElement> sectionLst = GetExtendedCaseFormSections(payload);
            var formName = "Formulär"; 
            var entity = new ExtendedCaseFormJsonModel()
            {
                id = payload.Id,
                name = formName,
                description = payload.Description,
                status = payload.Status,
                customerId = payload.CustomerId,
                languageId = payload.LanguageId,
                caseSolutionIds = payload.CaseSolutionIds,
                localization = new LocalizationElement()
                { dateFormat = "YYYY-MM-DD", decimalSeparator = "." },
                validatorsMessages = new ValidatorsMessagesElement()
                { required = "", dateYearFormat = "Correct date format (YYYY)", email = "Specify valid email" },
                styles = "ec-section .col-xs-6:first-child { text-align: left; } ec-section .col-md-6:first-child { text-align: left; } .checkbox label { display: block } .radio label { display: block } ",
                tabs = new List<TabElement>()
                    {
                        new TabElement()
                        {
                            columnCount = payload.Tabs[0].ColumnCount.ToString(),
                            id = StringHelper.GetCleanString(payload.Tabs[0].Name),
                            name = StringHelper.GetCleanString(payload.Tabs[0].Name),
                            sections = sectionLst
                        }
                }

            };

            //foreach(var t in payload.Translations)
            //{
            //    t.Id = _extendedCaseFormRepository.GetExtendedCaseFormTranslation(t);
            //}

            return _extendedCaseFormRepository.SaveExtendedCaseForm(entity, userId, payload.Translations);
        }

        private static List<SectionElement> GetExtendedCaseFormSections(ExtendedCaseFormPayloadModel payload)
        {
            var sectionLst = new List<SectionElement>();
            foreach (var t in payload.Tabs)
            {
                foreach (var s in t.Sections)
                {
                    var section = new SectionElement()
                    {
                        id = StringHelper.GetCleanString(s.Id),
                        name = s.SectionName,
                        controls = new List<ControlElement>()
                    };

                    if (s.Controls != null)
                    {

                        foreach (var c in s.Controls)
                        {
                            section.controls.Add(
                                new ControlElement()
                                {
                                    id = StringHelper.GetCleanString(c.Id),
                                    type = c.Type,
                                    label = c.Label,
                                    valueBinding = c.ValueBinding,
                                    validators = c.Required ? new ValidatorsElement()
                                    {
                                        onSave = new List<OnSaveElement>()
                                        {
                                    new OnSaveElement()
                                    {
                                        type = c.Required ? "required" : ""
                                    }
                                        }
                                    } : null,
                                    addonText = c.AddOnText,
                                    dataSource = c.DataSource != null ? c.DataSource : null
                                });
                        }
                    }
                    sectionLst.Add(section);
                }
            }

            return sectionLst;
        }

        public List<CaseSolution> GetCaseSolutionsWithExtendedCaseForm(ExtendedCaseFormPayloadModel formModel)
        {
            return _extendedCaseFormRepository.GetCaseSolutionsWithExtendedCaseForm(formModel);
        }

        public IList<ExtendedCaseFormEntity> GetExtendedCaseFormsCreatedByEditor(Customer customer, bool showActive)
        {

            IList<ExtendedCaseFormEntity> forms = new List<ExtendedCaseFormEntity>();
            if (showActive)
            {
                forms = _extendedCaseFormRepository.GetExtendedCaseFormsCreatedByEditor(customer).Where(e => e.Status == 1).ToList();
            }

            else
            {
                forms = _extendedCaseFormRepository.GetExtendedCaseFormsCreatedByEditor(customer);
            }
            return forms;
        }

        public ExtendedCaseFormEntity GetExtendedCaseFormById(int extendedCaseId)
        {
            return _extendedCaseFormRepository.GetExtendedCaseFormById(extendedCaseId);
        }

        public bool DeleteExtendedCaseForm(int extendedCaseFormId)
        {
            return _extendedCaseFormRepository.DeleteExtendedCaseForm(extendedCaseFormId);
        }

        public bool ExtendedCaseFormInCases(int extendedCaseFormId)
        {
            return _extendedCaseFormRepository.ExtendedCaseFormInCases(extendedCaseFormId);
        }
    }
}
