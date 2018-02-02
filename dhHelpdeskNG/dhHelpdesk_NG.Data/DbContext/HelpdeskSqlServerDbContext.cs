namespace DH.Helpdesk.Dal.DbContext
{
	using System.Data.Common;
	using System.Data.Entity;

	public class HelpdeskSqlServerDbContext : HelpdeskDbContext
    {

		public HelpdeskSqlServerDbContext() : base()
		{

		}

		public HelpdeskSqlServerDbContext(DbConnection connection) : base(connection)
		{

		}
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
