namespace DH.Helpdesk.Dal.NewInfrastructure.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> repositories;

        private IDbContext context;

        public UnitOfWork(IDbContext context)
        {
            this.repositories = new Dictionary<Type, object>();
            this.context = context;
        }

        public IRepository<TSet> GetRepository<TSet>() where TSet : class
        {
            if (this.repositories.Keys.Contains(typeof(TSet)))
            {
                return this.repositories[typeof(TSet)] as IRepository<TSet>;
            }

            var repository = new GenericRepository<TSet>(this.context);
            this.repositories.Add(typeof(TSet), repository);
            return repository;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                    this.context = null;
                }
            }
        }
    }
}