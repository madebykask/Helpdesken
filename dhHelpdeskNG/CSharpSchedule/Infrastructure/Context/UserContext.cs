using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.BusinessData.Models.Users.Output;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;

namespace DH.Helpdesk.CaseSolutionYearly.Infrastructure.Context
{
    internal sealed class UserContext : IUserContext
    {
        /// <summary>
        /// The user modules.
        /// </summary>
        private const string UserModules = "USER_CONTEXT_MODULES";

        private readonly IUserService _userService;
        private UserOverview _user;
        private int? _userId;
        private ICollection<UserWorkingGroup> _userWorkingGroups;
        //private IEnumerable<UserModuleOverview> _modules;

        public UserContext(IUserService userService)
        {
            _userService = userService;
        }

        public int UserId
        {
            get
            {
                if (!_userId.HasValue)
                {
                    _userId = User.Id;
                }

                return _userId.Value;
            }
        }

        public string Login
        {
            get
            {
                if (User != null)
                {
                    return User.UserId;
                }

                return string.Empty;
            }
        }

        public string FirstName
        {
            get
            {
                if (User != null)
                {
                    return User.FirstName;
                }

                return string.Empty;
            }
        }

        public string LastName
        {
            get
            {
                if (User != null)
                {
                    return User.SurName;
                }

                return string.Empty;
            }
        }

        public string Phone
        {
            get
            {
                if (User != null)
                {
                    return User.Phone;
                }

                return string.Empty;
            }
        }

        public string Email
        {
            get
            {
                if (User != null)
                {
                    return User.Email;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the user working groups.
        /// </summary>
        public ICollection<UserWorkingGroup> UserWorkingGroups
        {
            get { return _userWorkingGroups ?? (_userWorkingGroups = User.UserWorkingGroups); }
        }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        public IEnumerable<UserModuleOverview> Modules
        {
            get
            {
                //TODO: Review if used. Use cache instead of session if used.
                //if (_modules == null)
                //{
                //    _modules = (IEnumerable<UserModuleOverview>)HttpContext.Current.Session[UserModules];
                //    if (_modules == null)
                //    {
                //        HttpContext.Current.Session[UserModules] = _modules = _userService.GetUserModules(UserId);                            
                //    }                    
                //} 
                //return _modules

                throw new NotImplementedException();
            }
        }

        private UserOverview User
        {
            get
            {
                throw new NotImplementedException();
                //TODO: Review if used. Use cache/claims instead of session if used.
                //return _user ?? (_user = SessionFacade.CurrentUser);
            }
        }

        /// <summary>
        /// Gets current user associated with customer
        /// </summary>
        public void SetCurrentUser(UserOverview userOverview)
        {
            //TODO: Review if used. Use cache instead of session if used.
            //SessionFacade.CurrentUser = userOverview;
            throw new NotImplementedException();
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        public void Refresh()
        {
            _userId = null;
            _userWorkingGroups = null;

            //TODO: Review if used. Use cache instead of session if used.
            //HttpContext.Current.Session[UserModules] = _modules = null;
            _user = null;
            ////TODO: Review if used. Use cache instead of session if used.
            //SessionFacade.CurrentUser = _userService.GetUserOverview(SessionFacade.CurrentUser.Id);
        }

        public bool IsUserEmpty()
        {
            return User == null;
        }
    }
}
