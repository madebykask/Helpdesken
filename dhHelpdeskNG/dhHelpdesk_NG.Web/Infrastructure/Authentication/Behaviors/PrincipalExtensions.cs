using System.Security.Principal;
using System.Web.Security;
using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Common.Types;

namespace DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors
{
    public static class PrincipalExtensions
    {
        public static UserIdentity CreateHelpdeskUserIdentity(this WindowsIdentity identity)
        {
            UserIdentity userIdentity = null;
            if (identity != null)
            {
                var fullName = identity.Name;
                var userId = fullName.GetUserFromAdPath();
                var userDomain = fullName.GetDomainFromAdPath();
                
                //var initiator = _notifierRepository.GetInitiatorByUserId(userId, customerId, true);

                userIdentity = new UserIdentity(userId)
                {
                    Domain = userDomain,
                    //FirstName = initiator?.FirstName,
                    //LastName = initiator?.LastName,
                    //EmployeeNumber = string.Empty,
                    //Phone = initiator?.Phone,
                    //Email = initiator?.Email
                };
            }

            return userIdentity;
        }

        public static UserIdentity CreateHelpdeskUserIdentity(this FormsIdentity identity)
        {
            return new UserIdentity
            {
                UserId = identity.Name,
                //Domain = userDomain,
                //FirstName = initiator?.FirstName,
                //LastName = initiator?.LastName,
                //EmployeeNumber = string.Empty,
                //Phone = initiator?.Phone,
                //Email = initiator?.Email
            };
        }
    }
}