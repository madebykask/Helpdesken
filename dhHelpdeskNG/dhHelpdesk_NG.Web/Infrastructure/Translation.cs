﻿using System;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Web.Infrastructure.Cache;

namespace DH.Helpdesk.Web.Infrastructure
{
    using System.Linq;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Enums;
    using System.Collections.Generic;
    using System.Globalization;

    public static class Translation
    {
        public static ITranslateCacheService Cache
        {
            get
            {
                return DependencyResolver.Current.GetService<ITranslateCacheService>();//hack to support legacy code. TODO: should be injection, otherwise WebApi possible issues
            } 
        }

        /// <summary>
        /// Get translation for a string. It will generate translation based on currentLanguageId in the session.
        /// </summary>
        /// <param name="translate"></param>
        /// <param name="source"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        //[System.Obsolete("This is obsolete. Please use either GetMasterDataTranslation, GetCoreTextTranslation, GetForCase, or GetTextTranslationByTextType. This is spotty because it can give translations that isn't necessarily correct (based on texttype) and can give faulty translations")]
        public static string Get(string translate, Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, int customerId = 0)
        {
#pragma warning disable 0618
            translate = Get(translate, SessionFacade.CurrentLanguageId, source, customerId);            
            return translate;
#pragma warning restore 0618
        }

        public static string GetForJS(string translate, Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, int customerId = 0)
        {
#pragma warning disable 0618
            translate = Get(translate, SessionFacade.CurrentLanguageId, source, customerId);
            translate = translate.Replace("'", "\\'");
            translate = translate.Replace("\"", "\\'");
            return translate;
#pragma warning restore 0618
        }

        internal static CaseType TranslateCaseType(CaseType caseType)
        {
            if (caseType.ParentCaseType != null)
                caseType.ParentCaseType = TranslateCaseType(caseType.ParentCaseType);

            caseType.Name = GetCoreTextTranslation(caseType.Name);

            return caseType;
        }

        /// <summary>
        /// Will give you a translation for a specific textTypeId
        /// For currentLanguageId in session
        /// </summary>
        /// <param name="translate"></param>
        /// <param name="textTypeId"></param>
        /// <returns></returns>
        public static string GetTextTranslationByTextType(string translate, int textTypeId)
        {
            return translate = Get(translate, SessionFacade.CurrentLanguageId, Enums.TranslationSource.TextTranslation, 0, true, textTypeId);
        }

        /// <summary>
        /// Will give you master data translation (texttypeId 1)
        /// Translation will be based on currentLanguageId
        /// </summary>
        /// <param name="translate"></param>
        /// <returns></returns>
        public static string GetMasterDataTranslation(string translate)
        {
            return GetTextTranslationByTextType(translate, 1);
        }

        /// <summary>
        /// Will give you translation for Core text (textTypeId 0)
        /// </summary>
        /// <param name="translate"></param>
        /// <returns></returns>
        public static string GetCoreTextTranslation(string translate)
        {
            return GetTextTranslationByTextType(translate, 0);
        }

        public static string GetInstanceWord(string word)
        {
            switch (word.ToLower())
            {
                case "_temporary_leadtime":
                    return "Tid kvar";

                case "ledtid":
                    return "Ledtid";
            }

            return string.Empty;
        }
        public static string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
        /// <summary>
        /// Get translation for specific language
        /// </summary>
        /// <param name="translate"></param>
        /// <param name="languageId"></param>
        /// <param name="source"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [System.Obsolete("This is spotty because it can give translations that isn't necessarily correct (based on texttype) and can give faulty translations")]
        public static string Get(string translate, int languageId, Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, int customerId = 0)
        {
            return translate = Get(translate, languageId, source, customerId, false);
        }

        private static string Get(string translate, int languageId, Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, 
                                  int customerId = 0, bool diffTextType = false, int textTypeId = 0)
        {
            if (source == Enums.TranslationSource.TextTranslation && translate != null)
            {
                try
                {
                    var translations = Cache.GetAllTextTranslationsOptimized();
                    TextList translation = null;
                    if (diffTextType)
                    {
                        if (translations.ContainsKey(translate.ToLower() + textTypeId))
                            translation = translations[translate.ToLower() + textTypeId];
                    }
                    else
                    {
                        var types = Cache.GetAllTextTypes();
                        foreach (var textType in types)
                        {
                            if (!translations.ContainsKey(translate.ToLower() + textType.Id)) continue;
                            translation = translations[translate.ToLower() + textType.Id];
                            break;
                        }
                    }

                    if (translation != null)
                    {
                        var trans = translation.Translations.FirstOrDefault(x => x.Language_Id == languageId);
                        var text = (trans != null ? trans.TranslationName : string.Empty);
                        if (string.IsNullOrEmpty(text) && SessionFacade.CurrentLanguageId != LanguageIds.Swedish)
                        {
                            trans = translation.Translations.FirstOrDefault(x => x.Language_Id == SessionFacade.CurrentLanguageId);
                            text = (trans != null ? trans.TranslationName : string.Empty);
                        }

                        translate = !string.IsNullOrEmpty(text) ? text : translate;
                    }
                }
                catch(Exception)
                {
                }
            }
            else if (source == Enums.TranslationSource.CaseTranslation)
            {
                if (customerId > 0)
                {
                    try
                    {
                        var translation = Cache.GetCaseTranslations(customerId).FirstOrDefault(x => x.Name.ToLower() == translate.getCaseFieldName().ToLower() && x.Language_Id == languageId);
                        if (translation != null && !string.IsNullOrEmpty(translation.Label))
                            translate = translation.Label;
                        else
                        {
                            var translateByText = string.Empty;
                            var instanceWord = GetInstanceWord(translate);
                            if (instanceWord != string.Empty)
                            {
                                var translationText = Cache.GetAllTextTranslations().FirstOrDefault(x => x.TextToTranslate.ToLower() == instanceWord.ToLower());
                                if (translationText != null)
                                {
                                    var trans = translationText.TextTranslations.FirstOrDefault(x => x.Language_Id == languageId);
                                    translateByText = (trans != null ? trans.TextTranslated : string.Empty);
                                    if (translateByText != string.Empty)
                                        translate = translateByText;
                                }
                            }

                            if (translateByText == string.Empty)
                                translate = translate.GetDefaultValue(languageId);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return translate;
        }
        
        /// <summary>
        /// The get case.
        /// </summary>
        /// <param name="translate">
        /// The translate.
        /// </param>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetForCase(string translate, int customerId = 0)
        {
            return Get(translate, Enums.TranslationSource.CaseTranslation, customerId);
        }

        /// <summary>
        /// Returns tranlstion for case using SessionFacade.CurrentCustomer.Id
        /// </summary>
        /// <param name="translate"></param>
        /// <returns></returns>
        public static string CaseString(string translate)
        {
            return Get(translate, Enums.TranslationSource.CaseTranslation, SessionFacade.CurrentCustomer.Id);
        }

        public static string getCaseFieldName(this string value)
        {
            return value.Replace("tblLog_", "tblLog.");
        }

        public static List<KeyValuePair<string, string>> GetTranslationsForJS()
        {
            var texts = GetJSTextDictionary();
            var ret = new List<KeyValuePair<string, string>>();
            foreach (var text in texts)
                ret.Add(new KeyValuePair<string, string>(text, Translation.GetCoreTextTranslation(text)));

            return ret;
        }

        private static List<string> GetJSTextDictionary()
        {
            /* Note: Should be carefull to add to this list 
             *       As it will pass by json, if it be too much, system can't send it. 
             *       (Depend on MaxJsonLength)
             */
            var ret = new List<string> 
                        {
                            "saknas!",
                            "0 är inte tillåtet",
                            "Alla Totalt",
                            "Art nr",
                            "Artikel",
                            "Artikelnamn ENG",
                            "Artikelnamn SVE",
                            "Artiklar att fakturera",
                            "Avbryt",
                            "Beskrivning",
                            "Bifoga",
                            "Bifogade filer",
                            "Den här ordern är redan skickat. Du kan inte utföra den här aktiviteten.",
                            "Det finns inga artiklar att skicka!",
                            "Det finns inga filer att lägga till.",
                            "Det finns inga valbara filer att lägga till på ordern.",
                            "Det saknas inställningar för fakturering",
                            "Du har valt en artikel, men inte lagt till den än!",
                            "Du kan inte använda ärendemall eftersom det finns order som inte är skickade.",
                            "Du kan inte ändra",
                            "eftersom det finns order som inte är skickade.",
                            "Du måste spara ärendet en gång innan du kan skicka",
                            "Ej skickat Totalt",
                            "Endast PDF-filer som är bifogade på ärendet går att bifoga.",
                            "Enheter",
                            "Ett fel har uppstått vid spara Order",
                            "för",
                            "Inget kvar att kreditera",
                            "kan inte faktureras",
                            "kan inte ändras eftersom det finns order som är skickade.",
                            "Kredit",
                            "Kreditera",
                            "Kreditera order",
                            "kunde inte sparas då det saknas data i ett eller flera obligatoriska fält. Var vänlig kontrollera i ordern.",
                            "Lägg order",
                            "Lägg till",
                            "Max antal för denna artikel är",
                            "Namn",
                            "Ny order är redan skapad",
                            "Order",
                            "Order saknas",
                            "Orderreferens",
                            "PPE SEK",
                            "Projekt",
                            "Skicka",
                            "Skickat",
                            "Skickat av",
                            "Skickat Totalt",
                            "Spara",
                            "Spara och stäng",
                            "Spara...",
                            "Ta bort",
                            "Tecken kvar",
                            "Textrad",
                            "Total",
                            "Totalt alla ordrar",
                            "Typ",
                            "Vald",                            
                            "Var god kontakta systemadministratör.",
                            "Var vänlig spara ordern först.",
                            "Var vänlig spara ärendet och försök igen!",
                            "Välj artikel",
                            "Ärende",
                            "Ärendefiler",
                            "Översikt"                                                        
                        };
            return ret;
        }
    }

}