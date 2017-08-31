using DH.Helpdesk.BusinessData.Models.User.Interfaces;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class DepartmentOverview : IDepartmentInfo
    {
        public int? Id { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string SearchKey { get; set; }
        public string CountryName { get; set; }
    }
}