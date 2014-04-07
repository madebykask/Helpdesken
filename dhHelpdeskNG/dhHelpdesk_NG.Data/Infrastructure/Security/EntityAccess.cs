using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Domain.Interfaces;

namespace DH.Helpdesk.Dal.Infrastructure.Security
{
    public static class EntityAccess
    {
        public static IEnumerable<TEntity>CheckAccess<TEntity>(this IEnumerable<TEntity> entities, IWorkContext context)
        {
            if (context == null)
                return entities;

            if (entities == null)
                return null;

            if (typeof(IWorkingGroupEntity).IsAssignableFrom(typeof(TEntity)))
            {
                entities = entities
                    .Where(e =>
                    {
                        var workingGroups = ((IWorkingGroupEntity) e).WGs;
                        
                        if (workingGroups == null || !workingGroups.Any())
                            return true;

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
                            return true;

                        return context.User.UserWorkingGroups.Select(u => u.WorkingGroup_Id).Contains(workingGroup.Id);
                    });
            }

            if (typeof(IUserEntity).IsAssignableFrom(typeof(TEntity)))
            {
                entities = entities
                    .Where(e =>
                    {
                        var users = ((IUserEntity) e).Us;

                        if (users == null || !users.Any())
                            return true;

                        return users.Any(u => u.Id == context.User.UserId);
                    });
            }

            return entities;
        } 
    }
}