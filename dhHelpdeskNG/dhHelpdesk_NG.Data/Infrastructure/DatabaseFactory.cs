
namespace DH.Helpdesk.Dal.Infrastructure
{
    using DH.Helpdesk.Dal.DbContext;

    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private HelpdeskDbContext _dataContext;

        public HelpdeskDbContext Get()
        {
            // TODO: Göra detta snyggare... 

            this._dataContext = this._dataContext ?? new HelpdeskSqlServerDbContext();

            if(this._dataContext is HelpdeskOracleDbContext)
            {
                Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
                config.Workarounds.IgnoreSchemaName = true;
                config.Workarounds.DisableQuoting = true;
            }

            return this._dataContext;
        }

        public void Run()
        {
            if(this._dataContext is HelpdeskOracleDbContext)
            {
                Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
                config.Workarounds.IgnoreSchemaName = true;
                config.Workarounds.DisableQuoting = true;
            }
        }

        protected override void DisposeCore()
        {
            if(this._dataContext != null)
                this._dataContext.Dispose();
        }
    }
}
