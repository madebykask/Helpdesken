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
            if (entities == null)
                return null;

            if (typeof(IWorkingGroupEntity).IsAssignableFrom(typeof(TEntity)))
            {
                entities = entities
                    .Where(e => !((IWorkingGroupEntity)e).WGs.Any() ||
                                ((IWorkingGroupEntity)e).WGs
                                .Any(g => context.User.UserWorkingGroups.Select(u => u.WorkingGroup_Id).Contains(g.Id)));
            }

            if (typeof(IUserEntity).IsAssignableFrom(typeof(TEntity)))
            {
                entities = entities
                    .Where(e => !((IUserEntity)e).Us.Any() ||
                        ((IUserEntity) e).Us
                        .Any(u => u.Id == context.User.UserId));
            }

            return entities;
        } 
    }
}