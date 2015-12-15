namespace DH.Helpdesk.Web.Infrastructure
{
    using System.Linq;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Enums;

    public static class Translation
    {
        /// <summary>
        /// Get translation for a string. It will generate translation based on currentLanguageId in the session.
        /// </summary>
        /// <param name="translate"></param>
        /// <param name="source"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [System.Obsolete("This is obsolete. Please use either GetMasterDataTranslation, GetCoreTextTranslation, GetForCase, or GetTextTranslationByTextType. This is spotty because it can give translations that isn't necessarily correct (based on texttype) and can give faulty translations")]
        public static string Get(string translate, Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, int customerId = 0)
        {
            translate = Get(translate, SessionFacade.CurrentLanguageId, source, customerId);
            return translate;
        }

        /// <summary>
        /// Will give you a translation for a specific textTypeId
        /// For currentLanguageId in session
        /// </summary>
        /// <param name="translate"></param>
        /// <param name="TextTypeId"></param>
        /// <returns></returns>
        public static string GetTextTranslationByTextType(string translate, int TextTypeId)
        {
            return translate = Get(translate, SessionFacade.CurrentLanguageId, Enums.TranslationSource.TextTranslation,0,true,TextTypeId);
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

        private static string GetInstanceWord(string word)
        {
            switch (word.ToLower())
            {
                case "_temporary_.leadtime":
                    return "Tid kvar";

                case "ledtid":
                    return "Ledtid";
            }

            return string.Empty;
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

        private static string Get(string translate, int languageId, Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, int customerId = 0, bool DiffTextType = false, int TextTypeId = 0)
        {
            if (source == Enums.TranslationSource.TextTranslation)
            {
                if (SessionFacade.TextTranslation != null)
                {
                    try
                    {
                        var translation = SessionFacade.TextTranslation.Where(x => x.TextToTranslate.ToLower() == translate.ToLower()).FirstOrDefault();
                        if (DiffTextType)
                        {
                            translation = SessionFacade.TextTranslation.Where(x => x.TextToTranslate.ToLower() == translate.ToLower() && x.Type == TextTypeId).FirstOrDefault();
                        }
                                                
                        if (translation != null)
                        {
                            var trans = translation.TextTranslations.Where(x => x.Language_Id == languageId).FirstOrDefault();
                            var text = (trans != null ? trans.TextTranslated : string.Empty);
                            if (string.IsNullOrEmpty(text) && SessionFacade.CurrentLanguageId != LanguageIds.Swedish)
                            {
                                trans = translation.TextTranslations.Where(x => x.Language_Id == SessionFacade.CurrentLanguageId).FirstOrDefault();
                                text = (trans != null ? trans.TextTranslated : string.Empty);
                            }

                            translate = !string.IsNullOrEmpty(text) ? text : translate;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else if (source == Enums.TranslationSource.CaseTranslation)
            {
                if (SessionFacade.CaseTranslation != null && customerId > 0)
                {
                    try
                    {
                        var translation = SessionFacade.CaseTranslation.Where(x => x.Customer_Id == customerId && x.Name.ToLower() == translate.getCaseFieldName().ToLower() && x.Language_Id == languageId).FirstOrDefault();
                        if (translation != null && !string.IsNullOrEmpty(translation.Label))
                            translate = translation.Label;
                        else
                        {
                            var translateByText = string.Empty;
                            var instanceWord = GetInstanceWord(translate);
                            if (instanceWord != string.Empty)
                            {
                                var translationText = SessionFacade.TextTranslation.Where(x => x.TextToTranslate.ToLower() == instanceWord.ToLower()).FirstOrDefault();
                                if (translationText != null)
                                {
                                    var trans = translationText.TextTranslations.Where(x => x.Language_Id == languageId).FirstOrDefault();
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
    }

}