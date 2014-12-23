namespace DH.Helpdesk.NewSelfService.Infrastructure
{
    using System.Linq;
    using DH.Helpdesk.Common.Extensions.String;

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
                        if (translation != null)
                            translate = translation.TextTranslations.Where(x => x.Language_Id == SessionFacade.CurrentLanguageId).FirstOrDefault().TextTranslated ?? translate;
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
                            translate = translate.GetDefaultValue(SessionFacade.CurrentLanguageId);
                        //translate = Get(translate, Enums.TranslationSource.TextTranslation); 
                    }
                    catch
                    {
                    }
                }
            }

            return translate;
        }

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