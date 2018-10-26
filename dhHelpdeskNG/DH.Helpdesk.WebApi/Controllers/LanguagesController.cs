using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class LanguagesController : BaseApiController
    {
        private readonly ICacheService _cacheService;
        private readonly ILanguageService _languageService;
        private readonly ITranslateCacheService _translateCacheService;

        public LanguagesController(ICacheService cacheService, ILanguageService languageService, ITranslateCacheService translateCacheService)
        {
            _cacheService = cacheService;
            _languageService = languageService;
            _translateCacheService = translateCacheService;
        }

        /// <summary>
        /// List of available languages.
        /// </summary>
        /// <param name="langId"></param>
        /// <returns></returns>
        [HttpGet]
        [SkipCustomerAuthorization]
        // GET api/<controller>
        public async Task<IList<ItemOverview>> Get(int langId)
        {
            var cacheKey = "Languages_true";//TODO: move to LanguagesService.
            var languages = await _cacheService.GetAsync(cacheKey, () => _languageService.GetLanguagesAsync(true), DateTime.UtcNow.AddMinutes(5));
            //Translation.Get todo: translate language names
            return languages.Select(l => new ItemOverview(l.Name, l.Id.ToString())).ToList();
        }

    }
}