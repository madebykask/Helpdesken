namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class UserWorkingGroupOverview
    {
        public int UserRole { get; set; }
        public bool IsMemberOfGroup { get; set; }
        public int WorkingGroupId { get; set; }
        public int IsDefault { get; set; }
    }
}