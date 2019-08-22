namespace DH.Helpdesk.BusinessData.Models.User
{
    public class CustomerUserForEdit
    {
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public bool UserInfoPermission { get; set; }
        public bool CaptionPermission { get; set; }
        public bool ContactBeforeActionPermission { get; set; }
        public bool PriorityPermission { get; set; }
        public bool StateSecondaryPermission { get; set; }
        public bool WatchDatePermission { get; set; }
        public bool RestrictedCasePermission { get; set; }
    }
}