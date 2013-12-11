
namespace dhHelpdesk_NG.Domain
{
    public interface IBulletinBoardSearch : ISearch
    {
        int CustomerId { get; set; }
        string SearchBbs { get; set; }
    }

    public class BulletinBoardSearch : Search, IBulletinBoardSearch
    {
        public int CustomerId { get; set; }
        public string SearchBbs { get; set; }
    }
}
