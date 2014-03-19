
namespace DH.Helpdesk.Domain
{
    public interface IDocumentSearch : ISearch
    {
        int CustomerId { get; set; }
        string SearchDs { get; set; }
    }

    public class DocumentSearch : Search, IDocumentSearch
    {
        public int CustomerId { get; set; }
        public string SearchDs { get; set; }
    }
}
