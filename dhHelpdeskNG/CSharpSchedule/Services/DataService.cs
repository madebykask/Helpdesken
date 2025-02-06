using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpSchedule.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task<List<string>> GetUsersAsync()
        {
            var users = new List<string>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    _logger.LogInformation("Database connection opened.");

                    string query = "select top 10 EmailAddress from tblEMailLog order by id desc";
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users from database.");
            }

            return users;
        }

        public async Task InsertUserAsync(string name)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    _logger.LogInformation("Database connection opened.");

                    string query = "INSERT INTO Users (Name) VALUES (@Name)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                _logger.LogInformation("User {Name} inserted into database.", name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting user into database.");
            }
        }
    }
}
