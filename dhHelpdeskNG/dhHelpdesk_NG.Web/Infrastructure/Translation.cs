﻿namespace DH.Helpdesk.Web.Infrastructure
{
    using System.Linq;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Enums;

    public static class Translation
    {
        public static string Get(string translate, Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, int customerId = 0)
        {
            if (source == Enums.TranslationSource.TextTranslation)
            {
                if (SessionFacade.TextTranslation != null)
                {
                    try
                    {
                        var translation = SessionFacade.TextTranslation.Where(x => x.TextToTranslate.ToLower() == translate.ToLower()).FirstOrDefault();
                        if(translation != null)
                        {                           
                            var trans = translation.TextTranslations.Where(x => x.Language_Id == SessionFacade.CurrentLanguageId).FirstOrDefault();
                            var text = (trans != null ? trans.TextTranslated : string.Empty);
                            if (string.IsNullOrEmpty(text) && SessionFacade.CurrentLanguageId != LanguageIds.Swedish)
                            {
                                trans = translation.TextTranslations.Where(x => x.Language_Id == SessionFacade.CurrentCustomer.Language_Id).FirstOrDefault();
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
                        var translation = SessionFacade.CaseTranslation.Where(x => x.Customer_Id == customerId && x.Name.ToLower() == translate.getCaseFieldName().ToLower() && x.Language_Id == SessionFacade.CurrentLanguageId).FirstOrDefault();

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
                                     var trans = translationText.TextTranslations.Where(x => x.Language_Id == SessionFacade.CurrentLanguageId).FirstOrDefault();
                                     translateByText = (trans != null ? trans.TextTranslated : string.Empty);
                                     if (translateByText != string.Empty)
                                         translate = translateByText;
                                 }
                             }
                             
                             if (translateByText == string.Empty)                             
                                translate = translate.GetDefaultValue(SessionFacade.CurrentLanguageId);
                        }
                            //Apparently this row is commented out because it is replaced by .GetDefaultValue above
                            //translate = Get(translate, Enums.TranslationSource.TextTranslation); 
                    }
                    catch
                    {
                    }
                }                                    
            }

            return translate;
        }

        private static string GetInstanceWord(string word)
        {
            switch (word.ToLower())
            {
                case "_temporary_.leadtime":
                    return "Tid kvar";                    
            }

            return string.Empty;
        }


        public static string Get(string translate, int languageId, Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, int customerId = 0)
        {
            if (source == Enums.TranslationSource.TextTranslation)
            {
                if (SessionFacade.TextTranslation != null)
                {
                    try
                    {
                        var translation = SessionFacade.TextTranslation.Where(x => x.TextToTranslate.ToLower() == translate.ToLower()).FirstOrDefault();
                        if (translation != null)
                        {                            
                            var trans = translation.TextTranslations.Where(x => x.Language_Id == languageId).FirstOrDefault();
                            var text = (trans != null ? trans.TextTranslated : string.Empty);
                            if (string.IsNullOrEmpty(text) && SessionFacade.CurrentLanguageId != LanguageIds.Swedish)
                            {
                                trans = translation.TextTranslations.Where(x => x.Language_Id == SessionFacade.CurrentCustomer.Language_Id).FirstOrDefault();
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

        public static string getCaseFieldName(this string value)
        {
            return value.Replace("tblLog_", "tblLog.");
        }
    }

}