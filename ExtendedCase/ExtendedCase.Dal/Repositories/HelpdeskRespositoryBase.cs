using System.Collections.Generic;
using System.Linq;
using ExtendedCase.Dal.Connection;

namespace ExtendedCase.Dal.Repositories
{
    public abstract class HelpdeskRespositoryBase : DapperRepositoryBase
    {
        #region ctor()

        protected HelpdeskRespositoryBase(IDbConnectionFactory connectionFactory) 
            : base(DbConnections.HelpDeskDb, connectionFactory)
        {
        }
        
        #endregion

        #region Build Query Methods

        protected string BuildTableQuery(string tableName, List<string> columns, IDictionary<string, string> parameters)
        {
            const string sql = @"
                SELECT {0} 
                FROM   {1}
                WHERE  {2}
            ";

            var cols = BuildSelectColumns(columns);
            var where = BuildWhereExpression(parameters);

            var sqlText = string.Format(sql, cols, tableName, where);
            return sqlText;
        }

        protected string BuildOptionsTableQuery(string tableName, string idColumn, string valueColumn, IDictionary<string, string> parameters)
        {
            const string sql = @"
                SELECT {0} AS Value, {1} AS Text 
                FROM {2}
                WHERE {3}
            ";

            var where = BuildWhereExpression(parameters);
            var s = string.Format(sql, idColumn, valueColumn, tableName, where);
            return s;
        }

        protected string BuildSelectColumns(IList<string> columns)
        {
            var output = string.Join(",  ", columns);

            if (string.IsNullOrWhiteSpace(output))
                output = "*";

            return output;
        }

        protected string BuildWhereExpression(IDictionary<string, string> parameters, string  conditionOperator = "AND")
        {
            var where = string.Join($" {conditionOperator} ", parameters.Select(x => x.Value == null ? $"{x.Key} IS NULL" : $"{x.Key} = @{x.Key}"));
            return where;
        }

        #endregion
    }
}