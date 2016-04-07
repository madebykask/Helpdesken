namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;  
    using System.Configuration;
    using System.Net.Mail;
    using System.Text.RegularExpressions;

    using DH.Helpdesk.BusinessData.Models.Email;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Services.Infrastructure;
    using System.Threading;
    using System.Globalization;

    public sealed class EmailService : IEmailService
    {
        private readonly IEmailSendingSettingsProvider emailSendingSettingsProvider;
        private readonly string _EMAIL_SEND_MESSAGE = "Email has been sent!";
        private readonly int _MAX_NUMBER_SENDING_EMAIL = 3;

        public EmailService(IEmailSendingSettingsProvider emailSendingSettingsProvider)
        {
            this.emailSendingSettingsProvider = emailSendingSettingsProvider;
        }

        public EmailResponse SendEmail(MailAddress from, List<MailAddress> recipients, Mail mail, EmailResponse emailResponse)
        {
            EmailResponse res = emailResponse;
            var sendTime = DateTime.Now;

            foreach (var recipient in recipients)
            {
                res = this.SendEmail(from, recipient, mail, emailResponse);
            }

            res.SendTime = sendTime;
            return res;
        }

        public EmailResponse SendEmail(MailItem mailItem, EmailResponse emailResponse)
        {
            return this.SendEmail(new MailAddress(mailItem.From), new MailAddress(mailItem.To), mailItem.Mail, emailResponse);
        }

        public EmailResponse SendEmail(MailAddress from, MailAddress recipient, Mail mail, EmailResponse emailResponse)
        {
            EmailResponse res = emailResponse;
            var sendTime = DateTime.Now;

            var mailSendingSettings = this.emailSendingSettingsProvider.GetSettings();
           
            using (var smtpClient = new SmtpClient(mailSendingSettings.SmtpServer, mailSendingSettings.SmtpPort))
            {
                CultureInfo oldCI = Thread.CurrentThread.CurrentCulture;  
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                try
                {
                    var mailMessage = new MailMessage(@from, recipient)
                                          {
                                              Subject = mail.Subject,
                                              Body = mail.Body,
                                              IsBodyHtml = true
                                          };
                    smtpClient.Send(mailMessage);
                    res = new EmailResponse(sendTime, emailResponse.ResponseMessage + " | " + _EMAIL_SEND_MESSAGE, emailResponse.NumberOfTry);
                }
                catch (Exception ex)
                {
                    var msg = string.Empty;
                    if (ex.InnerException != null)
                        msg = string.Format("{0} - InnerMessage: {1}", ex.Message, ex.InnerException.Message);
                    else
                        msg = string.Format("{0}", ex.Message);

                    var tryCount = emailResponse.NumberOfTry + 1;
                    res = new EmailResponse(sendTime, emailResponse.ResponseMessage + " | " + msg, tryCount);
                }
                finally
                {
                    Thread.CurrentThread.CurrentCulture = oldCI;
                    Thread.CurrentThread.CurrentUICulture = oldCI;
                }

            }

            if (res.NumberOfTry != emailResponse.NumberOfTry && res.NumberOfTry <= _MAX_NUMBER_SENDING_EMAIL)                            
                res = this.SendEmail(from, recipient, mail, res);            

            return res;
        }

        public EmailResponse SendEmail(EmailItem item, EmailResponse emailResponse)
        {
            return this.SendEmail(
                                item.From,
                                item.To,
                                item.Subject,
                                item.Body,
                                item.Fields,
                                emailResponse,
                                item.MailMessageId,
                                item.IsHighPriority,
                                item.Files);
        }

        public EmailResponse SendEmail(
            string from, 
            string to, 
            string subject, 
            string body, 
            List<DH.Helpdesk.Domain.Field> fields,            
            EmailResponse emailResponse,
            string mailMessageId = "", 
            bool highPriority = false,
            List<string> files = null)
        {
            EmailResponse res = emailResponse;
            var sendTime = DateTime.Now;

            SmtpClient _smtpClient;
            CultureInfo oldCI = Thread.CurrentThread.CurrentCulture;            

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            try
            {                
                string smtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string smtpPort = ConfigurationManager.AppSettings["SmtpPort"].ToString();

                if (!string.IsNullOrWhiteSpace(smtpServer) && !string.IsNullOrWhiteSpace(from) && !string.IsNullOrWhiteSpace(to))
                {
                    if (IsValidEmail(from))
                    {
                        int port;
                        if (int.TryParse(smtpPort, out port))
                            _smtpClient = new SmtpClient(smtpServer, port);
                        else
                            _smtpClient = new SmtpClient(smtpServer);

                        MailMessage msg = new MailMessage();

                        if (!string.IsNullOrWhiteSpace(mailMessageId))
                            msg.Headers.Add("Message-ID", mailMessageId);
                        if (highPriority)
                            msg.Priority = MailPriority.High;  

                        string[] strTo = to.Replace(" ", string.Empty).Replace(Environment.NewLine, string.Empty).Split(new Char[] { ';' });
                        for (int i = 0; i < strTo.Length; i++)
                        {
                            if (strTo[i].Length > 2)
                            {
                                switch (strTo[i].Substring(0, 3))
                                {
                                    case "cc:":
                                        string cc = strTo[i].Substring(3);
                                        if (IsValidEmail(cc))
                                            msg.CC.Add(new MailAddress(cc));
                                        break;
                                    case "bcc":
                                        string bcc = strTo[i].Substring(4);
                                        if (IsValidEmail(bcc))
                                            msg.Bcc.Add(new MailAddress(bcc));
                                        break;
                                    case "to:":
                                        string to_ = strTo[i].Substring(3);
                                        if (IsValidEmail(to_))
                                            msg.To.Add(new MailAddress(to_));
                                        break;
                                    default: 
                                        if (IsValidEmail(strTo[i]))
                                            msg.To.Add(new MailAddress(strTo[i]));
                                        break;
                                }
                            }
                        }

                        bool attachFiles = false; 
                        if (body.Contains("[#14]"))
                        {
                            attachFiles = true;
                            body = body.Replace("[#14]", string.Empty); 
                        }

                        if (body.Contains("[/#98]"))
                        {
                            string str1 = "[#98]";
                            string str2 = "[/#98]";
                            string LinkText;

                            int Pos1 = body.IndexOf(str1) + str1.Length;
                            int Pos2 = body.IndexOf(str2);
                            LinkText = body.Substring(Pos1, Pos2 - Pos1);

                            body = body.Replace(LinkText + "[/#98]", string.Empty);
                        }

                        if (body.Contains("[/#99]"))
                        {
                            string str1 = "[#99]";
                            string str2 = "[/#99]";
                            string LinkText;

                            int Pos1 = body.IndexOf(str1) + str1.Length;
                            int Pos2 = body.IndexOf(str2);
                            LinkText = body.Substring(Pos1, Pos2 - Pos1);

                            body = body.Replace(LinkText + "[/#99]", string.Empty);
                        }

                        msg.Subject = AddInformationToMailBodyAndSubject(subject, fields);
                        msg.From = new MailAddress(from);
                        msg.IsBodyHtml = true;
                        msg.BodyEncoding = System.Text.Encoding.UTF8;
                        msg.Body = AddInformationToMailBodyAndSubject(body, fields).Replace(Environment.NewLine, "<br />");

                        
                        // för log filer 
                        if (files != null && attachFiles)
                        {
                            foreach (var f in files)
                            {
                                if (System.IO.File.Exists(f))
                                    msg.Attachments.Add(new Attachment(f));  
                            }
                        }

                        if (msg.To.Count > 0 || msg.Bcc.Count > 0 || msg.CC.Count > 0)
                        {
                            _smtpClient.Send(msg);
                            res = new EmailResponse(sendTime, emailResponse.ResponseMessage + " | " + _EMAIL_SEND_MESSAGE, emailResponse.NumberOfTry);                            
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

                var tryCount = emailResponse.NumberOfTry + 1;
                res = new EmailResponse(sendTime, emailResponse.ResponseMessage + " | " + msg, tryCount);
            }
            finally
            {
                _smtpClient = null;
                Thread.CurrentThread.CurrentCulture = oldCI;
                Thread.CurrentThread.CurrentUICulture = oldCI;
            }

            if (res.NumberOfTry != emailResponse.NumberOfTry && res.NumberOfTry <= _MAX_NUMBER_SENDING_EMAIL)                            
                res = this.SendEmail(from, to, subject, body, fields, res, mailMessageId, highPriority, files);                            
            
            return res;
           
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

    }
}
