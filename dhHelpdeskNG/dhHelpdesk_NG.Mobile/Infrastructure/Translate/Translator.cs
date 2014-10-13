namespace DH.Helpdesk.Mobile.Infrastructure.Translate
{
    using DH.Helpdesk.Dal.Infrastructure.Translate;

    public sealed class Translator : ITranslator
    {
        public string Translate(string text)
        {
            return Translation.Get(text);
        }
    }
}