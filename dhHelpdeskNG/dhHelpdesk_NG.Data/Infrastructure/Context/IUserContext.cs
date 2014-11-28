// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserContext.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IUserContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



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

        string UserName { get; }

        /// <summary>
        /// Gets the user working groups.
        /// </summary>
        ICollection<UserWorkingGroup> UserWorkingGroups { get; }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        IEnumerable<UserModuleOverview> Modules { get; }

        /// <summary>
        /// The refresh.
        /// </summary>
        void Refresh();

        bool IsUserEmpty();
    }
}