
namespace DH.Helpdesk.Domain
{
    public interface IComputerUserSearch : ISearch
    {
        int CustomerId { get; set; }
        int? DepartmentId { get; set; }
        int? DivisionId { get; set; }
        int? DomainId { get; set; }
        int? OuId { get; set; }
        int? RegionId { get; set; }
        int? StatusId { get; set; }
        string SearchCompUs { get; set; }
    }

    public class ComputerUserSearch : Search, IComputerUserSearch
    {
        public int CustomerId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DivisionId { get; set; }
        public int? DomainId { get; set; }
        public int? OuId { get; set; }
        public int? RegionId { get; set; }
        public int? StatusId { get; set; }
        public string SearchCompUs { get; set; }
    }
}
