namespace DH.Helpdesk.BusinessData.Models.User
{
    public class UserLoginInfo
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public int UserGroupId { get; set; }
    }
}