using System.Collections.Generic;
using System.Data;

namespace DH.Helpdesk.Dal.DbQueryExecutor
{
    public interface IDbQueryExecutor
    {
        int ExecQuery(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60);

        TEntity QuerySingle<TEntity>(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60);
        IList<TEntity> QueryList<TEntity>(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60);
        TValue ExecuteScalar<TValue>(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60);
        DataTable ExecuteTable(string query, object parameters = null, CommandType commandType = CommandType.Text, int timeout = 60);
    }
}