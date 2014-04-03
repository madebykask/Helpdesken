using System.Collections.Generic;
using System.Web;
using DH.Helpdesk.BusinessData.Models.Users.Output;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete
{
    internal sealed class UserContext : IUserContext
    {
        private const string _USER_MODULES = "_USER_MODULES";

        private readonly IUserService _userService;

        public UserContext(IUserService userService)
        {
            _userService = userService;
        }

        private IEnumerable<UserModuleOverview> _modules;        
        public IEnumerable<UserModuleOverview> Modules
        {
            get
            {
                if (_modules == null)
                {
                    _modules = (IEnumerable<UserModuleOverview>) HttpContext.Current.Session[_USER_MODULES];
                    if (_modules == null)
                    {
                        _modules = _userService.GetUserModules(SessionFacade.CurrentUser.Id);
                        HttpContext.Current.Session[_USER_MODULES] = _modules;
                    }
                }
                return _modules;
            }
            set
            {
                HttpContext.Current.Session[_USER_MODULES] = _modules = value;
            }
        }
    }
}