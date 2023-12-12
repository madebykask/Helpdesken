namespace DH.Helpdesk.TaskScheduler.Infrastructure.Translate
{
    public static class Translation
    {
        public static string Get(string translate)//,Enums.TranslationSource source = Enums.TranslationSource.TextTranslation, int customerId = 0
        {
            return translate;//TODO: implement translation for helpdesk and selfservice
        }

        public static string GetCaseFieldName(this string value)
        {
            return value.Replace("tblLog_", "tblLog.");
        }
    }
}
