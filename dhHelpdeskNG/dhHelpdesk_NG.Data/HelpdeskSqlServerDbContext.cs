using System.Data.Entity;

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
            base.OnModelCreating(modelBuilder);
        }
    }
}
