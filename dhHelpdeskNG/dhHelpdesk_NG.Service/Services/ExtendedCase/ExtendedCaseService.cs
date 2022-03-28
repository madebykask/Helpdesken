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
            //sectionLst.AddRange(GetInitiatorSectionsData(payload));
            sectionLst.Add(new SectionElement() { id = "InitiatorSectionData", controls = new List<ControlElement>() });

            var formName = "Formulär";
            var entity = new ExtendedCaseFormJsonModel()
            {
                id = payload.Id,
                name = formName,
                description = payload.Description,
                status = payload.Status,
                customerId = payload.CustomerId,
                customerGuid = payload.CustomerGuid,
                languageId = payload.LanguageId,
                caseSolutionIds = payload.CaseSolutionIds,
                localization = new LocalizationElement()
                { dateFormat = "YYYY-MM-DD", decimalSeparator = "." },
                dataSources = new List<DataSource>() {
                    new DataSource() {
                        type = "query",
                        id = "getInitiatorByName",
                        parameters = new List<DataSourcesParams>() {
                            new DataSourcesParams() { name = "name", field = "tabs.EditorInitiator.sections.HiddenFields.controls.currentUser" },
                            new DataSourcesParams() { name = "customerGuid", field = "tabs.EditorInitiator.sections.HiddenFields.controls.customerGuid" }
                        }
                    }
                    ,
                    new DataSource() {
                        type = "query",
                        id = "OusByDepartmentDs",
                        parameters = new List<DataSourcesParams>() {
                            new DataSourcesParams() { name = "CustomerGuid", field = "tabs.EditorInitiator.sections.HiddenFields.controls.customerGuid" },
                            new DataSourcesParams() { name = "Department_Id", field = "tabs.EditorInitiator.sections.InitiatorInfo.controls.departmentId" }
                        }
                    }
                },

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

            return _extendedCaseFormRepository.SaveExtendedCaseForm(entity, userId, payload.Translations);
        }

        private IList<SectionElement> GetInitiatorSectionsData(ExtendedCaseFormPayloadModel payload)
        {
            List<SectionElement> sectionLst = new List<SectionElement>();
            sectionLst.Add(new SectionElement
            {
                id = "InitiatorInfo",
                name = "@Translation.Section.InitiatorInfo",
                hiddenBinding = "function(m, log) { return true; }",
                controls = new List<ControlElement>()
                {
                    new ControlElement() {
                        id= "UserId",
                        type= "textbox",
                        label= "User Id",
                        valueBinding= @"function(m, log) { var ds = m.dataSources.getInitiatorByName; if (this.value != """") { return this.value; } else if (ds && ds.length > 0){ var userId = (ds[0].UserId || """"); return userId; } return this.value; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                        caseBinding= "reportedby",
                        caseBindingBehaviour= "newonly"
                    },

                    new ControlElement() {
                        id= "Initiator",
                        type= "textbox",
                        label= "Initiator",
                        valueBinding= @"function(m, log) {var ds = m.dataSources.getInitiatorByName; if (this.value != """") { return this.value; } else if (ds && ds.length > 0) { var flName = (ds[0].FirstName || """") + "" "" + (ds[0].LastName || """"); return flName.trim();} return this.value; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                        caseBinding= "persons_name",
                        caseBindingBehaviour= "newonly"
                    },

                    new ControlElement()
                    {
                        id = "Email",
                        type = "textbox",
                        label = "Email",
                        validators = new ValidatorsElement()
                        {
                            onSave = new List<OnSaveElement>()
                            {
                                new OnSaveElement() { type = "pattern", value = ".+\\@.+\\..+", messageName = "email"}
                            }
                        },
                        valueBinding = @"function(m, log) { var ds = m.dataSources.getInitiatorByName; if (this.value != """") { return this.value; } else if (ds && ds.length > 0) { var email = (ds[0].Email || """"); return email; } return this.value; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                        caseBinding = "persons_email",
                        caseBindingBehaviour = "newonly"
                    },

                    new ControlElement() {
                        id= "Phone",
                        type= "textbox",
                        label= "Phone",
                        valueBinding= @"function(m, log) { var ds = m.dataSources.getInitiatorByName; if (this.value != """") { return this.value; } else if (ds && ds.length > 0) { var phone = (ds[0].Phone || """"); return phone; } return this.value; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                        caseBinding= "persons_phone",
                        caseBindingBehaviour= "newonly"
                    },

                    new ControlElement() {
                        id= "Mobile",
                        type= "textbox",
                        label= "Mobile",
                        valueBinding= @"function(m, log) { var ds = m.dataSources.getInitiatorByName; if (this.value != """") { return this.value; } else if (ds && ds.length > 0) { var cellphone = (ds[0].Mobile || """"); return cellphone; } return this.value; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                        caseBinding= "persons_cellphone",
                        caseBindingBehaviour= "newonly"
                    },

                    new ControlElement() {
                        id= "regionId",
                        type= "dropdown",
                        label= "Region",
                        dataSource = new List<DataSource>()
                        {
                            new DataSource()
                            {
                                type = "option",
                                id = "RegionsByCustomer",
                                parameters = new List<DataSourcesParams>()
                                {
                                    new DataSourcesParams() {
                                        name = "CustomerGuid",
                                        field = "tabs.EditorInitiator.sections.HiddenFields.controls.customerGuid"
                                    }
                                }
                            }
                        },
                        valueBinding= @"function(m, log) { var ds = m.dataSources.getInitiatorByName; if (this.value != """") { return this.value; } else { if (ds && ds.length > 0) { var regionId = ds[0].RegionId; if (regionId !== null) { return ds[0].RegionId; } } } return this.value; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                        caseBinding= "region_id",
                        caseBindingBehaviour= "newonly"
                    },

                    new ControlElement() {
                        id= "departmentId",
                        type= "dropdown",
                        label= "Department",
                        dataSource = new List<DataSource>()
                        {
                            new DataSource()
                            {
                                type = "option",
                                id = "DepartmentsByCustomer",
                                parameters = new List<DataSourcesParams>()
                                {
                                    new DataSourcesParams() {
                                        name = "CustomerGuid",
                                        field = "tabs.EditorInitiator.sections.HiddenFields.controls.customerGuid"
                                    }
                                }
                            }
                        },
                        valueBinding= @"function(m, log) { var ds = m.dataSources.getInitiatorByName; if (this.value != """") { return this.value; } else { if (ds && ds.length > 0) { var departmentId = ds[0].DepartmentId; if (departmentId !== null) { return ds[0].DepartmentId; } } } return this.value; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                        caseBinding= "department_id",
                        caseBindingBehaviour= "newonly"
                    },

                new ControlElement() {
                    id= "organizationalUnit",
                    type= "dropdown",
                    label= "Organizational Unit",
                    dataSource = new List<DataSource>()
                    {
                        new DataSource()
                        {
                            type = "custom",
                            id = "OusByDepartmentDs",
                            valueField = "Id",
                            textField = "OU"
                        }
                    },
                    valueBinding= @"function(m, log) { var ds = m.dataSources.getInitiatorByName; if (this.value != """") { return this.value; } else { if (ds && ds.length > 0) { var orgUnitId = ds[0].OU_Id; if (orgUnitId !== null) { return ds[0].OU_Id; } } } return this.value; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                    caseBinding= "ou_id_1",
                    caseBindingBehaviour= "newonly"
                },

                new ControlElement() {
                    id= "CostCentre",
                    type= "textbox",
                    label= "Cost Centre",
                    valueBinding= @"function(m, log) { var ds = m.dataSources.getInitiatorByName; if (this.value != """") { return this.value; } else if (ds && ds.length > 0) { var costCentre = (ds[0].CostCentre || """"); return costCentre; } return this.value; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                },

                new ControlElement() {
                    id= "CostCentreValue",
                    type= "textbox",
                    hiddenBinding= @"function(m, log) { return true; }",
                    valueBinding= @"function(m, log) { return this.parent.controls.CostCentre.value || """"; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                    caseBinding= "costcentre",
                    caseBindingBehaviour= "newonly"
                },

                new ControlElement() {
                    id= "Placement",
                    type= "textbox",
                    label= "Placement",
                    valueBinding= @"function(m, log) { var ds = m.dataSources.getInitiatorByName; if (this.value != """") { return this.value; } else if (ds && ds.length > 0) { var placement = (ds[0].Place || """"); return placement; } return this.value; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                    caseBinding= "place",
                    caseBindingBehaviour= "newonly"
                },

                new ControlElement() {
                    id= "OrdererCode",
                    type= "textbox",
                    label= "Orderer Code",
                    valueBinding= @"function(m, log) { var ds = m.dataSources.getInitiatorByName; if (this.value != """") { return this.value; } else if (ds && ds.length > 0) { var userCode = (ds[0].UserCode || """"); return userCode; } return this.value; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty),
                    caseBinding= "usercode",
                    caseBindingBehaviour= "newonly"
                },
            }
            }); ;

            sectionLst.Add(new SectionElement()
            {
                id = "HiddenFields",
                name = "Hidden fields",
                hiddenBinding = @"function(m, log) { return true; }",
                controls = new List<ControlElement>()
                    {
                        new ControlElement()
                        {
                            id = "customerGuid",
                            type = "textbox",
                            valueBinding = @"function(m, log) { return """ + payload.CustomerGuid + @"""; }"
                        },

                        new ControlElement()
                        {
                            id = "currentUser",
                            type = "textbox",
                            valueBinding = @"function(m, log) { if (!m.formInfo.currentUser) return """"; if (!m.formInfo.caseId) return m.formInfo.currentUser; }".Replace(@"\r", String.Empty).Replace(@"\t", String.Empty)
                        }
                    }
            }
            );
            return sectionLst;
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
                                    validators = (c.Required || c.RequiredSelfService) ? new ValidatorsElement()
                                    {
                                        onSave = new List<OnSaveElement>()
                                        {
                                    new OnSaveElement()
                                    {
                                        type = c.Required || c.RequiredSelfService ? "required" : "",
                                        enabled = c.Required && c.RequiredSelfService ? null 
                                                    : (c.Required ? @"function(m) { if (m.formInfo.applicationType == ""helpdesk"") return true; }"
                                                    : (c.RequiredSelfService ?  @"function(m) { if (m.formInfo.applicationType == ""selfservice"") return true; }"
                                                    : null))
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
