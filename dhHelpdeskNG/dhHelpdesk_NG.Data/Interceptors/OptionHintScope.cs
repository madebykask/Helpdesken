using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SqlClient;
using System.Linq;

namespace DH.Helpdesk.Dal.Interceptors
{
    /// <summary>
    /// https://stackoverflow.com/questions/47093440/major-performance-difference-between-entity-framework-generated-sp-executesql-an?rq=1
    /// </summary>
    public class OptionHintScope : IDisposable
    {
        private readonly OptionHintDbCommandInterceptor _interceptor;

        public OptionHintScope(System.Data.Entity.DbContext context)
        {
            _interceptor = new OptionHintDbCommandInterceptor(context);
            DbInterception.Add(_interceptor);
        }

        public void Dispose()
        {
            DbInterception.Remove(_interceptor);
        }

        private class OptionHintDbCommandInterceptor : IDbCommandInterceptor
        {
            private readonly System.Data.Entity.DbContext _dbContext;

            internal OptionHintDbCommandInterceptor(System.Data.Entity.DbContext dbContext)
            {
                this._dbContext = dbContext;
            }

            public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
            {
            }

            public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
            {
            }

            public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
            {
                if (ShouldIntercept(command, interceptionContext))
                {
                    AddOptionHint(command);
                }
            }

            public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
            {
            }

            public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
            {
                if (ShouldIntercept(command, interceptionContext))
                {
                    AddOptionHint(command);
                }
            }

            public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
            {
            }

            private static void AddOptionHint(IDbCommand command)
            {
                var queryHint = " OPTION (OPTIMIZE FOR UNKNOWN)";
                if (!command.CommandText.EndsWith(queryHint))
                {
                    command.CommandText += queryHint;
                }
            }

            private bool ShouldIntercept(IDbCommand command, DbCommandInterceptionContext interceptionContext)
            {
                return
                    command.CommandType == CommandType.Text &&
                    command is SqlCommand &&
                    interceptionContext.DbContexts.Any(interceptionDbContext => ReferenceEquals(interceptionDbContext, _dbContext));
            }
        }
    }
}
