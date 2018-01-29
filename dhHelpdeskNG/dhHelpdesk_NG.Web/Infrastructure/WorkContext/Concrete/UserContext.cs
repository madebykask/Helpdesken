// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserContext.cs" company="">
//   
// </copyright>
// <summary>
//   The user context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete
{
    using System.Collections.Generic;
    using System.Web;

    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.BusinessData.Models.Users.Output;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;

    /// <summary>
    /// The user context.
    /// </summary>
    internal sealed class UserContext : IUserContext
    {
        /// <summary>
        /// The user modules.
        /// </summary>
        private const string UserModules = "USER_CONTEXT_MODULES";

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly IUserService userService;

        private UserOverview user;

        /// <summary>
        /// The user id.
        /// </summary>
        private int? userId;
        
        /// <summary>
        /// The user working groups.
        /// </summary>
        private ICollection<UserWorkingGroup> userWorkingGroups;

        /// <summary>
        /// The modules.
        /// </summary>
        private IEnumerable<UserModuleOverview> modules;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContext"/> class.
        /// </summary>
        /// <param name="userService">
        /// The user service.
        /// </param>
        public UserContext(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public int UserId
        {
            get
            {
                if (!this.userId.HasValue)
                {
                    this.userId = this.User.Id;
                }

                return this.userId.Value;
            }
        }

        public string Login
        {
            get
            {
                if (this.User != null)
                {
                    return this.User.UserId;
                }

                return string.Empty;
            }
        }

        public string FirstName
        {
            get
            {
                if (this.User != null)
                {
                    return this.User.FirstName;
                }

                return string.Empty;
            }
        }

        public string LastName
        {
            get
            {
                if (this.User != null)
                {
                    return this.User.SurName;
                }

                return string.Empty;
            }
        }

        public string Phone
        {
            get
            {
                if (this.User != null)
                {
                    return this.User.Phone;
                }

                return string.Empty;
            }
        }

        public string Email
        {
            get
            {
                if (this.User != null)
                {
                    return this.User.Email;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the user working groups.
        /// </summary>
        public ICollection<UserWorkingGroup> UserWorkingGroups
        {
            get
            {
                if (this.userWorkingGroups == null)
                {
                    this.userWorkingGroups = this.User.UserWorkingGroups;
                }

                return this.userWorkingGroups;
            }
        }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        public IEnumerable<UserModuleOverview> Modules
        {
            get
            {
                if (this.modules == null)
                {
                    this.modules = (IEnumerable<UserModuleOverview>)HttpContext.Current.Session[UserModules];
                    if (this.modules == null)
                    {
                        HttpContext.Current.Session[UserModules] = this.modules = this.userService.GetUserModules(this.UserId);                            
                    }                    
                }

                return this.modules;
            }
        }
        
        private UserOverview User
        {
            get
            {
                if (this.user == null)
                {
                    this.user = SessionFacade.CurrentUser;
                }

                return this.user;
            }
        }

        /// <summary>
        /// Gets current user associated with customer
        /// </summary>
        public void SetCurrentUser(UserOverview userOverview)
        {
            SessionFacade.CurrentUser = userOverview;
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        public void Refresh()
        {
            this.userId = null;
            this.userWorkingGroups = null;
            HttpContext.Current.Session[UserModules] = this.modules = null;
            this.user = null;
            SessionFacade.CurrentUser = this.userService.GetUserOverview(SessionFacade.CurrentUser.Id);
        }

        public bool IsUserEmpty()
        {
            return this.User == null;
        }
    }
}