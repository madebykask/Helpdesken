using System.Collections.Generic;

namespace DH.Helpdesk.Domain.Interfaces
{
    public interface IUserEntity
    {
        ICollection<User> Us { get; }  
    }
}