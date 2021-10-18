
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Security;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.BusinessData.Models.Employee;
using DH.Helpdesk.BusinessData.Models.Error;
using DH.Helpdesk.Web.Infrastructure.Configuration.Concrete;
using DH.Helpdesk.BusinessData.Models.Accounts;
using DH.Helpdesk.Common.Configuration;
using DH.Helpdesk.Common.Enums;
using System.Configuration;
using System.Linq;

namespace DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors
{
    public class MicrosoftAuthenticationBehavior : IAuthenticationBehavior 
    {
        private readonly IMasterDataService _masterDataService;
        public MicrosoftAuthenticationBehavior(IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }
        public UserIdentity CreateUserIdentityById(int userId)
        {
            return null;
        }
        public UserIdentity CreateUserIdentity(HttpContextBase ctx)
        {
            var userIdentity = ctx.User.Identity as ClaimsIdentity;
            var userClaims = userIdentity.Claims;
            var userEmail = userClaims.First(c => c.Type == "preferred_username").Value;
            string fullName = userClaims.First(c => c.Type == "name").Value;

            var lastError = new ErrorModel(string.Empty);

            var microsoftIdentity = TryMicrosoftLogin(userEmail, out lastError);

            return microsoftIdentity;
        }

        public void SignOut(HttpContextBase ctx)
        {
            // in windows mode a user can use login page to login with helpdesk credentials
            FormsAuthentication.SignOut();
        }

        public string GetLoginUrl()
        {
            return "/"; // for windows mode - redirect to any secured page so that a windows login would should up. Login page is not secured - anonymous;
        }
        private UserIdentity TryMicrosoftLogin(string emailAddress, out ErrorModel lastError)
        {
            lastError = null;
            var employeeNum = string.Empty;
            var user = _masterDataService.GetUserByEmail(emailAddress);

            var defaultEmployeeNumber = AppConfigHelper.GetAppSetting(AppSettingsKey.DefaultEmployeeNumber);
            if (!string.IsNullOrEmpty(defaultEmployeeNumber))
                employeeNum = defaultEmployeeNumber;

            var userIdentity = new UserIdentity()
            {
                UserId = user.UserID,
                Domain = "",
                FirstName = user?.FirstName,
                LastName = user?.SurName,
                EmployeeNumber = employeeNum,
                Phone = user?.Phone,
                Email = user?.Email ?? emailAddress
            };

            return userIdentity;
        }
    }
}