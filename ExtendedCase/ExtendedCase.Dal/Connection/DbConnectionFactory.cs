using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ExtendedCase.Dal.Connection
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        #region GetConnection
        
        public IDbConnection GetConnection(string connectionStringName)
        {
            if (string.IsNullOrWhiteSpace(connectionStringName))
                throw new ArgumentNullException(nameof(connectionStringName));

            return CreateConnection(connectionStringName);
        }

        #endregion

        #region Helper Methods

        private SqlConnection CreateConnection(string connectionStringName)
        {
            var connectionString = GetConnectionString(connectionStringName);

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception($"Connection string '{connectionStringName}' is null or empty");

            return new SqlConnection(connectionString);
        }

        private string GetConnectionString(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName]?.ToString();
        }

        #endregion
    }

    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection(string connectionStringName);
    }
}
