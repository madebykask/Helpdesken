namespace DH.Helpdesk.Dal.Infrastructure
{
    using System;

    [Obsolete]
    public interface IUnitOfWork
    {
        void Commit();
    }
}
