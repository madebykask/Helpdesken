namespace DH.Helpdesk.BusinessData.Models.Case.CaseHistory
{
    public class PriorityOverview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IsActive { get; set; }
        public int IsDefault { get; set; }
        public string Code { get; set; }
    }
}