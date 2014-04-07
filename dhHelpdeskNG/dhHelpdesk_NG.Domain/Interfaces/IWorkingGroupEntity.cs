using System.Collections.Generic;

namespace DH.Helpdesk.Domain.Interfaces
{
    public interface IWorkingGroupEntity
    {
        ICollection<WorkingGroupEntity> WGs { get; } 
    }
}