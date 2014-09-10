// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityAccess.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the EntityAccess type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Infrastructure.Security
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain.Interfaces;
    using System.Configuration;

    /// <summary>
    /// The entity access.
    /// </summary>
    public static class EntityAccess
    {
        /// <summary>
        /// The check access.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <typeparam name="TEntity">
        /// entity type
        /// </typeparam>
        /// <returns>
        /// The result.
        /// </returns>
        public static IEnumerable<TEntity> CheckAccess<TEntity>(this IEnumerable<TEntity> entities, IWorkContext context)
        {
            var app = ConfigurationManager.AppSettings["CurrentApplication"];
            if (app != null)
            {
                return entities;
            }

            if (context == null)
            {
                return entities;
            }

            if (entities == null)
            {
                return null;
            }

            if (typeof(IWorkingGroupEntity).IsAssignableFrom(typeof(TEntity)))
            {
                entities = entities
                    .Where(e =>
                    {
                        var workingGroups = ((IWorkingGroupEntity)e).WGs;

                        if (workingGroups == null || !workingGroups.Any())
                        {
                            return true;
                        }

                        
                        return workingGroups.Any(g => g != null  &&
                            context.User.UserWorkingGroups.Select(u => u.WorkingGroup_Id).Contains(g.Id));
            

                    });
            }

            if (typeof(ISingleWorkingGroupEntity).IsAssignableFrom(typeof(TEntity)))
            {
                entities = entities
                    .Where(e =>
                    {
                        var workingGroup = ((ISingleWorkingGroupEntity)e).WorkingGroup;

                        if (workingGroup == null)
                        {
                            return true;
                        }

                        return context.User.UserWorkingGroups.Select(u => u.WorkingGroup_Id).Contains(workingGroup.Id);
                    });
            }

            if (typeof(IUserEntity).IsAssignableFrom(typeof(TEntity)))
            {
                entities = entities
                    .Where(e =>
                    {
                        var users = ((IUserEntity)e).Us;

                        if (users == null || !users.Any())
                        {
                            return true;
                        }

                        return users.Any(u => u.Id == context.User.UserId);
                    });
            }

            return entities;
        } 
    }
}