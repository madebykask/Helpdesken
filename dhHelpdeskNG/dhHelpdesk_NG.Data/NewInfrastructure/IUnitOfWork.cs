using System.Data.Entity.Infrastructure;

namespace DH.Helpdesk.Dal.NewInfrastructure
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<TSet> GetRepository<TSet>() where TSet : class;

        bool AutoDetectChangesEnabled { get; set; }

        void DetectChanges();

        void Save();
    }
}