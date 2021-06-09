namespace DH.Helpdesk.SelfService.Infrastructure
{
    using System.Linq;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Enums;

    public static class Translation
    {
        public static string GetForJS(string translate, Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, int customerId = 0)
        {
            translate = Get(translate, SessionFacade.CurrentLanguageId, source, customerId);
            translate = translate.Replace("'", "\\'");
            translate = translate.Replace("\"", "\\'");
            return translate;
        }

        public static string Get(string translate, Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, int customerId = 0)
        {
            return Get(translate, SessionFacade.CurrentLanguageId, source, customerId);
        }

        public static string Get(string translate, int languageId, Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, int customerId = 0)
        {
            if (source == Enums.TranslationSource.TextTranslation)
            {
                if (SessionFacade.TextTranslation != null && translate != null)
                {
                    try
                    {
                        var translation = SessionFacade.TextTranslation.FirstOrDefault(x => x.TextToTranslate.ToLower() == translate.ToLower());
                        if (translation != null)
                        {
                            var trans = translation.TextTranslations.FirstOrDefault(x => x.Language_Id == languageId);
                            var text = (trans != null ? trans.TextTranslated : string.Empty);
                            //if (string.IsNullOrEmpty(text) && SessionFacade.CurrentLanguageId != LanguageIds.Swedish)
                            //{
                            //    trans = translation.TextTranslations.Where(x => x.Language_Id == SessionFacade.CurrentCustomer.Language_Id).FirstOrDefault();
                            //    text = (trans != null ? trans.TextTranslated : string.Empty);
                            //}

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
                        var translation = SessionFacade.CaseTranslation.FirstOrDefault(x => x.Customer_Id == customerId && x.Name.ToLower() == translate.GetCaseFieldName().ToLower() && x.Language_Id == languageId);
                        if (translation != null && !string.IsNullOrEmpty(translation.Label))
                            translate = translation.Label;
                        else
                        {
                            var translateByText = string.Empty;
                            var instanceWord = GetInstanceWord(translate);
                            if (instanceWord != string.Empty)
                            {
                                var translationText = SessionFacade.TextTranslation.FirstOrDefault(x => x.TextToTranslate.ToLower() == instanceWord.ToLower());
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

        public static string GetCaseFieldName(this string value)
        {
            return value.Replace("tblLog_", "tblLog.");
        }
        private static string GetInstanceWord(string word)
        {
            switch (word.ToLower())
            {
                case "_temporary_leadtime":
                    return "Tid kvar";
            }

            return string.Empty;
        }
    }

}