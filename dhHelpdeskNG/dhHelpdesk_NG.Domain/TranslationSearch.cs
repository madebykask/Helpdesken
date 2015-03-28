
namespace DH.Helpdesk.Domain
{
    public interface ITranslationSearch : ISearch
    {
        string SearchTextTr { get; set; }

    }

    public class TranslationSearch : Search, ITranslationSearch
    {
        public string SearchTextTr { get; set; }

    }

   
}
