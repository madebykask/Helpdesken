using System.Data.Common;
using System.Data.Entity;

namespace dhHelpdesk_NG.Data
{
    public class HelpdeskOracleDbContext : HelpdeskDbContext
    {
        public HelpdeskOracleDbContext()
            : base()
        {
        }

        public HelpdeskOracleDbContext(DbConnection connection)
            : base(connection)
        {
        }

        static HelpdeskOracleDbContext()
        {
            //  Database initialization strategies:
            System.Data.Entity.Database.SetInitializer<HelpdeskOracleDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.ColumnTypeCasingConvention>();
        }
    }
}
