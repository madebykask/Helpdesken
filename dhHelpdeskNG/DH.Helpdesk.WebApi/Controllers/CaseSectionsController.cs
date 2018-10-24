using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class CaseSectionsController : BaseApiController
    {
        private readonly ICaseSectionService _caseSectionService;
        private readonly ITranslateCacheService _translateCacheService;

        public CaseSectionsController(ICaseSectionService caseSectionService, ITranslateCacheService translateCacheService)
        {
            _caseSectionService = caseSectionService;
            _translateCacheService = translateCacheService;
        }

        // GET api/<controller>
        public async Task<IEnumerable<CaseSectionModel>>
            Get(int cid, int langId) //TODO: return not CaseSectionModel, but only required fields
        {
            var sections = await _caseSectionService.GetCaseSectionsAsync(cid, langId);
            sections.ForEach(section =>
            {
                if (string.IsNullOrWhiteSpace(section.SectionHeader))
                {
                    var defaultName = _caseSectionService.GetDefaultHeaderName((CaseSectionType)section.SectionType) ?? string.Empty;
                    section.SectionHeader = _translateCacheService.GetTextTranslation(defaultName, langId);
                }
            });

            return sections;
        }

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}


    }
}