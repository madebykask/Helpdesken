using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using DH.Helpdesk.LicenseReporter.Entities;
using DH.Helpdesk.LicenseReporter.Helpers;

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

            //Result
            string body = "";

            //Get all customers
            var customers = GetCustomers();

            //Filter out customers where address is excluded
            customers = customers.Where(x => !x.Address.Contains("licensefree")).ToList();

            foreach (var customer in customers)
            {
                var users = GetUsersByCustomer(customer.Id);

                //Exclude DH SOlutons email.
                users = users.Where(x => !x.Email.Contains("dhsolution")).ToList();

                if (string.IsNullOrEmpty(customer.ERPContractNumber))
                {
                    body = body + customer.Name + " " + "<b>" + users.Count + "</b>" + " st" + "<br>";
                }

                else
                {
                    body = body + customer.ERPContractNumber + " - " + customer.Name + " " + "<b>" + users.Count + "</b>" + " st" + "<br>";
                }

                

            }


            SendEmail(body);
        }
        
        public static List<User> GetUsersByCustomer(int id)
        {
            var users = new List<User>();

            string query = "select * from tblUsers where [Status] = 1 and Customer_Id = @Id";

            using (SqlConnection connection = new SqlConnection($"Data Source={dbServerInstance};Initial Catalog={dbDatabase};User Id={dbUsername};Password={dbPassword};"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection) { CommandType = CommandType.Text })
                {

                    command.Parameters.AddWithValue("@Id", id);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var user = new User()
                        {
                            Id = reader.SafeGetInteger("Id"),
                            Email = reader.SafeGetString("Email")
                            
                        };

                        users.Add(user);

                    }
                }


            }

            return users;
        }

        public static List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();

            string query = "select * from tblCustomer where Status = 1 order by ERPContractNumber,Name asc";

            using (SqlConnection connection = new SqlConnection($"Data Source={dbServerInstance};Initial Catalog={dbDatabase};User Id={dbUsername};Password={dbPassword};"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection) { CommandType = CommandType.Text })
                {
                    var reader =  command.ExecuteReader();

                    while (reader.Read())
                    {
                        var customer  = new Customer()
                        {
                            Id = reader.SafeGetInteger("Id"),
                            Name = reader.SafeGetString("Name"),
                            Address = reader.SafeGetString("Address"),
                            ERPContractNumber = reader.SafeGetString("ERPContractNumber")
                        };

                        customers.Add(customer);

                    }
                }


            }

            return customers;

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

            try
            {
                using (SmtpClient smtp = smtpPort == 0 ? new SmtpClient(smtpServer) : new SmtpClient(smtpServer, smtpPort))
                {
                    smtp.Send(mail);
                    Console.WriteLine("Email sent successfully");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());

                // Log exception to a text file at the same location as the exe file
                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
                using (StreamWriter sw = new StreamWriter(logFilePath, true)) // true to append text
                {
                    sw.WriteLine($"[{DateTime.Now}] Failed to send email. Exception: {ex.ToString()}");
                }
            }
        }
    }
}
