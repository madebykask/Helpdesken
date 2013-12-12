using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace dhHelpdesk_NG.Data
{
    public class HelpdeskSqlServerDbContext : HelpdeskDbContext
    {
        static HelpdeskSqlServerDbContext()
        {
            //  Database initialization strategies:
            System.Data.Entity.Database.SetInitializer<HelpdeskSqlServerDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
