﻿using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Email;
using DH.Helpdesk.Domain;

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

	public sealed class EmailService : IEmailService
	{
		private readonly string _EMAIL_SEND_MESSAGE = "Email has been sent!";
		private readonly int _MAX_NUMBER_SENDING_EMAIL = 3;

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
			EmailResponse res = emailsettings.Response;
			var sendTime = DateTime.Now;

			using (var smtpClient = new SmtpClient(emailsettings.SmtpSettings.Server, emailsettings.SmtpSettings.Port))
			{
				if (!string.IsNullOrEmpty(emailsettings.SmtpSettings.UserName) &&
				    !string.IsNullOrEmpty(emailsettings.SmtpSettings.Pass))
				{
					smtpClient.Credentials = new NetworkCredential(emailsettings.SmtpSettings.UserName, emailsettings.SmtpSettings.Pass);
				}

				smtpClient.EnableSsl = emailsettings.SmtpSettings.IsSecured;

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
					res = new EmailResponse(sendTime, emailsettings.Response.ResponseMessage + " | " + _EMAIL_SEND_MESSAGE,
						emailsettings.Response.NumberOfTry);
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

			if (res.NumberOfTry != emailsettings.Response.NumberOfTry && res.NumberOfTry <= _MAX_NUMBER_SENDING_EMAIL)
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

		public EmailResponse SendEmail(
			string from,
			string to,
			string subject,
			string body,
			List<DH.Helpdesk.Domain.Field> fields,
			EmailSettings emailsettings,
			string mailMessageId = "",
			bool highPriority = false,
			List<string> files = null,
			string siteSelfService = "",
			string siteHelpdesk = "",
			EmailType emailType = EmailType.ToMail)
		{
			EmailResponse res = emailsettings.Response;
			var sendTime = DateTime.Now;


			SmtpClient _smtpClient;
			CultureInfo oldCI = Thread.CurrentThread.CurrentCulture;

			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

			try
			{
				string smtpServer = emailsettings.SmtpSettings.Server;
				var smtpPort = emailsettings.SmtpSettings.Port;

				if (!string.IsNullOrWhiteSpace(smtpServer) && !string.IsNullOrWhiteSpace(from) && !string.IsNullOrWhiteSpace(to))
				{
					if (IsValidEmail(from))
					{
						if (smtpPort != 0)
							_smtpClient = new SmtpClient(smtpServer, smtpPort);
						else
							_smtpClient = new SmtpClient(smtpServer);

						if (!string.IsNullOrEmpty(emailsettings.SmtpSettings.UserName) &&
						    !string.IsNullOrEmpty(emailsettings.SmtpSettings.Pass))
						{
							_smtpClient.Credentials = new NetworkCredential(emailsettings.SmtpSettings.UserName,
								emailsettings.SmtpSettings.Pass);
						}

						_smtpClient.EnableSsl = emailsettings.SmtpSettings.IsSecured;

						var msg = GetMailMessage(from, to, subject, body, fields, mailMessageId, highPriority, files, siteSelfService, siteHelpdesk, emailType);

						if (msg.To.Count > 0 || msg.Bcc.Count > 0 || msg.CC.Count > 0)
						{
							_smtpClient.Send(msg);
							res = new EmailResponse(sendTime, res.ResponseMessage + " | " + _EMAIL_SEND_MESSAGE, res.NumberOfTry);
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

				var tryCount = res.NumberOfTry + 1;
				res = new EmailResponse(sendTime, res.ResponseMessage + " | " + msg, tryCount);
			}
			finally
			{
				_smtpClient = null;
				Thread.CurrentThread.CurrentCulture = oldCI;
				Thread.CurrentThread.CurrentUICulture = oldCI;
			}

			if (res.NumberOfTry != res.NumberOfTry && res.NumberOfTry <= _MAX_NUMBER_SENDING_EMAIL)
				res = this.SendEmail(from, to, subject, body, fields, emailsettings, mailMessageId, highPriority, files);

			return res;

		}

		public EmailResponse SendEmail(
			EmailLog el,
			string from,
			string to,
			string subject,
			string body,
			List<DH.Helpdesk.Domain.Field> fields,
			EmailSettings emailsettings,
			string mailMessageId = "",
			bool highPriority = false,
			List<string> files = null,
			string siteSelfService = "",
			string siteHelpdesk = "",
			EmailType emailType = EmailType.ToMail)
		{
			if (emailsettings.BatchEmail)
			{
				return EnqueueEmail(el, from, to, subject, body, fields, mailMessageId, highPriority, files, siteSelfService, siteHelpdesk, emailType);
			}
			else
			{
				return SendEmail(from, to, subject, body, fields, emailsettings, mailMessageId, highPriority, files, siteSelfService,
					siteHelpdesk, emailType);
			}
		}

		private MailMessage GetMailMessage(
			string from,
			string to,
			string subject,
			string body,
			List<DH.Helpdesk.Domain.Field> fields,
			string mailMessageId = "",
			bool highPriority = false,
			List<string> files = null,
			string siteSelfService = "",
			string siteHelpdesk = "",
			EmailType emailType = EmailType.ToMail
		)
		{
			var msg = new MailMessage();

			string urlSelfService;
			var urlHelpdesk = "";

			if (!string.IsNullOrWhiteSpace(mailMessageId))
				msg.Headers.Add("Message-ID", mailMessageId);
			if (highPriority)
				msg.Priority = MailPriority.High;

			if (IsValidEmail(to))
			{
				switch (emailType)
				{
					case EmailType.ToMail:
						msg.To.Add(new MailAddress(to));
						break;

					case EmailType.CcMail:
						msg.CC.Add(new MailAddress(to));
						break;

					case EmailType.BccMail:
						msg.Bcc.Add(new MailAddress(to));
						break;

					default:
						msg.To.Add(new MailAddress(to));
						break;
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
				int count = Regex.Matches(body, "/#98").Count;

				string str1 = "[#98]";
				string str2 = "[/#98]";
				string LinkText = "";

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

				foreach (var field in fields)
					if (field.Key == "[#98]")
						field.StringValue = urlSelfService;
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

				foreach (var field in fields)
					if (field.Key == "[#99]")
						field.StringValue = urlHelpdesk;
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

			return msg;
		}

		private EmailResponse EnqueueEmail(
				EmailLog el,
				string from,
				string to,
				string subject,
				string body,
				List<DH.Helpdesk.Domain.Field> fields,
				string mailMessageId = "",
				bool highPriority = false,
				List<string> files = null,
				string siteSelfService = "",
				string siteHelpdesk = "",
				EmailType emailType = EmailType.ToMail
			)
		{
			var res = new EmailResponse { NumberOfTry = 1 };

			try
			{
				var msg = GetMailMessage(from, to, subject, body, fields, mailMessageId, highPriority, files, siteSelfService, siteHelpdesk, emailType);

				el.Body = msg.Body;
				el.Subject = msg.Subject;
				el.Cc = String.Join(",", msg.CC.Select(x => x.Address));
				el.Bcc = String.Join(",", msg.Bcc.Select(x => x.Address));
				el.HighPriority = highPriority;
				el.Files = files != null ? String.Join(",", files) : "";
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
