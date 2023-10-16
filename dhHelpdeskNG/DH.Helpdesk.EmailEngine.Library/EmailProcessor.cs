using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
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
                var blockedEmails = setting.BlockedEmailRecipients;
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
                    smtpSsl = Convert.ToBoolean (ConfigurationManager.AppSettings["smtpSsl"]);// false;
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

                    AlternateView alterView = ContentToAlternateView(mailMessage.Body);
                    mailMessage.AlternateViews.Add(alterView);

                    if (!string.IsNullOrWhiteSpace(email.Cc))
                    {
                        var addresses = email.Cc.Split(',');
                        foreach (var address in addresses)
                        {
                            if(!IsBlockedRecipient(address, blockedEmails))
                            {
                                mailMessage.CC.Add(new MailAddress(address));
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(email.Bcc))
                    {
                        var addresses = email.Bcc.Split(',');
                        foreach (var address in addresses)
                        {
                            if (!IsBlockedRecipient(address, blockedEmails))
                            {
                                mailMessage.Bcc.Add(new MailAddress(address));
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(email.EmailAddress))
                    {
                        var addresses = email.EmailAddress.Split(',');

                        //
                        foreach (var address in addresses)
                        {

                            if (IsValidEmail(address) && !IsBlockedRecipient(address, blockedEmails))
                            {

                                mailMessage.To.Add(new MailAddress(address));

                            }
                            else
                            {
                                _logger.Debug("Email address: " + address + " is not valid");
                            }
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



        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool IsBlockedRecipient(string sEmail, string sBlockedEmailRecipients)
        {
            // Return false if sEmail or sBlockedEmailRecipients are empty or contain invalid characters
            if (string.IsNullOrWhiteSpace(sEmail) || string.IsNullOrWhiteSpace(sBlockedEmailRecipients))
            {
                return false;
            }

            // Split sBlockedEmailRecipients into an array of strings using the semicolon as a delimiter
            string[] emails = sBlockedEmailRecipients.Split(';');
            if (emails.Length == 0)
            {
                return false;
            }

            foreach (string pattern in emails)
            {
                if (!string.IsNullOrWhiteSpace(pattern))
                {
                    // Check if sEmail contains the pattern using a case-insensitive comparison
                    if (sEmail.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void AttachFiles(MailMessage mailMessage, string filesString)
        {
            //TODO - Change separator - se EmailService
            var files = filesString?.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
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
        private static AlternateView ContentToAlternateView(string content)
        {
            Stream Base64ToImageStream(string base64String)
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                return ms;
            }

            var imgCount = 0;
            List<LinkedResource> resourceCollection = new List<LinkedResource>();
            foreach (Match m in Regex.Matches(content, @"<img([\s\S]+?)src\s?=\s?['""](?<srcAttr>[^'""]+?)['""]([\s\S]+?)\/?>"))
            {
                imgCount++;
                var srcAttribute = m.Groups["srcAttr"].Value;
                string type = Regex.Match(srcAttribute, ":(?<type>.*?);base64,").Groups["type"].Value;
                string base64 = Regex.Match(srcAttribute, "base64,(?<base64>.*?)$").Groups["base64"].Value;

                //ignore replacement when match normal <img> tag
                if (String.IsNullOrEmpty(type) || String.IsNullOrEmpty(base64)) continue;

                content = content.Replace(srcAttribute, "cid:" + imgCount);

                var tempResource = new LinkedResource(Base64ToImageStream(base64), new ContentType(type))
                {
                    ContentId = imgCount.ToString()
                };
                resourceCollection.Add(tempResource);
            }

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            foreach (var item in resourceCollection)
            {
                alternateView.LinkedResources.Add(item);
            }

            return alternateView;
        }
        #region IDisposable

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion IDisposable
    }
}
