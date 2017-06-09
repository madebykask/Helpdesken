using System.Configuration;

namespace DH.Helpdesk.Dal.DbQueryExecutor
{
    public class SqlDbQueryExecutorFactory : IDbQueryExecutorFactory
    {
        public IDbQueryExecutor Create()
        {
            //todo: use constants or config file
            var connectionString = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;
            return new SqlDbQueryExecutor(connectionString);
        }
    }
}