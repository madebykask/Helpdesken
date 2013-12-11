using System.Linq;

namespace dhHelpdesk_NG.Web.Infrastructure
{
    public static class Translation
    {
        public static string Get(string translate, Enums.TranslationSource source, int customerId = 0)
        {
            if (source == Enums.TranslationSource.TextTranslation)
            {
                if (SessionFacade.TextTranslation != null)
                {
                    try
                    {
                        var translation = SessionFacade.TextTranslation.Where(x => x.TextToTranslate == translate).FirstOrDefault();

                        if (translation != null)
                        {
                            translate = translation.TextTranslations.Where(x => x.Language_Id == SessionFacade.CurrentLanguage).FirstOrDefault().TextTranslated ?? translate;
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
                        var translation = SessionFacade.CaseTranslation.Where(x => x.Customer_Id == customerId && x.Name == translate && x.Language_Id == SessionFacade.CurrentLanguage).FirstOrDefault();

                        if (translation != null)
                        {
                            translate = translation.Label;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return translate;
        }
    }
}