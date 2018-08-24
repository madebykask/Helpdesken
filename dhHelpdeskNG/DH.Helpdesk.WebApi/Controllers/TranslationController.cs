using System;
using System.Linq;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Common;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/Translation")]
    public class TranslationController : ApiController
    {
        private readonly ILanguageService _languageService;
        private readonly ITextTranslationService _translationService;
        private readonly ICacheService _cacheService;

        private const string TranslationsCacheKey = "__translations_";

        public TranslationController(ILanguageService languageService, ITextTranslationService translationService, ICacheService cacheService)
        {
            _languageService = languageService;
            _translationService = translationService;
            _cacheService = cacheService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Languages")]
        public IHttpActionResult ActiveLanguages()
        {
            var languages = _languageService.GetActiveLanguages();
            return Ok(languages);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{lang}")]
        public IHttpActionResult Get(string lang)
        {
            if (string.IsNullOrEmpty(lang))
                return BadRequest("Language parameter is missing");

            var language = _languageService.GetActiveLanguages().FirstOrDefault(x => x.LanguageId.Equals(lang, StringComparison.OrdinalIgnoreCase));
            if (language == null)
                return NotFound();

            var res = _cacheService.Get($"{TranslationsCacheKey}{lang}", () =>
            {
                var translations = _translationService.GetTextTranslationsFor(language.Id);
                var dic = translations.Distinct(new LambdaEqualityComparer<CustomKeyValue<string, string>>(x => x.Key))
                                      .ToDictionary(x => x.Key, y => !string.IsNullOrEmpty(y.Value)? y.Value : y.Key);
                return dic;
            });
            
            return Ok(res);
        }
    }
}
