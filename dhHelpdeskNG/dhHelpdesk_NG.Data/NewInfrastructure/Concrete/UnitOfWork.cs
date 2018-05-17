using System.Data.Entity.Infrastructure;

namespace DH.Helpdesk.Dal.NewInfrastructure.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

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

        public bool AutoDetectChangesEnabled
        {
            get { return context.Configuration.AutoDetectChangesEnabled; }
            set { context.Configuration.AutoDetectChangesEnabled = value; }
        }

        public void DetectChanges()
        {
            this.context.ChangeTracker.DetectChanges();
        }

        public void Save()
        {
            try
            {
                this.context.SaveChanges();                
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        sb.AppendLine(string.Format(
                                                "Property: {0}; Value: {1}; Error message: {2};",
                                                validationError.PropertyName,
                                                validationErrors.Entry.Property(validationError.PropertyName).CurrentValue,
                                                validationError.ErrorMessage));
                    }
                }
             
                throw new Exception(sb.ToString());
            }
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