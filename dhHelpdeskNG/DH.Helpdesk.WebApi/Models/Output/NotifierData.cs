namespace DH.Helpdesk.WebApi.Models.Output
{
    public class NotifierData
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Place { get; set; }
        public string Phone { get; set; }
        public string UserCode { get; set; }
        public string Cellphone { get; set; }
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? OuId { get; set; }
        public string OuName { get; set; }
        public string CostCentre { get; set; }
    }
}