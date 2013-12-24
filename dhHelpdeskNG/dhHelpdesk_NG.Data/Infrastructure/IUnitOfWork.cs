namespace dhHelpdesk_NG.Data.Infrastructure
{
    using System;

    [Obsolete]
    public interface IUnitOfWork
    {
        void Commit();
    }
}
