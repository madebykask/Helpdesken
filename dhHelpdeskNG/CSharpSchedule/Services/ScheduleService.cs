using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpSchedule.Services
{
    public class ScheduleService
    {
        private readonly string _connectionString;
        private readonly ILogger<ScheduleService> _logger;
        public ScheduleService(IConfiguration configuration, ILogger<ScheduleService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }
        public async Task<List<string>> GetSchedulesAsync()
        {
            var schedules = new List<string>();
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
                            schedules.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving schedules from database.");
            }
            return schedules;
        }
    }
   
}
