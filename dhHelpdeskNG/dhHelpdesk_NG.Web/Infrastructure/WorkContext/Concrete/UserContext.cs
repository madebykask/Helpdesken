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
                    this.userId = SessionFacade.CurrentUser.Id;
                }

                return this.userId.Value;
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
                    this.userWorkingGroups = SessionFacade.CurrentUser.UserWorkingGroups;
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

        /// <summary>
        /// The refresh.
        /// </summary>
        public void Refresh()
        {
            this.userId = null;
            this.userWorkingGroups = null;
            HttpContext.Current.Session[UserModules] = this.modules = null;
        }
    }
}