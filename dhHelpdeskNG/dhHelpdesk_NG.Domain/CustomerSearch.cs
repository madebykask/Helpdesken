
namespace DH.Helpdesk.Domain
{
    public interface ICustomerSearch : ISearch
    {
        string SearchCs { get; set; }
	}

    public class CustomerSearch : Search, ICustomerSearch
    {
        public string SearchCs { get; set; }
		public bool ActiveOnly { get; set; }
	}

    public interface ICustomerSearchSelection : ISearch
    {
        int CustomerId { get; set; }
        int? UserGroupId { get; set; }
    }

    public class CustomerSearchSelection : Search, ICustomerSearchSelection
    {
        public int CustomerId { get; set; }
        public int? UserGroupId { get; set; }
    }
}
