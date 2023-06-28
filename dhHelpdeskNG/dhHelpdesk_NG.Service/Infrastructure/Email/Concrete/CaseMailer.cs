using System;
using System.Collections.Generic;

using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using System.Configuration;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Email;
using DH.Helpdesk.BusinessData.Models.Email;
using DH.Helpdesk.BusinessData.Models.MailTemplates;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Domain.MailTemplates;
using DH.Helpdesk.Common.Constants;
using System.Collections;
using PostSharp;

namespace DH.Helpdesk.Services.Infrastructure.Email.Concrete
{
    public sealed class CaseMailer : ICaseMailer
    {
        private readonly IEmailLogRepository _emailLogRepository;
        private readonly IEmailService _emailService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IEmailFactory _emailFactory;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly IEmailSendingSettingsProvider _emailSendingSettingsProvider;
        private readonly ICaseExtraFollowersService _caseExtraFollowersService;
		private readonly IGlobalSettingService _globalSettingService;
        private readonly ILogService _logService;
        private readonly ICustomerService _customerService;

        public CaseMailer(
            IEmailLogRepository emailLogRepository,
            IEmailService emailService,
            IMailTemplateService mailTemplateService,
            IEmailFactory emailFactory,
            IUserService userService,
            ISettingService settingService,
            IEmailSendingSettingsProvider emailSendingSettingsProvider,
            ICaseExtraFollowersService caseExtraFollowersService,
			IGlobalSettingService globalSettingService,
            ILogService logService,
            ICustomerService customerService)
        {
            _emailLogRepository = emailLogRepository;
            _emailService = emailService;
            _mailTemplateService = mailTemplateService;
            _emailFactory = emailFactory;
            _userService = userService;
            _settingService = settingService;
            _emailSendingSettingsProvider = emailSendingSettingsProvider;
            _caseExtraFollowersService = caseExtraFollowersService;
			_globalSettingService = globalSettingService;
            _logService = logService;
            _customerService = customerService;
        }

        public void InformNotifierIfNeeded(
            int caseHistoryId,
            List<Field> fields,
            CaseLog log,
            bool dontSendMailToNotfier,
            Case newCase,
            string helpdeskMailFromAdress,
            List<MailFile> files,
            MailSenders mailSenders,
            bool isCreatingCase,
            bool caseMailSetting_DontSendMail,
            string absoluterUrl,
            string extraFollowersEmails = null)
        {

            //if (!isCreatingCase)
            //{
            if (log == null || string.IsNullOrWhiteSpace(log.TextExternal) ||
                !log.SendMailAboutCaseToNotifier ||
                dontSendMailToNotfier ||
                (caseMailSetting_DontSendMail == false && newCase.FinishingDate != null))
            {
                return;
            }

            var template = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(
                                                newCase.Customer_Id,
                                                newCase.RegLanguage_Id,
                                                (int)GlobalEnums.MailTemplates.InformNotifier);

            if (template == null)
            {
                return;
            }
            string extraBody = "";
            if (template.MailTemplate.IncludeLogText_External)
            {
                extraBody = GetExternalLogTextHistory(newCase, helpdeskMailFromAdress, log);
            }
            var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id);

            var smtpInfo = 
                new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, 
                    customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            if (!string.IsNullOrEmpty(template.Body) && !string.IsNullOrEmpty(template.Subject))
            {
                var to = newCase.PersonsEmail.Split(';', ',').ToList();

                var extraFollowers = !string.IsNullOrEmpty(extraFollowersEmails)
                    ? extraFollowersEmails.Split(';', ',').ToList()
                    : _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id).Select(x => x.Follower).ToList();

                var customEmailSender4 = mailSenders.DefaultOwnerWGEMail;
                if (string.IsNullOrWhiteSpace(customEmailSender4))
                    customEmailSender4 = mailSenders.WGEmail;

                if (string.IsNullOrWhiteSpace(customEmailSender4))
                    customEmailSender4 = mailSenders.SystemEmail;

                var emailLogs = new Dictionary<string, EmailLog>();
                if (template.MailTemplate.SendMethod == EmailSendMethod.OneEmail)
                {
                    var recepients = string.Join(",", to.ToDistintList(true).ToArray());
                    var ccRecepients = extraFollowers.Any() ? string.Join(",", extraFollowers.ToDistintList(true).ToArray()) : null;
                    var emailLog = _emailFactory.CreateEmailLog(
                        caseHistoryId,
                       GlobalEnums.MailTemplates.InformNotifier,
                        recepients,
                        _emailService.GetMailMessageId(customEmailSender4));
                    emailLog.Cc = ccRecepients;
                    emailLogs.Add(recepients, emailLog);
                }
                else
                {
                    if(extraFollowers.Any())
                        to = to.Union(extraFollowers).Where( _emailService.IsValidEmail).ToList().ToDistintList(true).ToList();
                    foreach (var recepient in to)
                    {
                        var emailLog = _emailFactory.CreateEmailLog(
                            caseHistoryId,
                            GlobalEnums.MailTemplates.InformNotifier,
                            recepient,
                            _emailService.GetMailMessageId(customEmailSender4));
                        emailLogs.Add(recepient, emailLog);
                    }
                }

                foreach (var eLog in emailLogs)
                {
                    if (!string.IsNullOrWhiteSpace(eLog.Key))
                    {
                        if(log.Id > 0) 
                            eLog.Value.Log_Id = log.Id;

                        var siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + eLog.Value.EmailLogGUID.ToString();
                        var mailResponse = EmailResponse.GetEmptyEmailResponse();
                        var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);

						var globalSetting = _globalSettingService.GetGlobalSettings().First();
						var caseEditPath = globalSetting.UseMobileRouting ?
							CasePaths.EDIT_CASE_MOBILEROUTE :
							CasePaths.EDIT_CASE_DESKTOP;

                        var siteHelpdesk = absoluterUrl + caseEditPath + newCase.Id.ToString();
                        var emailType = GetEmailType(template.MailTemplate.SendMethod, eLog.Value, extraFollowers.ToArray());

                        var res =
                            _emailService.SendEmail(eLog.Value,
                                customEmailSender4,
                                eLog.Value.EmailAddress,
                                eLog.Value.Cc,
                                template.Subject,
                                template.Body + extraBody,
                                fields,
                                mailSetting,
                                eLog.Value.MessageId,
                                log.HighPriority,
                                files,
                                siteSelfService,
                                siteHelpdesk,
                                emailType);

                        eLog.Value.SetResponse(res.SendTime, res.ResponseMessage);
                        var now = DateTime.Now;
                        eLog.Value.CreatedDate = now;
                        eLog.Value.ChangedDate = now;
                        _emailLogRepository.Add(eLog.Value);
                        _emailLogRepository.Commit();
                    }
                }

            }
            //}
        }

        public void InformOwnerDefaultGroupIfNeeded(
            int caseHistoryId,
            List<Field> fields,
            CaseLog log,
            bool dontSendMailToNotfier,
            Case newCase,
            string helpdeskMailFromAdress,
            List<MailFile> files,
            string absoluterUrl)
        {
            if (log == null || log.Id <= 0 ||
                !log.SendMailAboutCaseToNotifier ||
                dontSendMailToNotfier ||
                newCase.FinishingDate != null)
            {
                return;
            }
            //Todo - check this
            var template = 
                _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, (int)GlobalEnums.MailTemplates.InformNotifier);

            if (template == null)
            {
                return;
            }

            var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            if (!string.IsNullOrEmpty(template.Body) && !string.IsNullOrEmpty(template.Subject))
            {
                var mailMessageId = _emailService.GetMailMessageId(helpdeskMailFromAdress);

                // http://redmine.fastdev.se/issues/10997
                /*var caseOwner = userService.GetUser(newCase.User_Id);
                if (caseOwner == null || 
                    !caseOwner.Default_WorkingGroup_Id.HasValue)
                {
                    return;
                }

                var defaultWorkingGroup = workingGroupService.GetWorkingGroup(caseOwner.Default_WorkingGroup_Id.Value);
                if (defaultWorkingGroup == null || 
                    !emailService.IsValidEmail(defaultWorkingGroup.EMail))
                {
                    return;
                }*/

                var defaultWorkingGroup = _userService.GetUserDefaultWorkingGroup(newCase.User_Id.Value, newCase.Customer_Id);
                if (defaultWorkingGroup == null ||
                    !_emailService.IsValidEmail(defaultWorkingGroup.EMail))
                {
                    return;
                }

                var defaultWorkingGroupEmailLog = 
                    _emailFactory.CreateEmailLog(caseHistoryId,
                        GlobalEnums.MailTemplates.InformNotifier,
                        defaultWorkingGroup.EMail,
                        mailMessageId);

                defaultWorkingGroupEmailLog.Log_Id = log.Id;

                var siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"] + defaultWorkingGroupEmailLog.EmailLogGUID;

				var globalSetting = _globalSettingService.GetGlobalSettings().First();
				var caseEditPath = globalSetting.UseMobileRouting ?
					CasePaths.EDIT_CASE_MOBILEROUTE :
					CasePaths.EDIT_CASE_DESKTOP;

                var siteHelpdesk = absoluterUrl + caseEditPath + newCase.Id;
                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);

                var defaultWorkingGroupEmailItem = 
                    _emailFactory.CreateEmailItem(
                        helpdeskMailFromAdress,
                        defaultWorkingGroupEmailLog.EmailAddress,
                        template.Subject,
                        template.Body,
                        fields,
                        defaultWorkingGroupEmailLog.MessageId,
                        log.HighPriority,
                        files);

                var res = 
                    _emailService.SendEmail(defaultWorkingGroupEmailLog, 
                        defaultWorkingGroupEmailItem, 
                        mailSetting, 
                        siteSelfService, 
                        siteHelpdesk);

                defaultWorkingGroupEmailLog.SetResponse(res.SendTime, res.ResponseMessage);
                var now = DateTime.Now;
                defaultWorkingGroupEmailLog.CreatedDate = now;
                defaultWorkingGroupEmailLog.ChangedDate = now;
                _emailLogRepository.Add(defaultWorkingGroupEmailLog);
                _emailLogRepository.Commit();
            }
        }

        public void InformAboutInternalLogIfNeeded(
            int caseHistoryId,
            List<Field> fields,
            CaseLog log,
            Case newCase,
            string helpdeskMailFromAdress,
            List<MailFile> files,
            string absoluterUrl,
            MailSenders mailSenders)
        {
            if (log == null || log.Id <= 0 || 
                (string.IsNullOrWhiteSpace(log.EmailRecepientsInternalLogTo) && 
                 string.IsNullOrWhiteSpace(log.EmailRecepientsInternalLogCc)))
                return;

            var template = 
                _mailTemplateService.GetMailTemplateForCustomerAndLanguage(
                    newCase.Customer_Id,
                    newCase.RegLanguage_Id,
                    (int)GlobalEnums.MailTemplates.InternalLogNote);

            if (template == null)
                return;

            var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            if (!string.IsNullOrEmpty(template.Body) && !string.IsNullOrEmpty(template.Subject))
            {
                var customEmailSender4 = mailSenders.DefaultOwnerWGEMail;
                if (string.IsNullOrWhiteSpace(customEmailSender4))
                    customEmailSender4 = mailSenders.WGEmail;

                if (string.IsNullOrWhiteSpace(customEmailSender4))
                    customEmailSender4 = mailSenders.SystemEmail;

                var to = !string.IsNullOrEmpty(log.EmailRecepientsInternalLogTo)
                    ? log.EmailRecepientsInternalLogTo
                        .Replace(" ", "")
                        .Replace(Environment.NewLine, "|")
                        .Split(new[] { '|', ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    : new string[0];

                var cc = !string.IsNullOrEmpty(log.EmailRecepientsInternalLogCc)
                    ? log.EmailRecepientsInternalLogCc
                        .Replace(" ", "")
                        .Replace(Environment.NewLine, "|")
                        .Split(new[] { '|', ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    : new string[0];

                var emailLogs = new Dictionary<string, EmailLog>();
                if (template.MailTemplate.SendMethod == EmailSendMethod.OneEmail)
                {
                    var recepients = string.Join(",", to.ToDistintList(true).ToArray());
                    var ccRecepients = cc.Any() ? string.Join(",", cc.ToDistintList(true).ToArray()) : null;
                    var emailLog = _emailFactory.CreateEmailLog(
                        caseHistoryId,
                        GlobalEnums.MailTemplates.InternalLogNote,
                        recepients,
                        _emailService.GetMailMessageId(customEmailSender4));
                    emailLog.Cc = ccRecepients;
                    emailLogs.Add(recepients, emailLog);
                }
                else
                {
                    if(cc.Any())
                        to = to.Union(cc).Where( _emailService.IsValidEmail).ToList().ToDistintList(true).ToArray();
                    foreach (var recepient in to)
                    {
                        var emailLog = _emailFactory.CreateEmailLog(
                            caseHistoryId,
                            GlobalEnums.MailTemplates.InternalLogNote,
                            recepient,
                            _emailService.GetMailMessageId(customEmailSender4));
                        emailLogs.Add(recepient, emailLog);
                    }
                }

                foreach (var eLog in emailLogs)
                {
                    if (!string.IsNullOrWhiteSpace(eLog.Value.EmailAddress) || !string.IsNullOrWhiteSpace(eLog.Value.Cc))
                    {
                        eLog.Value.Log_Id = log.Id;

                        var siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"] + eLog.Value.EmailLogGUID;

						var globalSetting = _globalSettingService.GetGlobalSettings().First();
						var caseEditPath = globalSetting.UseMobileRouting ?
							CasePaths.EDIT_CASE_MOBILEROUTE :
							CasePaths.EDIT_CASE_DESKTOP;

                        var siteHelpdesk = absoluterUrl + caseEditPath + newCase.Id;
                        var mailResponse = EmailResponse.GetEmptyEmailResponse();
                        var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                        var emailType = GetEmailType(template.MailTemplate.SendMethod, eLog.Value, cc);

                        mailResponse =
                            _emailService.SendEmail(eLog.Value,
                                customEmailSender4,
                                eLog.Value.EmailAddress,
                                eLog.Value.Cc,
                                template.Subject,
                                template.Body,
                                fields,
                                mailSetting,
                                eLog.Value.MessageId,
                                log.HighPriority,
                                files,
                                siteSelfService,
                                siteHelpdesk,
                                emailType);

                        eLog.Value.SetResponse(mailResponse.SendTime, mailResponse.ResponseMessage);
                        var now = DateTime.Now;
                        eLog.Value.CreatedDate = now;
                        eLog.Value.ChangedDate = now;
                        _emailLogRepository.Add(eLog.Value);
                        _emailLogRepository.Commit();
                    }
                }
            }
        }

        private static EmailType GetEmailType(EmailSendMethod sendMethod, EmailLog eLog, string[] ccReciepients)
        {
            if (ccReciepients == null || !ccReciepients.Any()) return EmailType.ToMail;

            return sendMethod == EmailSendMethod.SeparateEmails &&
                   !string.IsNullOrEmpty(eLog.EmailAddress) &&
                   ccReciepients.Contains(eLog.EmailAddress)
                ? EmailType.CcMail
                : EmailType.ToMail;
        }
        
        public string GetExternalLogTextHistory(Case newCase, string helpdeskMailFromAdress, CaseLog log)
        {
            string extraBody = "";
            try
            {
                
                string userEmailToShow = helpdeskMailFromAdress;
                var curCustomer = _customerService.GetCustomer(newCase.Customer_Id);
                var customerTimeZone = TimeZoneInfo.FindSystemTimeZoneById(curCustomer.TimeZoneId);
                var correctedDate = new DateTime();
                var emailLogs = _emailLogRepository.GetEmailLogsByCaseId(newCase.Id).OrderByDescending(z => z.Id).ToList();
                var oldLogs = _logService.GetLogsByCaseId(newCase.Id).Where(x => x.Case_Id == log.CaseId && x.Id != log.Id).OrderByDescending(z => z.Id).ToList();
                if (oldLogs.Any())
                {
                    foreach (var post in oldLogs)
                    {
                        //checking if external lognote is not empty
                        if (post.Text_External != null && post.Text_External.Replace("<p><br></p>", "") != "")
                        {
                            if (post.Text_External.EndsWith("<div><p><b>"))
                            {
                                post.Text_External = post.Text_External.Replace("<div><p><b>", "");
                            }
                            post.Text_External = post.Text_External.Replace("<p><br></p>", "").Replace("<p>\r\n</p>", "").Replace("<o:p>&nbsp;</o:p>", "");

                            //Here i want to see if oldLogs contains an id corresponding LogId in EmailLog
                            var emailLogExists = emailLogs.FirstOrDefault(x => x.Log_Id != null && x.Log_Id == post.Id);

                            if (!String.IsNullOrEmpty(post.RegUser))
                            {
                                userEmailToShow = post.RegUser;
                            }
                            else if (emailLogExists != null && !String.IsNullOrEmpty(emailLogExists.From))
                            {
                                userEmailToShow = emailLogExists.From;
                            }
                            else
                            {
                                userEmailToShow = helpdeskMailFromAdress;
                            }
                            //Get corrected date and time for the customer
                            correctedDate = TimeZoneInfo.ConvertTimeFromUtc(post.LogDate, customerTimeZone);

                            extraBody += "<br /><hr>";
                            extraBody += "<div id=\"externalLogNotesHistory\">";
                            extraBody += "<font face=\"verdana\" size=\"2\"><strong>" + correctedDate.ToString("g") + "</strong>";
                            extraBody += "<br />" + userEmailToShow;
                            extraBody += "<br />" + post.Text_External + "</font></div>";
                        }
                    }
                    //Plus Extra body with Description from Case
                    //*Reported by is descided by this rule:
                    //1.User_ID
                    //2.RegUserName
                    //3.RegUserID
                    if (newCase.User_Id != null)
                    {
                        userEmailToShow = _userService.GetUser((int)newCase.User_Id).Email;
                    }
                    else if (!String.IsNullOrEmpty(newCase.RegUserName))
                    {
                        userEmailToShow = newCase.RegUserName;
                    }
                    else if (newCase.RegUserId != null)
                    {
                        //Check this
                        userEmailToShow = newCase.RegUserId;
                    }
                    else
                    {
                        userEmailToShow = helpdeskMailFromAdress;
                    }

                    correctedDate = TimeZoneInfo.ConvertTimeFromUtc(newCase.RegTime, customerTimeZone);
                    var description = newCase.Description.Replace("<p><br></p>", "").Replace("<p>\r\n</p>", "").Replace("<o:p>&nbsp;</o:p>", "");
                    extraBody += "<br /><hr>";
                    extraBody += "<div id=\"externalLogNotesDescription\">";
                    extraBody += "<font face=\"verdana\" size=\"2\"><strong>" + correctedDate.ToString("g") + "</strong>";
                    extraBody += "<br />" + userEmailToShow;
                    extraBody += "<br />" + description + "</font></div>";

                }
                return extraBody;

            }
            catch (Exception ex)
            {
                //Add to error log?
                return extraBody;
            }
        }
    }
}