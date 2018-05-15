using DH.Helpdesk.BusinessData.Models.User.Interfaces;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class UserBasicOvierview : IUserInfo
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
    }
}