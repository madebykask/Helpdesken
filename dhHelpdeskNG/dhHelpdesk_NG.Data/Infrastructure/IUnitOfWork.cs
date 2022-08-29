namespace DH.Helpdesk.Dal.Infrastructure
{
    using System;


    [Obsolete("Use transactions insted of this.")]
    public interface IUnitOfWork
    {
        void Commit();
    }
}
