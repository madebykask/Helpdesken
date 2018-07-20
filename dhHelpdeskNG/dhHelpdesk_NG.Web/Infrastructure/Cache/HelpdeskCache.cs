using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Common;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Web.Infrastructure.Cache
{
    public class HelpdeskCache: IHelpdeskCache
    {
        private string _caseTranslationsKey = "CaseTranslations";
        private string _caseTranslationsFormat = "{0}_{1}";

        private string _textTranslationsKey = "TextTranslations";

        private readonly ICacheService _cacheService;
        private readonly IMasterDataService _masterDataService;

        public HelpdeskCache(ICacheService cacheService, IMasterDataService masterDataService)
        {
            _cacheService = cacheService;
            _masterDataService = masterDataService;
        }

        public IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int customerId)
        {
            return _cacheService.Get(string.Format(_caseTranslationsFormat, _caseTranslationsKey, customerId),
                () => _masterDataService.GetCustomerCaseTranslations(customerId),
                DateTime.UtcNow.AddMinutes(10));
        }

        public void ClearCaseTranslations(int customerId)
        {
            _cacheService.Delete(string.Format(_caseTranslationsFormat, _caseTranslationsKey, customerId));
        }

        public IList<Text> GetTextTranslations()
        {
            return _cacheService.Get(_textTranslationsKey,
                    () => _masterDataService.GetTranslationTexts(),
                    DateTime.UtcNow.AddMinutes(10));
        }

        public void ClearTextTranslations()
        {
            _cacheService.Delete(_textTranslationsKey);
        }
    }

    public interface IHelpdeskCache
    {
        IList<CaseFieldSettingsForTranslation> GetCaseTranslations(int customerId);
        void ClearCaseTranslations(int customerId);
        IList<Text> GetTextTranslations();
        void ClearTextTranslations();
    }
}