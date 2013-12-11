
namespace dhHelpdesk_NG.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private HelpdeskDbContext _dataContext;

        public HelpdeskDbContext Get()
        {
            // TODO: Göra detta snyggare... 

            _dataContext = _dataContext ?? new HelpdeskSqlServerDbContext();

            if(_dataContext is HelpdeskOracleDbContext)
            {
                Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
                config.Workarounds.IgnoreSchemaName = true;
                config.Workarounds.DisableQuoting = true;
            }

            return _dataContext;
        }

        public void Run()
        {
            if(_dataContext is HelpdeskOracleDbContext)
            {
                Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
                config.Workarounds.IgnoreSchemaName = true;
                config.Workarounds.DisableQuoting = true;
            }
        }

        protected override void DisposeCore()
        {
            if(_dataContext != null)
                _dataContext.Dispose();
        }
    }
}
