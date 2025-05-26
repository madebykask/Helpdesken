using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Data;

namespace DH.Helpdesk.CaseSolutionScheduleYearly.Services
{
    public class ScheduleService
    {
        private readonly string _connectionString;
        public ScheduleService(string connection)
        {
            _connectionString = connection;
        }
        public async Task<List<CaseScheduleItem>> GetSchedulesAsync(DateTime now)
        {
            var result = new List<CaseScheduleItem>();

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var cmd = new SqlCommand(@"
                        SELECT CaseSolution_Id, NextRun, LastExecuted, ScheduleTime, RepeatType, RepeatInterval, StartYear, 
                               ScheduleMonthlyDay, ScheduleMonthlyOrder, ScheduleMonthlyWeekday, DaysOfWeek, ScheduleMonths
                        FROM dbo.tblCaseSolutionSchedule
                        WHERE 
                            NextRun IS NOT NULL
                            AND NextRun <= @Now
                            AND RepeatType = 'EveryXYears' 
                            AND (LastExecuted IS NULL OR LastExecuted < NextRun)
                    ", conn);

                cmd.Parameters.AddWithValue("@Now", now);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new CaseScheduleItem
                        {
                            CaseSolutionId = reader.GetInt32(0),
                            NextRun = reader.GetDateTime(1),
                            LastExecuted = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                            ScheduleTime = reader.GetDecimal(3),
                            RepeatType = reader.GetString(4),
                            RepeatInterval = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            StartYear = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                            ScheduleMonthlyDay = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7),
                            ScheduleMonthlyOrder = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                            ScheduleMonthlyWeekday = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                            DaysOfWeek = reader.IsDBNull(10) ? null : reader.GetString(10),
                            ScheduleMonths = reader.IsDBNull(11) ? null : reader.GetString(11)
                        });
                    }
                }
            }

           Log.Information("Loaded {Count} schedules due for execution", result.Count);
            return result;
        }
        public async Task UpdateScheduleExecutionAsync(CaseScheduleItem item, DateTime executed)
        {
            var next = ScheduleCalculator.CalculateNextRunYearly(item, executed); // 🟢 Beräkna nästa körning

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var cmd = new SqlCommand(@"
                        UPDATE dbo.tblCaseSolutionSchedule
                        SET LastExecuted = @Executed,
                            NextRun = @NextRun
                        WHERE CaseSolution_Id = @Id
                    ", conn);

                cmd.Parameters.AddWithValue("@Executed", executed);
                cmd.Parameters.AddWithValue("@NextRun", next.HasValue ? (object)next.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", item.CaseSolutionId);

                await cmd.ExecuteNonQueryAsync();
            }

            Log.Information("Updated schedule {Id} with LastExecuted={Executed} and NextRun={NextRun}",
                item.CaseSolutionId, executed, next);
        }


    }


}
