using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.MapperData.CaseHistory
{
    public class DepartmentMapperData
    {
        public int? Id { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string SearchKey { get; set; }
        public Country Country { get; set; }
    }
}