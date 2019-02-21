using System;
using System.Linq;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure.Filters;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/Translation")]
    public class TranslationController : ApiController
    {
        private readonly ILanguageService _languageService;
        private readonly ITranslateCacheService _translationService;

        public TranslationController(ILanguageService languageService, ITranslateCacheService translationService)
        {
            _languageService = languageService;
            _translationService = translationService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("Languages")]
        public IHttpActionResult ActiveLanguages() //TODO: async
        {
            var languages = _languageService.GetActiveLanguages();
            return Ok(languages);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texttype"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoCamelCasingFilter]
        [Route("{texttype}/{lang}")] //ex: api/translation/mobile/en
        public IHttpActionResult Get(string texttype, string lang) //TODO async
        {
            if (string.IsNullOrEmpty(lang))
                return BadRequest("Language parameter is missing");

            var language = _languageService.GetActiveLanguages().FirstOrDefault(x => x.LanguageId.Equals(lang, StringComparison.OrdinalIgnoreCase));
            if (language == null)
                return NotFound();

            //todo: review texttype resolving
            var textTypeEntity = _translationService.GetTextType(texttype);
            var textTypeId = textTypeEntity?.Id ?? 0;

            var translations = _translationService.GetTextTranslationsForLanguage(language.Id, textTypeId);
            var res = translations.Distinct(new LambdaEqualityComparer<CustomKeyValue<string, string>>(x => x.Key))
                                  .ToDictionary(x => x.Key, y => !string.IsNullOrEmpty(y.Value) ? y.Value : y.Key);

            return Ok(res);
        }
    }
}
