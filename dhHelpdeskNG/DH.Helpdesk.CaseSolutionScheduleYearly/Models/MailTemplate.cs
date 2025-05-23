using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DH.Helpdesk.CaseSolutionScheduleYearly.Models
{
    public class MailTemplate
    {
        public int MailId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int SendMethod { get; set; }
        public bool IncludeLogText_External { get; set; }

        // Keep existing constructor
        public MailTemplate(System.Data.DataRow row)
        {
            MailId = Convert.ToInt32(row["MailId"]);
            Subject = row["Subject"].ToString();
            Body = row["Body"].ToString();
            SendMethod = Convert.ToInt32(row["SendMethod"]);
            IncludeLogText_External = Convert.ToBoolean(row["IncludeLogText_External"]);
        }

        // Add constructor for SqlDataReader
        public MailTemplate(SqlDataReader reader)
        {
            MailId = reader.GetInt32(reader.GetOrdinal("MailId"));
            Subject = reader.GetString(reader.GetOrdinal("Subject"));
            Body = reader.GetString(reader.GetOrdinal("Body"));
            SendMethod = reader.GetInt32(reader.GetOrdinal("SendMethod"));
            IncludeLogText_External = reader.GetBoolean(reader.GetOrdinal("IncludeLogText_External"));
        }
    }
}
