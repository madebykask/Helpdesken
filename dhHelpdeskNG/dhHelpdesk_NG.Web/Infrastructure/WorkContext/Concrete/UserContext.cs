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
        /// The user service.
        /// </summary>
        private readonly IUserService userService;

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
            get { return SessionFacade.CurrentUser.Id; }
        }

        /// <summary>
        /// Gets the user working groups.
        /// </summary>
        public ICollection<UserWorkingGroup> UserWorkingGroups
        {
            get { return SessionFacade.CurrentUser.UserWorkingGroups; }
        }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        public IEnumerable<UserModuleOverview> Modules
        {
            get
            {
                return this.userService.GetUserModules(this.UserId);
            }
        }
    }
}