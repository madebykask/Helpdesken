using System.Globalization;
using System.Threading.Tasks;
using DH.Helpdesk.Common.Extensions.Boolean;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using DH.Helpdesk.BusinessData.Models.ExtendedCase;
    using DH.Helpdesk.BusinessData.Models.Language.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public interface ILanguageService
    {
        IList<Language> GetLanguages(bool active = true);
        Task<IList<Language>> GetLanguagesAsync(bool active = true);

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

        IList<ItemOverview> GetOverviews(bool active = false);
        IList<ExtendedCaseFieldTranslation> GetExtendedCaseTranslations(ExtendedCaseFormJsonModel form, int? languageId, List<ExtendedCaseFieldTranslation> initialTranslations);
    }

    public class LanguageService : ILanguageService //TODO: needs refactoring
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

        public IList<Language> GetLanguages(bool active = true)
        {
            var intActive = active.ToInt();
            return _languageRepository.GetMany(x => x.IsActive == intActive).OrderBy(x => x.Name).ToList();
        }

        public async Task<IList<Language>> GetLanguagesAsync(bool active = true)
        {
            return await _languageRepository.GetLanguagesAsync(active);
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
            return GetOverviews(true).ToList();
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

        public IList<ItemOverview> GetOverviews(bool active = false)
        {
            var languages = GetLanguages(active);
            return languages.Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        public IList<ExtendedCaseFieldTranslation> GetExtendedCaseTranslations(ExtendedCaseFormJsonModel form, int? languageId, List<ExtendedCaseFieldTranslation> initialTranslations)
        {
            var activeLanguages = _languageRepository.GetActiveLanguages();
            var defaultLanguage = activeLanguages.Where(x => x.IsActive == 1).OrderBy(x=> x.Id).FirstOrDefault();

            IList<ExtendedCaseFieldTranslation> fieldtranslations = new List<ExtendedCaseFieldTranslation>();
            if (form == null)
            {
                foreach (var t in initialTranslations)
                {
                    var prefix = t.Name == initialTranslations[0].Name ? "Section." : "Control."; 
                    fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                    {
                        Language = defaultLanguage,
                        IsDefaultLanguage = true,
                        Name = t.Name,
                        TranslationText =
                            GetExtendedCaseTranslation(defaultLanguage.Id, prefix + Regex.Replace(StringHelper.HandleSwedishChars(t.Name), "[^a-zA-Z0-9 _]", "", RegexOptions.Compiled))
                    });
                }
                foreach (var al in activeLanguages.Where(l => l.Id != defaultLanguage.Id))
                {
                    foreach (var t in initialTranslations)
                    {
                        var prefix = t.Name == initialTranslations[0].Name ? "Section." : "Control.";
                        fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                        {
                            Language = al,
                            IsDefaultLanguage = (al.Id == defaultLanguage.Id),
                            Name = t.Name,
                            TranslationText =
                                GetExtendedCaseTranslation(al.Id, prefix + Regex.Replace(StringHelper.HandleSwedishChars(t.Name), "[^a-zA-Z0-9]", "", RegexOptions.Compiled))
                        });
                    }
                }
            }
            else
            {
                fieldtranslations = GetExtendedCaseTranlations(form, languageId, activeLanguages, defaultLanguage);
            }

            return fieldtranslations;
        }

        private IList<ExtendedCaseFieldTranslation> GetExtendedCaseTranlations(ExtendedCaseFormJsonModel form, int? languageId, IEnumerable<LanguageOverview> activeLanguages, LanguageOverview usedLanguage)
        {
            IList<ExtendedCaseFieldTranslation> fieldtranslations = new List<ExtendedCaseFieldTranslation>();
            foreach (var t in form.tabs)
            {
                foreach (var s in t.sections)
                {
                    fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                    {
                        Language = usedLanguage,
                        Name = s.name,
                        TranslationText =
                            GetExtendedCaseTranslation(usedLanguage.Id, "Section." + Regex.Replace(s.name, "[^a-zA-Z0-9]", "", RegexOptions.Compiled))
                    });

                    foreach (var l in activeLanguages.Where(x => x.Id != languageId))
                    {
                        fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                        {
                            Language = l,
                            Name = s.name,
                            TranslationText =
                                GetExtendedCaseTranslation(l.Id, "Section." + Regex.Replace(s.name, "[^a-zA-Z0-9]", "", RegexOptions.Compiled))
                        });
                    }
                    foreach (var c in s.controls)
                    {
                        fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                        {
                            Language = usedLanguage,
                            Name = c.label,
                            TranslationText =
                                GetExtendedCaseTranslation(usedLanguage.Id, "Control." + Regex.Replace(c.label, "[^a-zA-Z0-9]", "", RegexOptions.Compiled))
                        });

                        foreach (var l in activeLanguages.Where(x => x.Id != languageId))
                        {
                            fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                            {
                                Language = l,
                                Name = c.label,
                                TranslationText =
                                    GetExtendedCaseTranslation(l.Id, "Control." + Regex.Replace(c.label, "[^a-zA-Z0-9]", "", RegexOptions.Compiled))
                            });
                        }
                    }
                }
            }

            return fieldtranslations;
        }

        private string GetExtendedCaseTranslation(int id, string name)
        {
            return _languageRepository.GetExtendedCaseTranslation(id, name);
        }
    }
}
