using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Models.Output;
using DH.Helpdesk.Web.Common.Constants.Case;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class CaseSectionsController : BaseApiController
    {
        private readonly ICaseSectionService _caseSectionService;
        private readonly ITranslateCacheService _translateCacheService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;

        public CaseSectionsController(ICaseSectionService caseSectionService,
            ITranslateCacheService translateCacheService,
            ICaseFieldSettingService caseFieldSettingService)
        {
            _caseSectionService = caseSectionService;
            _translateCacheService = translateCacheService;
            _caseFieldSettingService = caseFieldSettingService;
        }

        /// <summary>
        /// Case sections list
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="langId"></param>
        /// <returns></returns> 
        public async Task<IEnumerable<CaseSectionOutputModel>> Get(int cid, int langId)
        {
            var model = new List<CaseSectionOutputModel>();
            var sections = await _caseSectionService.GetCaseSectionsAsync(cid, langId);
            var caseFieldSettings = await _caseFieldSettingService.GetCaseFieldSettingsAsync(cid);
            var allOrderedFields =  SetSectionHeaderOrders(caseFieldSettings).ToArray();
            var orderedFields = new List<Tuple<int, int>>();           
          

            sections.ForEach(section =>
            {
                var sectionModel = new CaseSectionOutputModel()
                {
                    CustomerId = section.CustomerId,
                    Id = section.Id,
                    IsEditCollapsed = section.IsEditCollapsed,
                    IsNewCollapsed = section.IsNewCollapsed,
                    SectionType = (CaseSectionType) section.SectionType
                };
                if (string.IsNullOrWhiteSpace(section.SectionHeader))
                {
                    var defaultName = _caseSectionService.GetDefaultHeaderName((CaseSectionType)section.SectionType) ?? string.Empty;
                    section.SectionHeader = _translateCacheService.GetTextTranslation(defaultName, langId);
                }
                sectionModel.SectionHeader = section.SectionHeader;

                var sectionfields = new List<CaseFieldSetting>();
                var fields = caseFieldSettings.Where(x => section.CaseSectionFields.Contains(x.Id));
                if (fields.Any())
                {
                    var caseSectionFieldIds = fields.Select(sf => sf.Id).ToList();
                    foreach (var f in caseSectionFieldIds)
                    {
                        var order = Array.IndexOf(allOrderedFields, f);
                        orderedFields.Add(new Tuple<int, int>(f, order));
                    }

                    foreach (var f in orderedFields.OrderBy(o => o.Item2))
                    {
                        sectionfields.Add(fields.FirstOrDefault(x => x.Id == f.Item1));
                    }
                }
                else
                {
                    sectionfields = fields.ToList();
                }                                

                sectionModel.CaseSectionFields = sectionfields.Select(f =>
                    {
                        GlobalEnums.TranslationCaseFields name;
                        return Enum.TryParse(f.Name, out name) ? name.MapToCaseFieldsNamesApi() : null;
                    }).Where(name => !string.IsNullOrWhiteSpace(name)).ToList();
                               
                model.Add(sectionModel);
            });

            return model;
        }

        private static IList<int> SetSectionHeaderOrders(IList<CaseFieldSetting> fieldSettings)
        {
            var sectionHeaderFieldsOrder = new List<int>();

            if (fieldSettings.Any())
            {
                //Initiator
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.ReportedBy)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Persons_Name")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Persons_EMail")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Persons_Phone")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Persons_CellPhone")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Region_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Department_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "OU_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.CostCentre)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.Place)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.UserCode)?.Id ?? 0);

                //Regarding
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.IsAbout_ReportedBy)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "IsAbout_Persons_Name")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "IsAbout_Persons_EMail")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "IsAbout_Persons_Phone")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "IsAbout_Persons_CellPhone")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "IsAbout_Region_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "IsAbout_Department_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "IsAbout_OU_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.IsAbout_CostCentre)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.IsAbout_Place)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.IsAbout_UserCode)?.Id ?? 0);

                //ComputerInfo
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.InventoryNumber)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "ComputerType_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.InventoryLocation)?.Id ?? 0);
               
                //CaseInfo
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.CaseNumber)?.Id?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.RegTime)?.Id?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.ChangeTime)?.Id?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "User_Id")?.Id?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.RegistrationSourceCustomer)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "CaseType_Id")?.Id?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "ProductArea_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "System_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Urgency_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Impact_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Category_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Supplier_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.InvoiceNumber)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.ReferenceNumber)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.Caption)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.Description)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.Miscellaneous)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.AgreedDate)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.Available)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.Cost)?.Id ?? 0);

                //CaseManagement
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "WorkingGroup_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "CaseResponsibleUser_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Performer_User_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Priority_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "Status_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == "StateSecondary_Id")?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.Project)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.Problem)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.CausingPart)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.Change)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.PlanDate)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.WatchDate)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.VerifiedDescription)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.SolutionRate)?.Id ?? 0);

                //Communication and Status
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.FinishingDescription)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.FinishingDate)?.Id ?? 0);
                sectionHeaderFieldsOrder.Add(fieldSettings.FirstOrDefault(f => f.Name == CaseFieldsNamesApi.ClosingReason)?.Id ?? 0);
            }
            return sectionHeaderFieldsOrder;
        }
    }
}