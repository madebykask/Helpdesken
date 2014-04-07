using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Users.Output;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Infrastructure.Context
{
    public interface IUserContext
    {
        int UserId { get; }
        ICollection<UserWorkingGroup> UserWorkingGroups { get; }
        IEnumerable<UserModuleOverview> Modules { get; } 
    }
}