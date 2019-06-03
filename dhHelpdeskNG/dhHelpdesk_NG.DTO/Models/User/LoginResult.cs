using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.User.Input;

namespace DH.Helpdesk.BusinessData.Models.User
{
    public class LoginResult
    {
        public bool IsSuccess { get; set; }
        public TimeZoneAutodetectResult TimeZoneAutodetect { get; set; }
        public bool PasswordExpired { get; set; }
        public string ErrorMessage { get; set; }
        public UserOverview User { get; set; }

        #region Factory Methods

        public static LoginResult Success(UserOverview user)
        {
            return new LoginResult()
            {
                IsSuccess = true,
                User = user
            };
        }

        public static LoginResult Failed(string errorMessage)
        {
            return new LoginResult()
            {
                ErrorMessage = errorMessage
            };
        }

        #endregion
    }
}