using DH.Helpdesk.Dal.Infrastructure.Translate;

namespace DH.Helpdesk.WebApi.Infrastructure.Translate
{
    public sealed class Translator : ITranslator
    {
        public string Translate(string text)
        {
            return Translation.Get(text);
        }
    }
}