using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Users.Output;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.SelfService.Infrastructure.WorkContext.Concrete
{
    using DH.Helpdesk.SelfService.Infrastructure;

    internal sealed class UserContext : IUserContext
    {
        private readonly IUserService _userService;

        public UserContext(IUserService userService)
        {
            _userService = userService;
        }

        public int UserId
        {
            get { return SessionFacade.CurrentUser.Id; }
        }

        public ICollection<UserWorkingGroup> UserWorkingGroups
        {
            get { return SessionFacade.CurrentUser.UserWorkingGroups; }
        }

        public IEnumerable<UserModuleOverview> Modules
        {
            get
            {
                return _userService.GetUserModules(UserId);
            }
        }
    }
}