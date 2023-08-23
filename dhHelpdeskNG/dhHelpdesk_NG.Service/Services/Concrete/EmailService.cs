using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Email;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Domain;
using log4net;

namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Text.RegularExpressions;
    using DH.Helpdesk.BusinessData.Models.Email;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Common.Tools;
    using System.Threading;
    using System.Globalization;
    using System.Net;
    using System.Net.Mime;
    using System.IO;
    using DH.Helpdesk.Dal.Repositories;

    public sealed class EmailService : IEmailService
    {
        //Implement iSettingsservice
        private readonly ISettingService _settingService;
        private readonly ICaseHistoryRepository _caseHistoryRepository;

        public EmailService(ISettingService settingsService, ICaseHistoryRepository caseHistoryRepository)
        {
            _settingService = settingsService;
            _caseHistoryRepository = caseHistoryRepository;
        }

        private readonly ILog _logger = LogManager.GetLogger(typeof(EmailService));

        const string EMAIL_SEND_MESSAGE = "Email has been sent!";
        const int MAX_NUMBER_SENDING_EMAIL = 3;

        public EmailResponse SendEmail(MailAddress from, List<MailAddress> recipients, Mail mail, EmailSettings emailsettings)
        {
            EmailResponse res = emailsettings.Response;
            var sendTime = DateTime.Now;

            foreach (var recipient in recipients)
            {
                res = SendEmail(from, recipient, mail, emailsettings);
            }

            res.SendTime = sendTime;
            return res;
        }

        public EmailResponse SendEmail(MailItem mailItem, EmailSettings emailSettings)
        {
            return this.SendEmail(new MailAddress(mailItem.From), new MailAddress(mailItem.To), mailItem.Mail, emailSettings);
        }

        public EmailResponse SendEmail(MailAddress from, MailAddress recipient, Mail mail, EmailSettings emailsettings)
        {
            var res = emailsettings.Response;
            var sendTime = DateTime.Now;

            using (var smtpClient = new SmtpClient(emailsettings.SmtpSettings.Server, emailsettings.SmtpSettings.Port))
            {
                if (!string.IsNullOrEmpty(emailsettings.SmtpSettings.UserName) &&
                    !string.IsNullOrEmpty(emailsettings.SmtpSettings.Pass))
                {
                    smtpClient.Credentials = new NetworkCredential(emailsettings.SmtpSettings.UserName, emailsettings.SmtpSettings.Pass);
                }

                smtpClient.EnableSsl = emailsettings.SmtpSettings.IsSecured;

                var oldCI = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                try
                {
                    var mailMessage = new MailMessage(@from, recipient)
                    {
                        Subject = mail.Subject,
                        Body = mail.Body,
                        IsBodyHtml = true,
                    };

                    AlternateView alterView = ContentToAlternateView(mail.Body);
                    mailMessage.AlternateViews.Add(alterView);
                    smtpClient.Send(mailMessage);
                    res = new EmailResponse(sendTime, emailsettings.Response.ResponseMessage + " | " + EMAIL_SEND_MESSAGE, emailsettings.Response.NumberOfTry);
                }
                catch (Exception ex)
                {
                    var msg = string.Empty;
                    if (ex.InnerException != null)
                        msg = string.Format("{0} - InnerMessage: {1}", ex.Message, ex.InnerException.Message);
                    else
                        msg = string.Format("{0}", ex.Message);

                    var tryCount = emailsettings.Response.NumberOfTry + 1;
                    res = new EmailResponse(sendTime, emailsettings.Response.ResponseMessage + " | " + msg, tryCount);
                }
                finally
                {
                    Thread.CurrentThread.CurrentCulture = oldCI;
                    Thread.CurrentThread.CurrentUICulture = oldCI;
                }

            }

            if (res.NumberOfTry != emailsettings.Response.NumberOfTry && res.NumberOfTry <= MAX_NUMBER_SENDING_EMAIL)
            {
                emailsettings.Response.NumberOfTry = res.NumberOfTry;
                res = this.SendEmail(from, recipient, mail, emailsettings);
            }

            return res;
        }

        public EmailResponse SendEmail(EmailLog el, EmailItem item, EmailSettings emailsettings, string siteSelfService = "",
            string siteHelpdesk = "", EmailType emailType = EmailType.ToMail)
        {
            return this.SendEmail(
                el,
                item.From,
                item.To,
                null,
                item.Subject,
                item.Body,
                item.Fields,
                emailsettings,
                item.MailMessageId,
                item.IsHighPriority,
                item.Files,
                siteSelfService, siteHelpdesk,
                emailType);
        }

        //sends case email from Helpdesk
        public EmailResponse SendEmail(
            string from,
            string to,
            string cc,
            string subject,
            string body,
            List<Field> fields,
            EmailSettings emailsettings,
            string mailMessageId = "",
            bool highPriority = false,
            List<MailFile> files = null,
            string siteSelfService = "",
            string siteHelpdesk = "",
            EmailType emailType = EmailType.ToMail,
            string siteSelfServiceMergeParent = "")
        {
            var res = emailsettings.Response;
            var sendTime = DateTime.Now;


            SmtpClient _smtpClient;
            var oldCI = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            //Todo - check Gmail/Images
            try
            {
                string smtpServer = emailsettings.SmtpSettings.Server;
                var smtpPort = emailsettings.SmtpSettings.Port;

                if (!string.IsNullOrWhiteSpace(smtpServer) && !string.IsNullOrWhiteSpace(from) && (!string.IsNullOrWhiteSpace(to) || !string.IsNullOrWhiteSpace(cc)))
                {
                    if (IsValidEmail(from))
                    {
                        _smtpClient = smtpPort != 0
                            ? new SmtpClient(smtpServer, smtpPort)
                            : new SmtpClient(smtpServer);

                        if (!string.IsNullOrEmpty(emailsettings.SmtpSettings.UserName) &&
                            !string.IsNullOrEmpty(emailsettings.SmtpSettings.Pass))
                        {
                            _smtpClient.Credentials =
                                new NetworkCredential(emailsettings.SmtpSettings.UserName, emailsettings.SmtpSettings.Pass);
                        }

                        _smtpClient.EnableSsl = emailsettings.SmtpSettings.IsSecured;
                        
                        var msg = GetMailMessage(from, to, cc, subject, body, fields, mailMessageId, highPriority, files, siteSelfService, siteHelpdesk, emailType, siteSelfServiceMergeParent);

                        if (msg.To.Count > 0 || msg.Bcc.Count > 0 || msg.CC.Count > 0)
                        {
                            AlternateView alterView = ContentToAlternateView(msg.Body);
                            msg.AlternateViews.Add(alterView);
                            _smtpClient.Send(msg);
                            res = new EmailResponse(sendTime, emailsettings.Response.ResponseMessage + " | " + EMAIL_SEND_MESSAGE, res.NumberOfTry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = string.Empty;
                if (ex.InnerException != null)
                    msg = string.Format("{0} - InnerMessage: {1}", ex.Message, ex.InnerException.Message);
                else
                    msg = string.Format("{0}", ex.Message);

                var tryCount = emailsettings.Response.NumberOfTry + 1;
                res = new EmailResponse(sendTime, emailsettings.Response.ResponseMessage + " | " + msg, tryCount);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = oldCI;
                Thread.CurrentThread.CurrentUICulture = oldCI;
            }

            if (res.NumberOfTry != emailsettings.Response.NumberOfTry && res.NumberOfTry <= MAX_NUMBER_SENDING_EMAIL)
            {
                emailsettings.Response.NumberOfTry = res.NumberOfTry;
                res = this.SendEmail(from, to, cc, subject, body, fields, emailsettings, mailMessageId, highPriority, files);
            }
            return res;
        }

        public EmailResponse SendEmail(
            EmailLog el,
            string from,
            string to,
            string cc,
            string subject,
            string body,
            List<Field> fields,
            EmailSettings emailsettings,
            string mailMessageId = "",
            bool highPriority = false,
            List<MailFile> files = null,
            string siteSelfService = "",
            string siteHelpdesk = "",
            EmailType emailType = EmailType.ToMail, 
             string siteSelfServiceMergeParent = "")
        {
           
            //Get customerid from casehistory
            int customerID = _caseHistoryRepository.GetById((int)el.CaseHistory_Id).Customer_Id;
            to = RemoveBlockedRecipients(customerID, to);
            if(!String.IsNullOrEmpty(cc))
            {
                cc = RemoveBlockedRecipients(customerID, cc);
            }
            if(!String.IsNullOrEmpty(to))
            {
                return emailsettings.BatchEmail
                ? EnqueueEmail(el, from, to, cc, subject, body, fields, mailMessageId, highPriority, files, siteSelfService, siteHelpdesk, emailType, siteSelfServiceMergeParent)
                : SendEmail(from, to, cc, subject, body, fields, emailsettings, mailMessageId, highPriority, files, siteSelfService, siteHelpdesk, emailType, siteSelfServiceMergeParent);
            }
            else
            {
                return new EmailResponse(DateTime.Now, "No valid recipients", 0);
            }
        }


        private MailMessage GetMailMessage(
            string from,
            string to,
            string cc,
            string subject,
            string body,
            List<Field> fields,
            string mailMessageId = "",
            bool highPriority = false,
            List<MailFile> files = null,
            string siteSelfService = "",
            string siteHelpdesk = "",
            EmailType emailType = EmailType.ToMail,
            string siteSelfServiceMergeParent = ""
        )
        {
            var msg = new MailMessage();

            string urlSelfService;
            var urlHelpdesk = "";

            if (!string.IsNullOrWhiteSpace(mailMessageId))
                msg.Headers.Add("Message-ID", mailMessageId);

            if (highPriority)
                msg.Priority = MailPriority.High;

            to = string.IsNullOrEmpty(to) ? "" : string.Join(",", to.Split(',', ';').ToDistintList(true).Where(IsValidEmail));
            cc = string.IsNullOrEmpty(cc) ? "" : string.Join(",", cc.Split(',', ';').ToDistintList(true).Where(IsValidEmail));

            switch (emailType)
            {
                case EmailType.ToMail:
                    if (!string.IsNullOrEmpty(to))
                        msg.To.Add(to);
                    if (!string.IsNullOrEmpty(cc))
                        msg.CC.Add(cc);
                    break;

                case EmailType.CcMail:
                    if (!string.IsNullOrEmpty(to))
                        msg.To.Add(to);
                    break;

                case EmailType.BccMail:
                    if (!string.IsNullOrEmpty(to))
                        msg.To.Add(to);
                    break;

                default:
                    msg.To.Add(to);
                    if (!string.IsNullOrEmpty(cc))
                        msg.CC.Add(cc);
                    break;
            }

            var attachFiles = false;
            if (body.Contains("[#14]"))
            {
                attachFiles = true;
                body = body.Replace("[#14]", string.Empty);
            }

            var attachInternalFiles = false;
            if (body.Contains("[#30]"))
            {
                attachInternalFiles = true;
                body = body.Replace("[#30]", string.Empty);
            }

            if (body.Contains("[/#98]"))
            {
                var count = Regex.Matches(body, "/#98").Count;

                var str1 = "[#98]";
                var str2 = "[/#98]";
                var LinkText = "";

                for (int i = 1; i <= count; i++)
                {
                    int Pos1 = body.IndexOf(str1) + str1.Length;
                    int Pos2 = body.IndexOf(str2);
                    LinkText = body.Substring(Pos1, Pos2 - Pos1);

                    urlSelfService = "<a href='" + siteSelfService + "'>" + LinkText + "</a>";

                    var regex = new Regex(Regex.Escape(LinkText + "[/#98]"));
                    body = regex.Replace(body, string.Empty, 1);

                    regex = new Regex(Regex.Escape("[#98]"));
                    body = regex.Replace(body, urlSelfService, 1);
                }
            }
            else
            {
                urlSelfService = "<a href='" + siteSelfService + "'>" + siteSelfService + "</a>";

                if (fields != null)
                {
                    foreach (var field in fields)
                        if (field.Key == "[#98]")
                            field.StringValue = urlSelfService;
                }
            }
            if (body.Contains("[/#MP98]"))
            {
                var count = Regex.Matches(body, "/#MP98").Count;

                var str1 = "[#MP98]";
                var str2 = "[/#MP98]";
                var LinkText = "";

                for (int i = 1; i <= count; i++)
                {
                    int Pos1 = body.IndexOf(str1) + str1.Length;
                    int Pos2 = body.IndexOf(str2);
                    LinkText = body.Substring(Pos1, Pos2 - Pos1);

                    urlSelfService = "<a href='" + siteSelfServiceMergeParent + "'>" + LinkText + "</a>";

                    var regex = new Regex(Regex.Escape(LinkText + "[/#MP98]"));
                    body = regex.Replace(body, string.Empty, 1);

                    regex = new Regex(Regex.Escape("[#MP98]"));
                    body = regex.Replace(body, urlSelfService, 1);
                }
            }
            else
            {
                urlSelfService = "<a href='" + siteSelfServiceMergeParent + "'>" + siteSelfServiceMergeParent + "</a>";

                if (fields != null)
                {
                    foreach (var field in fields)
                        if (field.Key == "[#MP98]")
                            field.StringValue = urlSelfService;
                }
            }

            if (body.Contains("[/#99]"))
            {
                int count = Regex.Matches(body, "/#99").Count;

                for (int i = 1; i <= count; i++)
                {

                    string str1 = "[#99]";
                    string str2 = "[/#99]";
                    string LinkText;

                    int Pos1 = body.IndexOf(str1) + str1.Length;
                    int Pos2 = body.IndexOf(str2);
                    LinkText = body.Substring(Pos1, Pos2 - Pos1);

                    urlHelpdesk = "<a href='" + siteHelpdesk + "'>" + LinkText + "</a>";

                    var regex = new Regex(Regex.Escape(LinkText + "[/#99]"));
                    body = regex.Replace(body, string.Empty, 1);

                    regex = new Regex(Regex.Escape("[#99]"));
                    body = regex.Replace(body, urlHelpdesk, 1);
                }

            }
            else
            {
                urlHelpdesk = "<a href='" + siteHelpdesk + "'>" + siteHelpdesk + "</a>";

                if (fields != null)
                {

                    foreach (var field in fields)
                        if (field.Key == "[#99]")
                            field.StringValue = urlHelpdesk;
                }

            }
            if (body.Contains("[/#MP99]"))
            {
                int count = Regex.Matches(body, "/#MP99").Count;

                for (int i = 1; i <= count; i++)
                {

                    string str1 = "[#MP99]";
                    string str2 = "[/#MP99]";
                    string LinkText;

                    int Pos1 = body.IndexOf(str1) + str1.Length;
                    int Pos2 = body.IndexOf(str2);
                    LinkText = body.Substring(Pos1, Pos2 - Pos1);

                    urlHelpdesk = "<a href='" + siteHelpdesk + "'>" + LinkText + "</a>";

                    var regex = new Regex(Regex.Escape(LinkText + "[/#MP99]"));
                    body = regex.Replace(body, string.Empty, 1);

                    regex = new Regex(Regex.Escape("[#MP99]"));
                    body = regex.Replace(body, urlHelpdesk, 1);
                }

            }
            else
            {
                urlHelpdesk = "<a href='" + siteHelpdesk + "'>" + siteHelpdesk + "</a>";

                if (fields != null)
                {

                    foreach (var field in fields)
                        if (field.Key == "[#MP99]")
                            field.StringValue = urlHelpdesk;
                }

            }

            msg.Subject = AddInformationToMailBodyAndSubject(subject, fields);
            msg.From = new MailAddress(from);
            msg.IsBodyHtml = true;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.Body = AddInformationToMailBodyAndSubject(body, fields)
                .Replace(Environment.NewLine, "<br />")
                .Replace("\n", "<br />");


            //body
            _logger.Warn($"Email: {msg.Subject} | {msg.Body}");

            // attach files
            if (attachFiles && files != null)
            {
                var externalFiles = files.Where(f => !f.IsInternal).ToList();
                AttachFiles(msg, externalFiles);
            }

            if (attachInternalFiles && files != null)
            {
                var internalFiles = files.Where(f => f.IsInternal).ToList();
                AttachFiles(msg, internalFiles);
            }

            return msg;
        }

        private void AttachFiles(MailMessage msg, IList<MailFile> files)
        {
            if (files != null && files.Any())
            {
                foreach (var f in files)
                {
                    if (System.IO.File.Exists(f.FilePath))
                    {
                        msg.Attachments.Add(new Attachment(f.FilePath));
                    }
                }
            }
        }

        private EmailResponse EnqueueEmail(
                EmailLog el,
                string from,
                string to,
                string cc,
                string subject,
                string body,
                List<Field> fields,
                string mailMessageId = "",
                bool highPriority = false,
                List<MailFile> files = null,
                string siteSelfService = "",
                string siteHelpdesk = "",
                EmailType emailType = EmailType.ToMail,
                string siteSelfServiceMergeParent = ""
            )
        {
            var res = new EmailResponse { NumberOfTry = 1 };

            try
            {
                var attachExternalFiles = body.Contains("[#14]");
                var attachInternalFiles = body.Contains("[#30]");

                var externalFiles = new List<string>();
                var internalFiles = new List<string>();

                if (attachExternalFiles && files != null)
                {
                    externalFiles = files.Where(f => !f.IsInternal).Select(f => f.FilePath).ToList();
                }

                if (attachInternalFiles && files != null)
                {
                    internalFiles = files.Where(f => f.IsInternal).Select(f => f.FilePath).ToList();
                }

                var msg = GetMailMessage(from, to, cc, subject, body, fields,
                    mailMessageId, highPriority, files, siteSelfService, siteHelpdesk, emailType, siteSelfServiceMergeParent);

                el.Body = msg.Body;
                el.Subject = msg.Subject;
                el.Cc = string.Join(",", msg.CC.Select(x => x.Address));
                el.Bcc = string.Join(",", msg.Bcc.Select(x => x.Address));
                el.HighPriority = highPriority;
                el.Files = externalFiles.Any() ? externalFiles.JoinEmailAttachmentsToString() : ""; //TODO - Change separator
                el.FilesInternal = internalFiles.Any() ? internalFiles.JoinEmailAttachmentsToString() : "";
                el.From = msg.From.Address;
                el.SendStatus = EmailSendStatus.Pending;
                el.Attempts = 0;

                res.ResponseMessage = "Enqueued";
            }
            catch (Exception ex)
            {
                res.ResponseMessage = ex.InnerException != null ? "Enqueue error: " + $"{ex.Message} - InnerMessage: {ex.InnerException.Message}" : $"{ex.Message}";
            }

            return res;
        }

        public string GetMailMessageId(string helpdeskFromAddress)
        {
            return "<" + DateTime.Now.Year.ToString()
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
            return EmailHelper.IsValid(inputEmail);
        }

        private string AddInformationToMailBodyAndSubject(string text, List<DH.Helpdesk.Domain.Field> fields)
        {
            string ret = text;

            if (fields != null)
                foreach (var f in fields)
                {
                    ret = ret.Replace(f.Key, f.StringValue);
                }

            return ret;
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
        //Kan det vara en lista med flera adresser??
        public string RemoveBlockedRecipients(int customerId, string emailList)
        {
            // Split email lists into arrays
            string[] emails = emailList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(e => e.Trim())
                                       .ToArray();

            string blockedEmailsList = _settingService.GetCustomerSettings(customerId).BlockedEmailRecipients;
            string[] blockedPatterns = blockedEmailsList.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                        .Select(e => e.Trim())
                                                        .ToArray();

            // Filter out the emails based on the blocked patterns
            string[] filteredEmails = emails.Where(email =>
                    !blockedPatterns.Any(pattern =>
                        email.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) != -1
                    )
                ).ToArray();

            // Join the filtered emails back into a single string
            return string.Join(",", filteredEmails);
        }
    }
}
