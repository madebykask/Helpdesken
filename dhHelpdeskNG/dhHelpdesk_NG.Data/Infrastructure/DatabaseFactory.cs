namespace DH.Helpdesk.Dal.Infrastructure
{
    using DH.Helpdesk.Dal.DbContext;

    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private HelpdeskDbContext dataContext;

        public HelpdeskDbContext Get()
        {
            // TODO: Göra detta snyggare... 
            this.dataContext = this.dataContext ?? new HelpdeskSqlServerDbContext();

            if (!(this.dataContext is HelpdeskOracleDbContext))
            {
                return this.dataContext;
            }

            Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig config =
                Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            config.Workarounds.IgnoreSchemaName = true;
            config.Workarounds.DisableQuoting = true;

            return this.dataContext;
        }

        public void Run()
        {
            if (!(this.dataContext is HelpdeskOracleDbContext))
            {
                return;
            }

            Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig config =
                Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            config.Workarounds.IgnoreSchemaName = true;
            config.Workarounds.DisableQuoting = true;
        }

        protected override void DisposeCore()
        {
            if (this.dataContext != null)
            {
                this.dataContext.Dispose();
            }
        }
    }
}
