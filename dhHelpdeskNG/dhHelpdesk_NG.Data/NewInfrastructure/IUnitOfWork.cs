namespace DH.Helpdesk.Dal.NewInfrastructure
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<TSet> GetRepository<TSet>() where TSet : class;

        void Save();
    }
}