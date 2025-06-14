﻿using System.Globalization;
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
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public LanguageService(
            ILanguageRepository languageRepository,
            IUnitOfWork unitOfWork)
        {
            this._languageRepository = languageRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

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
                fieldtranslations = GetInitialTranslations(initialTranslations, activeLanguages, defaultLanguage);
            }
            else
            {
                fieldtranslations = GetExtendedCaseTranslations(form, activeLanguages, defaultLanguage);

                foreach (var t in GetInitialTranslations(initialTranslations, activeLanguages, defaultLanguage))
                {
                    fieldtranslations.Add(t);
                }
            }

            return fieldtranslations;
        }

        private IList<ExtendedCaseFieldTranslation> GetInitialTranslations(List<ExtendedCaseFieldTranslation> initialTranslations, IEnumerable<LanguageOverview> activeLanguages, LanguageOverview defaultLanguage)
        {
            IList<ExtendedCaseFieldTranslation> fieldtranslations = new List<ExtendedCaseFieldTranslation>();

            foreach (var t in initialTranslations)
            {
                //var prefix = t.Name == initialTranslations[0].Name ? "Section." : "Control.";
                fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                {
                    Language = defaultLanguage,
                    IsDefaultLanguage = true,
                    Name = t.Name,
                    Prefix = t.Prefix,
                    TranslationText =
                        GetExtendedCaseTranslation(t.Prefix + "." + StringHelper.GetCleanString(t.Name), defaultLanguage.Id)
                });
            }
            foreach (var al in activeLanguages.Where(l => l.Id != defaultLanguage.Id))
            {
                foreach (var t in initialTranslations)
                {
                    //var prefix = t.Name == initialTranslations[0].Name ? "Section." : "Control.";
                    fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                    {
                        Language = al,
                        IsDefaultLanguage = (al.Id == defaultLanguage.Id),
                        Name = t.Name,
                        Prefix = t.Prefix,
                        TranslationText =
                            GetExtendedCaseTranslation(t.Prefix + "." + StringHelper.GetCleanString(t.Name), al.Id)
                    });
                }
            }
            return fieldtranslations;
        }

        private IList<ExtendedCaseFieldTranslation> GetExtendedCaseTranslations(ExtendedCaseFormJsonModel form, IEnumerable<LanguageOverview> activeLanguages, LanguageOverview defaultLanguage)
        {
            IList<ExtendedCaseFieldTranslation> fieldtranslations = new List<ExtendedCaseFieldTranslation>();
            foreach (var t in form.tabs)
            {
                fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                {
                    Id = _languageRepository.GetExtendedFormTranslationId(t.name, defaultLanguage.Id),
                    Language = defaultLanguage,
                    Name = t.name,
                    TranslationText =GetExtendedCaseTranslation(t.name.Replace("@Translation.", ""), defaultLanguage.Id),
                    IsDefaultLanguage = true,
                    Prefix = "Tab"
                });

                foreach (var l in activeLanguages.Where(x => x.Id != defaultLanguage.Id))
                {
                    fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                    {
                        Id = _languageRepository.GetExtendedFormTranslationId(t.name, l.Id),
                        Language = l,
                        Name = t.name,
                        TranslationText =
                            GetExtendedCaseTranslation(t.name.Replace("@Translation.", ""), l.Id),
                        Prefix = "Tab"
                    });
                }

                foreach (var s in t.sections.Where(x => x.id != "InitiatorInfo" && x.id != "HiddenFields"))
                {
                    fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                    {
                        Id = _languageRepository.GetExtendedFormTranslationId(s.name, defaultLanguage.Id),
                        Language = defaultLanguage,
                        Name = s.name,
                        TranslationText =
                            GetExtendedCaseTranslation(s.name.Replace("@Translation.",""), defaultLanguage.Id),
                        IsDefaultLanguage = true,
                        Prefix = "Section"
                    });

                    foreach (var l in activeLanguages.Where(x => x.Id != defaultLanguage.Id))
                    {
                        fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                        {
                            Id = _languageRepository.GetExtendedFormTranslationId(s.name, l.Id),
                            Language = l,
                            Name = s.name,
                            TranslationText =
                                GetExtendedCaseTranslation(s.name.Replace("@Translation.", ""), l.Id),
                            Prefix = "Section"
                        });
                    }

                    foreach (var c in s.controls)
                    {
                        fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                        {
                            Id = _languageRepository.GetExtendedFormTranslationId(c.label, defaultLanguage.Id),
                            Language = defaultLanguage,
                            Name = c.label,
                            TranslationText = GetExtendedCaseTranslation(c.label.Replace("@Translation.", ""), defaultLanguage.Id),
                            IsDefaultLanguage = true,
                            Prefix = "Control"
                        });

                        if (c.addonText != null)
                        {
                            fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                            {
                                Id = _languageRepository.GetExtendedFormTranslationId(c.addonText, defaultLanguage.Id),
                                Language = defaultLanguage,
                                Name = c.addonText,
                                TranslationText = GetExtendedCaseTranslation(c.addonText.Replace("@Translation.", ""), defaultLanguage.Id),
                                IsDefaultLanguage = true,
                                Prefix = "Message"
                            });
                        }

                        if(c.dataSource != null)
                        {
                            foreach(var d in c.dataSource)
                            {
                                fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                                {
                                    Id = _languageRepository.GetExtendedFormTranslationId(d.text, defaultLanguage.Id),
                                    Language = defaultLanguage,
                                    Name = d.text,
                                    TranslationText = GetExtendedCaseTranslation(d.text.Replace("@Translation.", ""), defaultLanguage.Id),
                                    IsDefaultLanguage = true,
                                    Prefix = "DataSource.Value"
                                });
                            }
                        }

                        foreach (var l in activeLanguages.Where(x => x.Id != defaultLanguage.Id))
                        {
                            fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                            {
                                Id = _languageRepository.GetExtendedFormTranslationId(c.label, l.Id),
                                Language = l,
                                Name = c.label,
                                TranslationText = GetExtendedCaseTranslation(c.label.Replace("@Translation.", ""), l.Id),
                                IsDefaultLanguage = false,
                                Prefix = "Control"
                            });

                            if (c.addonText != null)
                            {
                                fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                                {
                                    Id = _languageRepository.GetExtendedFormTranslationId(c.addonText, l.Id),
                                    Language = l,
                                    Name = c.addonText,
                                    TranslationText = GetExtendedCaseTranslation(c.addonText.Replace("@Translation.", ""), l.Id),
                                    IsDefaultLanguage = false,
                                    Prefix = "Message"
                                });
                            }

                            if (c.dataSource != null)
                            {
                                foreach (var d in c.dataSource)
                                {
                                    fieldtranslations.Add(new ExtendedCaseFieldTranslation()
                                    {
                                        Id = _languageRepository.GetExtendedFormTranslationId(d.text, l.Id),
                                        Language = l,
                                        Name = d.text,
                                        TranslationText = GetExtendedCaseTranslation(d.text.Replace("@Translation.", ""), l.Id),
                                        IsDefaultLanguage = true,
                                        Prefix = "DataSource.Value"
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return fieldtranslations;
        }

        private string GetExtendedCaseTranslation(string name, int languageId)
        {
            return _languageRepository.GetExtendedCaseTranslation(name, languageId);
        }
    }
}
