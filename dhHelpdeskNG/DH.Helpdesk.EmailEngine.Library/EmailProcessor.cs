using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using DH.Helpdesk.Dal.DbContext;
using DH.Helpdesk.Domain;
using log4net;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Web.Mail;

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

                if (setting.UseGraphSendingEmail)
                {
                    SendGraphEmail(email, setting, sendTime, attempt);
                }
                else
                {
                    SendSmtpEmail(email, setting, sendTime, attempt);
                }

                email.SendTime = sendTime;
                email.SendStatus = EmailSendStatus.Sent;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error sending email. EmailLog Id: {email.Id}. ", ex);
                email.SendStatus = EmailSendStatus.Sent;
            }
        }

        private void SendGraphEmail(EmailLog email, Setting setting, DateTime sendTime, EmailLogAttempt attempt)
        {
            try
            {
                //email.EmailAddress = e "kc@dhsolutions.se";
                var token = GetOAuthToken(setting.GraphTenantId, setting.GraphClientId, setting.GraphClientSecret);

                if (token != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                        var blockedEmails = setting.BlockedEmailRecipients;

                        var toRecipients = email.EmailAddress.Split(',')
                            .Where(address => IsValidEmail(address) && !IsBlockedRecipient(address, blockedEmails))
                            .Select(address => new { emailAddress = new { address } }).ToArray();

                        var ccRecipients = !string.IsNullOrWhiteSpace(email.Cc)
                            ? email.Cc.Split(',')
                                .Where(address => IsValidEmail(address) && !IsBlockedRecipient(address, blockedEmails))
                                .Select(address => new { emailAddress = new { address } }).ToArray()
                            : new object[0];

                        var bccRecipients = !string.IsNullOrWhiteSpace(email.Bcc)
                            ? email.Bcc.Split(',')
                                .Where(address => IsValidEmail(address) && !IsBlockedRecipient(address, blockedEmails))
                                .Select(address => new { emailAddress = new { address } }).ToArray()
                            : new object[0];

                        // Process the second set of attachments
                        var attachments = !string.IsNullOrWhiteSpace(email.Files)
                        ? email.Files.Split('|')
                            .Where(File.Exists)
                            .Select(filePath => new Dictionary<string, object>
                            {
                                { "@odata.type", "#microsoft.graph.fileAttachment" },
                                { "name", Path.GetFileName(filePath) },
                                { "contentBytes", Convert.ToBase64String(File.ReadAllBytes(filePath)) }
                            })
                            .Cast<object>() // Ensure compatibility with List<object>
                            .ToList()
                        : new List<object>();


                        var attachments1 = !string.IsNullOrWhiteSpace(email.FilesInternal)
                        ? email.FilesInternal.Split('|')
                            .Where(File.Exists)
                            .Select(filePath => new Dictionary<string, object>
                            {
                                { "@odata.type", "#microsoft.graph.fileAttachment" },
                                { "name", Path.GetFileName(filePath) },
                                { "contentBytes", Convert.ToBase64String(File.ReadAllBytes(filePath)) }
                            })
                            .Cast<object>() // Ensure compatibility with List<object>
                            .ToList()
                        : new List<object>();


                        // Combine both attachment lists
                        var combinedAttachments = attachments.Concat(attachments1).ToArray();

                        // Set the importance based on HighPriority variable
                        string importance = email.HighPriority ? "high" : "normal";


                        
                        // Conditionally add custom headers
                        var customHeaders = new List<object>();
                        if (!string.IsNullOrWhiteSpace(email.MessageId))
                        {
                            customHeaders.Add(new { name = "x-message-id", value = email.MessageId });
                        }


                        var message = new
                        {
                            message = new
                            {
                                subject = email.Subject,
                                body = new { contentType = "HTML", content = email.Body },
                                toRecipients,
                                ccRecipients,
                                bccRecipients,
                                attachments = combinedAttachments,
                                importance = importance,
                                internetMessageHeaders = customHeaders.Any() ? customHeaders : null // Only include headers if any exist
                            }
                        };





                        var jsonMessage = JsonConvert.SerializeObject(message);
                        var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                        var usr = setting.GraphUserName;// "support@dhsolutions.se";
                        var emailEndpoint = $"https://graph.microsoft.com/v1.0/users/{usr}/sendMail";

                        var response = client.PostAsync(emailEndpoint, content).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            string error = response.Content.ReadAsStringAsync().Result;
                            throw new Exception($"Failed to send email via Graph: {error}");
                        }

                        email.SendTime = sendTime;
                        email.SendStatus = EmailSendStatus.Sent;

                    }
                }
                else
                {
                    _logger.Error($"Error sending email. EmailLog Id: {email.Id}. ", null);
                    var errorMsg = new StringBuilder("No token acquired");
                    attempt.Message = errorMsg.ToString();
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




        private void SendSmtpEmail(EmailLog email, Setting setting, DateTime sendTime, EmailLogAttempt attempt)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.SMTPServer) || setting.SMTPPort <= 0)
                {
                    setting.SMTPServer = ConfigurationManager.AppSettings["DefaultSmtpServer"];
                    setting.SMTPPort = int.Parse(ConfigurationManager.AppSettings["DefaultSmtpPort"]);
                    setting.SMTPUserName = "";
                    setting.SMTPPassWord = "";
                    setting.IsSMTPSecured = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpSsl"]);// false;
                }


                using (var smtpClient = new SmtpClient(setting.SMTPServer, setting.SMTPPort))
                {
                    if (!string.IsNullOrEmpty(setting.SMTPPassWord))
                    {
                        smtpClient.Credentials = new NetworkCredential(setting.SMTPUserName, setting.SMTPPassWord);
                    }

                    smtpClient.EnableSsl = setting.IsSMTPSecured;

                    var mailMessage = new System.Net.Mail.MailMessage
                    {
                        Subject = email.Subject,
                        Body = email.Body,
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(email.From)
                    };

                    var blockedEmails = setting.BlockedEmailRecipients;

                    AlternateView alterView = ContentToAlternateView(mailMessage.Body);
                    mailMessage.AlternateViews.Add(alterView);

                    if (!string.IsNullOrWhiteSpace(email.Cc))
                    {
                        var addresses = email.Cc.Split(',');
                        foreach (var address in addresses)
                        {
                            if (!IsBlockedRecipient(address, blockedEmails))
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
                        mailMessage.Priority = System.Net.Mail.MailPriority.High;

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

        private AlternateView ContentToAlternateView(string content)
        {
            Stream Base64ToImageStream(string base64String)
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                return ms;
            }

            var imgCount = 0;
            List<LinkedResource> resourceCollection = new List<LinkedResource>();
            foreach (Match m in Regex.Matches(content, "<img([\\s\\S]+?)src\\s?=\\s?['\\\"](?<srcAttr>[^'\\\"]+?)['\\\"]([\\s\\S]+?)/?>"))
            {
                imgCount++;
                var srcAttribute = m.Groups["srcAttr"].Value;
                string type = Regex.Match(srcAttribute, ":(?<type>.*?);base64,").Groups["type"].Value;
                string base64 = Regex.Match(srcAttribute, "base64,(?<base64>.*?)$").Groups["base64"].Value;

                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(base64)) continue;

                content = content.Replace(srcAttribute, "cid:" + imgCount);

                var tempResource = new LinkedResource(Base64ToImageStream(base64), new System.Net.Mime.ContentType(type))
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



        static string GetOAuthToken(string tenantId, string clientId, string clientSecret)
        {
            using (HttpClient client = new HttpClient())
            {
                var tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
                var content = new FormUrlEncodedContent(new[]
                {
                     new KeyValuePair<string, string>("client_id", clientId),
                     new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),
                     new KeyValuePair<string, string>("client_secret", clientSecret),
                     new KeyValuePair<string, string>("grant_type", "client_credentials")
                 });

                HttpResponseMessage response = client.PostAsync(tokenEndpoint, content).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    //throw new Exception($"Failed to get access token: {responseString}");
                }

                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);
                return responseObject.access_token;
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

        private int? GetCustomerIdForEmail(EmailLog email)
        {
            var caseHistoryCustomerId = _context.CaseHistories
                .Where(ch => ch.Id == email.CaseHistory_Id)
                .Select(ch => (int?)ch.Customer_Id)
                .FirstOrDefault();

            if (caseHistoryCustomerId.HasValue)
            {
                return caseHistoryCustomerId;
            }

            return _context.Logs
                .Where(log => log.Id == email.Log_Id)
                .Select(log => (int?)log.Case.Customer_Id)
                .FirstOrDefault();
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

        private void AttachFiles(System.Net.Mail.MailMessage mailMessage, string filesString)
        {
            //TODO - Change separator - se EmailService
            var files = filesString?.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (files != null && files.Any())
            {
                foreach (var file in files)
                {
                    if (System.IO.File.Exists(file))
                        mailMessage.Attachments.Add(new System.Net.Mail.Attachment(file));
                }
            }
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
