namespace DH.Helpdesk.BusinessData.Models.User.Interfaces
{
    public class DepartmentInfoAdapter : IDepartmentInfo
    {
        private readonly Domain.Department _department;

        public DepartmentInfoAdapter(Domain.Department d)
        {
            DepartmentId = d.DepartmentId;
            DepartmentName = d.DepartmentName;
            SearchKey = d.SearchKey;
            CountryName = d.Country?.Name;
        }

        public string DepartmentId { get; }
        public string DepartmentName { get; set; }
        public string SearchKey { get; set; }
        public string CountryName { get; set; }
    }
}