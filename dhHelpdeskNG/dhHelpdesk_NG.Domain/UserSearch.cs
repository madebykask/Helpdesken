
namespace DH.Helpdesk.Domain
{
    public interface IUserSearch : ISearch
    {
        int CustomerId { get; set; }
        string SearchUs { get; set; }
    }

    public class UserSearch : Search, IUserSearch
    {
        public int CustomerId { get; set; }
        public string SearchUs { get; set; }

    }
}
