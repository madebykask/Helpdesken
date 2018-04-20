namespace DH.Helpdesk.Dal.NewInfrastructure
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;

        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        DbContextConfiguration Configuration { get; }

        DbChangeTracker ChangeTracker { get; }

        int SaveChanges();

        void Dispose();
    }
}
