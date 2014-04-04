using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Users.Output;

namespace DH.Helpdesk.Web.Infrastructure.WorkContext
{
    public interface IUserContext
    {
        IEnumerable<UserModuleOverview> Modules { get; } 
    }
}