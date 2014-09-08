using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Users.Output;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.NewSelfService.Infrastructure.WorkContext.Concrete
{
    using System;

    using DH.Helpdesk.NewSelfService.Infrastructure;

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

        public string UserName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<UserWorkingGroup> UserWorkingGroups
        {
            //get { return SessionFacade.CurrentUser.UserWorkingGroups; }
            get { return null; }

        }

        public IEnumerable<UserModuleOverview> Modules
        {
            get
            {
                return _userService.GetUserModules(UserId);
            }
        }

        public void Refresh()
        {
            throw new System.NotImplementedException();
        }
    }
}