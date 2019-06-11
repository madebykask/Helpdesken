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

        public CaseMailer(
            IEmailLogRepository emailLogRepository,
            IEmailService emailService,
            IMailTemplateService mailTemplateService,
            IEmailFactory emailFactory,
            IUserService userService,
            ISettingService settingService,
            IEmailSendingSettingsProvider emailSendingSettingsProvider,
            ICaseExtraFollowersService caseExtraFollowersService)
        {
            this._emailLogRepository = emailLogRepository;
            this._emailService = emailService;
            this._mailTemplateService = mailTemplateService;
            this._emailFactory = emailFactory;
            this._userService = userService;
            this._settingService = settingService;
            this._emailSendingSettingsProvider = emailSendingSettingsProvider;
            _caseExtraFollowersService = caseExtraFollowersService;
        }

        public void InformNotifierIfNeeded(
            int caseHistoryId,
            List<Field> fields,
            CaseLog log,
            bool dontSendMailToNotfier,
            Case newCase,
            string helpdeskMailFromAdress,
            List<string> files,
            MailSenders mailSenders,
            bool isCreatingCase,
            bool caseMailSetting_DontSendMail,
            string absoluterUrl,
            string extraFollowersEmails = null)
        {

            //if (!isCreatingCase)
            //{
            if (log == null || log.Id <= 0 || string.IsNullOrWhiteSpace(log.TextExternal) ||
                !log.SendMailAboutCaseToNotifier ||
                dontSendMailToNotfier ||
                (caseMailSetting_DontSendMail == false && newCase.FinishingDate != null))
            {
                return;
            }

            var template = this._mailTemplateService.GetMailTemplateForCustomerAndLanguage(
                                                newCase.Customer_Id,
                                                newCase.RegLanguage_Id,
                                                (int)GlobalEnums.MailTemplates.InformNotifier);
            if (template == null)
            {
                return;
            }

            var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName,
                customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            if (!string.IsNullOrEmpty(template.Body) && !String.IsNullOrEmpty(template.Subject))
            {
                var to = newCase.PersonsEmail.Split(';', ',').ToList();

                var allEmails = to.Select(x => new
                {
                    EmailAddress = x,
                    EmailType = EmailType.ToMail
                }).ToList();

                var extraFollowers = extraFollowersEmails != null
                    ? extraFollowersEmails.Split(';', ',').ToList()
                    : _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id).Select(x => x.Follower).ToList();

                var ccEmails = extraFollowers.Select(x => new
                {
                    EmailAddress = x,
                    EmailType = EmailType.CcMail
                }).ToList();

                allEmails.AddRange(ccEmails);

                foreach (var t in allEmails)
                {
                    var curMail = t.EmailAddress.Trim();
                    if (!string.IsNullOrWhiteSpace(curMail) && this._emailService.IsValidEmail(curMail))
                    {
                        var customEmailSender4 = mailSenders.DefaultOwnerWGEMail;
                        if (string.IsNullOrWhiteSpace(customEmailSender4))
                            customEmailSender4 = mailSenders.WGEmail;
                        if (string.IsNullOrWhiteSpace(customEmailSender4))
                            customEmailSender4 = mailSenders.SystemEmail;

                        var mailMessageId = this._emailService.GetMailMessageId(customEmailSender4);
                        var notifierEmailLog = this._emailFactory.CreatEmailLog(
                                                        caseHistoryId,
                                                        (int)GlobalEnums.MailTemplates.InformNotifier,
                                                        curMail,
                                                        mailMessageId);

                        var siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + notifierEmailLog.EmailLogGUID.ToString();
                        var mailResponse = EmailResponse.GetEmptyEmailResponse();
                        var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                        //var siteHelpdesk = AbsoluterUrl;
                        var siteHelpdesk = absoluterUrl + "Cases/edit/" + newCase.Id.ToString();
                        var notifierEmailItem = this._emailFactory.CreateEmailItem(
                                                        customEmailSender4,
                                                        notifierEmailLog.EmailAddress,
                                                        template.Subject,
                                                        template.Body,
                                                        fields,
                                                        notifierEmailLog.MessageId,
                                                        log.HighPriority,
                                                        files);
                        var e_res = this._emailService.SendEmail(notifierEmailLog, notifierEmailItem, mailSetting, siteSelfService, siteHelpdesk, t.EmailType);
                        notifierEmailLog.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                        var now = DateTime.Now;
                        notifierEmailLog.CreatedDate = now;
                        notifierEmailLog.ChangedDate = now;
                        this._emailLogRepository.Add(notifierEmailLog);
                        this._emailLogRepository.Commit();
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
            List<string> files,
            string absoluterUrl)
        {
            if (log == null || log.Id <= 0 ||
                !log.SendMailAboutCaseToNotifier ||
                dontSendMailToNotfier ||
                newCase.FinishingDate != null)
            {
                return;
            }

            var template = this._mailTemplateService.GetMailTemplateForCustomerAndLanguage(
                                                newCase.Customer_Id,
                                                newCase.RegLanguage_Id,
                                                (int)GlobalEnums.MailTemplates.InformNotifier);
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

            if (!String.IsNullOrEmpty(template.Body) && !String.IsNullOrEmpty(template.Subject))
            {
                var mailMessageId = this._emailService.GetMailMessageId(helpdeskMailFromAdress);

                // http://redmine.fastdev.se/issues/10997
                /*var caseOwner = this.userService.GetUser(newCase.User_Id);
                if (caseOwner == null || 
                    !caseOwner.Default_WorkingGroup_Id.HasValue)
                {
                    return;
                }

                var defaultWorkingGroup = this.workingGroupService.GetWorkingGroup(caseOwner.Default_WorkingGroup_Id.Value);
                if (defaultWorkingGroup == null || 
                    !this.emailService.IsValidEmail(defaultWorkingGroup.EMail))
                {
                    return;
                }*/

                var defaultWorkingGroup = this._userService.GetUserDefaultWorkingGroup(newCase.User_Id.Value, newCase.Customer_Id);
                if (defaultWorkingGroup == null ||
                    !this._emailService.IsValidEmail(defaultWorkingGroup.EMail))
                {
                    return;
                }

                var defaultWorkingGroupEmailLog = this._emailFactory.CreatEmailLog(
                                                caseHistoryId,
                                                (int)GlobalEnums.MailTemplates.InformNotifier,
                                                defaultWorkingGroup.EMail,
                                                mailMessageId);

                string site = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + defaultWorkingGroupEmailLog.EmailLogGUID.ToString();
                //var siteHelpdesk = AbsoluterUrl;
                var siteHelpdesk = absoluterUrl + "Cases/edit/" + newCase.Id.ToString();
                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                var defaultWorkingGroupEmailItem = this._emailFactory.CreateEmailItem(
                                                helpdeskMailFromAdress,
                                                defaultWorkingGroupEmailLog.EmailAddress,
                                                template.Subject,
                                                template.Body,
                                                fields,
                                                defaultWorkingGroupEmailLog.MessageId,
                                                log.HighPriority,
                                                files);
                var e_res = this._emailService.SendEmail(defaultWorkingGroupEmailLog, defaultWorkingGroupEmailItem, mailSetting, site, siteHelpdesk);
                defaultWorkingGroupEmailLog.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                var now = DateTime.Now;
                defaultWorkingGroupEmailLog.CreatedDate = now;
                defaultWorkingGroupEmailLog.ChangedDate = now;
                this._emailLogRepository.Add(defaultWorkingGroupEmailLog);
                this._emailLogRepository.Commit();
            }
        }

        public void InformAboutInternalLogIfNeeded(
            int caseHistoryId,
            List<Field> fields,
            CaseLog log,
            Case newCase,
            string helpdeskMailFromAdress,
            List<string> files, string absoluterUrl,
            MailSenders mailSenders)
        {
            if (log == null || log.Id <= 0 ||
                (string.IsNullOrWhiteSpace(log.EmailRecepientsInternalLogTo) &&
                string.IsNullOrWhiteSpace(log.EmailRecepientsInternalLogCc)))
            {
                return;
            }

            var template = this._mailTemplateService.GetMailTemplateForCustomerAndLanguage(
                                                newCase.Customer_Id,
                                                newCase.RegLanguage_Id,
                                                (int)GlobalEnums.MailTemplates.InternalLogNote);
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

            if (!String.IsNullOrEmpty(template.Body) && !String.IsNullOrEmpty(template.Subject))
            {

                string customEmailSender4 = mailSenders.DefaultOwnerWGEMail;
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

                var allEmails = to.Select(x => new
                {
                    EmailAdress = x,
                    EmailType = EmailType.ToMail
                }).ToList();
                var emailsCc = cc.Select(x => new
                {
                    EmailAdress = x,
                    EmailType = EmailType.CcMail
                }).ToList();
                allEmails.AddRange(emailsCc);

                foreach (var item in allEmails)
                {
                    if (!string.IsNullOrWhiteSpace(item.EmailAdress) && this._emailService.IsValidEmail(item.EmailAdress))
                    {
                        var internalEmailLog = this._emailFactory.CreatEmailLog(
                            caseHistoryId,
                            (int)GlobalEnums.MailTemplates.InternalLogNote,
                            item.EmailAdress,
                            this._emailService.GetMailMessageId(helpdeskMailFromAdress));
                        string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() +
                                                 internalEmailLog.EmailLogGUID.ToString();
                        var siteHelpdesk = absoluterUrl + "Cases/edit/" + newCase.Id.ToString();
                        var mailResponse = EmailResponse.GetEmptyEmailResponse();
                        var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                        var internalEmail = this._emailFactory.CreateEmailItem(
                            customEmailSender4,
                            internalEmailLog.EmailAddress,
                            template.Subject,
                            template.Body,
                            fields,
                            internalEmailLog.MessageId,
                            log.HighPriority,
                            files);
                        mailResponse = this._emailService.SendEmail(internalEmailLog, internalEmail, mailSetting, siteSelfService,
                            siteHelpdesk, item.EmailType);
                        internalEmailLog.SetResponse(mailResponse.SendTime, mailResponse.ResponseMessage);
                        var now = DateTime.Now;
                        internalEmailLog.CreatedDate = now;
                        internalEmailLog.ChangedDate = now;
                        this._emailLogRepository.Add(internalEmailLog);
                        this._emailLogRepository.Commit();
                    }
                }
            }
        }
    }
}