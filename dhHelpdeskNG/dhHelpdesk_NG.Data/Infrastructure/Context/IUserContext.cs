// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserContext.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IUserContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using DH.Helpdesk.BusinessData.Models.User.Input;

namespace DH.Helpdesk.Dal.Infrastructure.Context
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Users.Output;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The UserContext interface.
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// Gets the user id.
        /// </summary>
        int UserId { get; }

        string Login { get; }

        string FirstName { get; }

        string LastName { get; }
        string Phone { get; }
        string Email { get; }

        /// <summary>
        /// Gets the user working groups.
        /// </summary>
        ICollection<UserWorkingGroup> UserWorkingGroups { get; }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        IEnumerable<UserModuleOverview> Modules { get; }

        /// <summary>
        /// Sets customer user
        /// </summary>
        void SetCurrentUser(UserOverview user);

        /// <summary>
        /// The refresh.
        /// </summary>
        void Refresh();

        bool IsUserEmpty();
    }
}