using System.Globalization;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Language.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ILanguageService
    {
        IList<Language> GetLanguages();
        IList<Language> GetLanguagesForGlobalSettings();

        Language GetLanguage(int id);

        DeleteMessage DeleteLanguage(int id);

        void SaveLanguage(Language language, out IDictionary<string, string> errors);
        void Commit();

        List<ItemOverview> GetActiveOverviews();

        /// <summary>
        /// The get active languages.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<LanguageOverview> GetActiveLanguages();

        List<ItemOverview> FindActiveLanguageOverivews();
    }

    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LanguageService(
            ILanguageRepository languageRepository,
            IUnitOfWork unitOfWork)
        {
            this._languageRepository = languageRepository;
            this._unitOfWork = unitOfWork;
        }

        public IList<Language> GetLanguages()
        {
            return this._languageRepository.GetMany(x => x.IsActive == 1).OrderBy(x => x.Name).ToList();
        }

        public IList<Language> GetLanguagesForGlobalSettings()
        {
            return this._languageRepository.GetMany(z => z.IsActive == 1).ToList();
        }

        public Language GetLanguage(int id)
        {
            return this._languageRepository.GetById(id);
        }

        public DeleteMessage DeleteLanguage(int id)
        {
            var language = this._languageRepository.GetById(id);

            if (language != null)
            {
                try
                {
                    this._languageRepository.Delete(language);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveLanguage(Language language, out IDictionary<string, string> errors)
        {
            if (language == null)
                throw new ArgumentNullException("language");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(language.Name))
                errors.Add("Language.Name", "Du måste ange ett språk");

            if (string.IsNullOrEmpty(language.LanguageID))
                errors.Add("Language.LanguageID", "Du måste ange ett språk id");

            if (language.Id == 0)
                this._languageRepository.Add(language);
            else
                this._languageRepository.Update(language);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public List<ItemOverview> GetActiveOverviews()
        {            
            var languagesEntity = this._languageRepository.GetActiveLanguages();
            var languages = this._languageRepository.FindActiveOverviewsByIds(languagesEntity.Select(l => l.Id).ToList());            
            return languages;
        }

        /// <summary>
        /// The get active languages.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<LanguageOverview> GetActiveLanguages()
        {
            return this._languageRepository.GetActiveLanguages();
        }

        public List<ItemOverview> FindActiveLanguageOverivews()
        {
            var overviews = this._languageRepository.GetAll().Select(l => new { Name = l.Name, Value = l.Id.ToString() }).ToList();
            return overviews.Select(o => new ItemOverview(o.Name, o.Value.ToString(CultureInfo.InvariantCulture))).ToList();
        }
    }
}
