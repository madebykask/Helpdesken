namespace DH.Helpdesk.BusinessData.Models.Customer
{
    public class CaseDefaultsInfo
    {
        public int? RegionId { get; set; }
        public int CaseTypeId { get; set; }
        public int EmailCaseTypeId { get; set; }
        public int? SupplierId { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
    }
}