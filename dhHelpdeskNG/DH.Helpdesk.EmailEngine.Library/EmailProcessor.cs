using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using DH.Helpdesk.Dal.DbContext;
using DH.Helpdesk.Domain;
using log4net;

namespace DH.Helpdesk.EmailEngine.Library
{
    public class EmailProcessor : IDisposable
    {
        private readonly HelpdeskSqlServerDbContext _context;
        private readonly Dictionary<int, Setting> _settings;
        private readonly ILog _logger;

        public EmailProcessor(ILog logger)
        {
            _context = new HelpdeskSqlServerDbContext();
            _settings = new Dictionary<int, Setting>();
            _logger = logger;
        }

        public void ProcessEmails()
        {
            _logger.Debug("ProcessEmails has been started.");

            var maxAttempts = int.Parse(ConfigurationManager.AppSettings["MaxAttempts"]);
            var query = from e in _context.EmailLogs
                        from eh in _context.CaseHistories.Where(w => w.Id == e.CaseHistory_Id).DefaultIfEmpty()
                        from el in _context.Logs.Where(w => w.Id == e.Log_Id).DefaultIfEmpty()
                        where e.SendStatus == EmailSendStatus.Pending && (e.Attempts == null || e.Attempts < maxAttempts)
                        select new { e, historyCustomer = (eh == null ? (int?)null : eh.Customer_Id), logCustomer = (el == null ? (int?)null : el.Case.Customer_Id) };

            var emails = query.OrderBy(x => x.e.Id).Take(1000).ToList();

            foreach (var email in emails)
            {
                _logger.Debug($"Processing email: Id={email.e.Id}, Subject={email.e.Subject}.");
                SendEmail(email.e, email.historyCustomer ?? email.logCustomer);
            }

            _logger.Info($"Emails sent: {emails.Count}");
            _context.Commit();
        }

        private void SendEmail(EmailLog email, int? customerId)
        {
            var sendTime = DateTime.Now;
            var attempt = new EmailLogAttempt { Date = sendTime };
            email.Attempts = email.Attempts.HasValue ? email.Attempts + 1 : 1;
            email.LastAttempt = sendTime;
            email.EmailLogAttempts.Add(attempt);

            try
            {
                if (!customerId.HasValue)
                    throw new Exception($"Cannot find CustomerId. EmailLog Id: {email.Id}");

                var setting = GetCustomerSetting(customerId.Value);
                if (setting == null)
                    throw new Exception($"Cannot find customer settings. EmailLog Id: {email.Id}, CustomerId: {customerId}");

                var smtpServer = setting.SMTPServer;
                var smtpPort = setting.SMTPPort;
                var smtpUser = setting.SMTPUserName;
                var smtpPassword = setting.SMTPPassWord;
                var smtpSsl = setting.IsSMTPSecured;

                if (string.IsNullOrEmpty(smtpServer) || smtpPort <= 0)
                {
                    smtpServer = ConfigurationManager.AppSettings["DefaultSmtpServer"];
                    smtpPort = int.Parse(ConfigurationManager.AppSettings["DefaultSmtpPort"]);
                    smtpUser = "";
                    smtpPassword = "";
                    smtpSsl = false;
                }

                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    if (!string.IsNullOrEmpty(smtpPassword))
                    {
                        smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                    }

                    smtpClient.EnableSsl = smtpSsl;

                    var mailMessage = new MailMessage
                    {
                        Subject = email.Subject,
                        Body = email.Body,
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(email.From)
                    };


                    if (!string.IsNullOrWhiteSpace(email.Cc))
                    {
                        var addresses = email.Cc.Split(',');
                        foreach (var address in addresses)
                        {
                            mailMessage.CC.Add(new MailAddress(address));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(email.Bcc))
                    {
                        var addresses = email.Bcc.Split(',');
                        foreach (var address in addresses)
                        {
                            mailMessage.Bcc.Add(new MailAddress(address));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(email.EmailAddress))
                    {
                        var addresses = email.EmailAddress.Split(',');
                        foreach (var address in addresses)
                        {
                            mailMessage.To.Add(new MailAddress(address));
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(email.MessageId))
                        mailMessage.Headers.Add("Message-ID", email.MessageId);

                    if (email.HighPriority)
                        mailMessage.Priority = MailPriority.High;

                    //Attach Files
                    if (!string.IsNullOrWhiteSpace(email.Files))
                    {
                        AttachFiles(mailMessage, email.Files);
                    }

                    //Attach Internal Files
                    if (!string.IsNullOrWhiteSpace(email.FilesInternal))
                    {
                        AttachFiles(mailMessage, email.FilesInternal);
                    }

                    smtpClient.Send(mailMessage);

                    _logger.Debug("Email has been sent successfuly!");
                    email.SendTime = sendTime;
                    email.SendStatus = EmailSendStatus.Sent;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error sending email. EmailLog Id: {email.Id}. ", ex);
                var errorMsg = new StringBuilder(ex.Message);
                var inner = ex.InnerException;
                while (inner != null)
                {
                    errorMsg.AppendLine(inner.Message);
                    inner = inner.InnerException;
                }
                attempt.Message = errorMsg.ToString();
            }
        }

        private void AttachFiles(MailMessage mailMessage, string filesString)
        {
            var files = filesString?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (files != null && files.Any())
            {
                foreach (var file in files)
                {
                    if (System.IO.File.Exists(file))
                        mailMessage.Attachments.Add(new Attachment(file));
                }
            }
        }

        private Setting GetCustomerSetting(int customerId)
        {
            if (_settings.ContainsKey(customerId))
                return _settings[customerId];

            var setting = _context.Settings.FirstOrDefault(x => x.Customer_Id == customerId);
            _settings[customerId] = setting;

            return setting;
        }

        #region IDisposable

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion IDisposable
    }
}
