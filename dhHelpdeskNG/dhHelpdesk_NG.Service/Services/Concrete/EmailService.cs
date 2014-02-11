namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Configuration;
    using System.Net.Mail;
    using System.Text.RegularExpressions;

    public class EmailService : IEmailService
    {

        public void SendEmail(string from, string to, string subject, string body, string mailMessageId, bool highPriority = false)
        {
            SmtpClient _smtpClient;

            try
            {
                string smtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string smtpPort = ConfigurationManager.AppSettings["SmtpPort"].ToString(); 

                if (string.IsNullOrWhiteSpace(smtpServer))
                {
                    throw new Exception("SMTP server is missing");
                }
                else if (string.IsNullOrWhiteSpace(from))
                {
                    throw new Exception("Email from address is missing");
                }
                else
                {
                    int port;
                    if (int.TryParse(smtpPort, out port))
                        _smtpClient = new SmtpClient(smtpServer, port);
                    else
                        _smtpClient = new SmtpClient(smtpServer);

                    MailMessage msg = new MailMessage();
                    string[] strTo = to.Split(new Char[] { ';' });

                    for (int i = 0; i < strTo.Length; i++)
                        if (IsValidEmail(strTo[i]))
                            msg.To.Add(new MailAddress(strTo[i]));

                    if (highPriority) 
                        msg.Priority = MailPriority.High;  
                    msg.Subject = subject;
                    msg.From = new MailAddress(from);
                    msg.IsBodyHtml = true;
                    msg.BodyEncoding = System.Text.Encoding.UTF8;
                    msg.Body = body;

                    _smtpClient.Send(msg);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                _smtpClient = null;
            }
        }

        public string GetMailMessageId(string helpdeskFromAddress)
        {
            return "<"  + DateTime.Now.Year.ToString() 
                        + DateTime.Now.Month.ToString().PadLeft(2, '0')  
                        + DateTime.Now.Day.ToString().PadLeft(2, '0')  
                        + DateTime.Now.Hour.ToString().PadLeft(2, '0')  
                        + DateTime.Now.Minute.ToString().PadLeft(2, '0')  
                        + DateTime.Now.Second.ToString().PadLeft(2, '0')
                        + Guid.NewGuid().ToString().Substring(0, 8)    
                        + "@"
                        + helpdeskFromAddress.Replace('@', '.')   
                    + ">";
        }

        public bool IsValidEmail(string inputEmail)
        {
            string strEmail = string.Empty;

            if (string.IsNullOrEmpty(inputEmail) == false)
                strEmail = inputEmail;

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            Regex re = new Regex(strRegex);
            if (re.IsMatch(strEmail))
                return (true);
            else
                return (false);
        }

    }
}
