using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Web.Common.Extensions;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Models.Output;

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

                var fields = caseFieldSettings.Where(x => section.CaseSectionFields.Contains(x.Id));
                sectionModel.CaseSectionFields = fields.Select(f =>
                    {
                        GlobalEnums.TranslationCaseFields name;
                        return Enum.TryParse(f.Name, out name) ? name.MapToCaseFieldsNamesApi() : null;
                    }).Where(name => !string.IsNullOrWhiteSpace(name)).ToList();
                model.Add(sectionModel);
            });

            return model;
        }
    }
}