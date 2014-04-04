using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Users.Output;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete
{
    internal sealed class UserContext : IUserContext
    {
        private readonly IUserService _userService;

        public UserContext(IUserService userService)
        {
            _userService = userService;
        }

        public IEnumerable<UserModuleOverview> Modules
        {
            get
            {
                return _userService.GetUserModules(SessionFacade.CurrentUser.Id);
            }
        }
    }
}