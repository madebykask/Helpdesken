using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;

namespace DH.Helpdesk.LicenseReporter
{
    internal class Program
    {

        private static string dbServerInstance = ConfigurationManager.AppSettings["dbServerInstance"];
        private static string dbDatabase = ConfigurationManager.AppSettings["dbDatabase"];
        private static string dbUsername = ConfigurationManager.AppSettings["dbUsername"];
        private static string dbPassword = ConfigurationManager.AppSettings["dbPassword"];

        static void Main(string[] args)
        {
            string body = "";
            body += GetCountLicenses(ConfigurationManager.AppSettings["customerName"]);
            body += "<br>";

            SendEmail(body);
        }
        
        public static string GetCountLicenses(string customer)
        {
            string customerDB = (customer == "Logisnext") ? "'Logisnext IT Servicedesk' or tblCustomer.name = 'Parts support'" : $"'{customer}'";
            string query = $@"select count (tblUsers.email) AS 'customer' from tblUsers INNER JOIN tblCustomer ON tblUsers.Customer_Id = tblCustomer.Id where (tblCustomer.name = {customerDB}) and email not like '%dhsolutions%' and tblUsers.Status=1";

            using (SqlConnection conn = new SqlConnection($"Data Source={dbServerInstance};Initial Catalog={dbDatabase};User Id={dbUsername};Password={dbPassword};"))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                int number = Convert.ToInt32(dt.Rows[0]["customer"].ToString());
                string minLicense = "";

                if (customer == "LinSon" && number < 5)
                {
                    number = 5;
                }
                if (number < 7)
                {
                    number = 7;
                }
                if (customer == "Logisnext" && number > 50)
                {
                    number = 50;
                }

                return $"{customer}: <b>{number}</b>st";
            }
        }


        public static void SendEmail(string body)
        {
            string from = ConfigurationManager.AppSettings["fromEmail"];
            string[] to = ConfigurationManager.AppSettings["toEmails"].Split(',');
            string subject = ConfigurationManager.AppSettings["emailSubject"];
            string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            foreach (string recipient in to)
            {
                mail.To.Add(recipient);
            }

            using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
            {
                smtp.Send(mail);
            }
        }
    }
}
