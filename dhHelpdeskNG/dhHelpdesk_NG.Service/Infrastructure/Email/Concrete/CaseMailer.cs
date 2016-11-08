using System;
using System.Collections.Generic;

using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using System.Configuration;
using DH.Helpdesk.BusinessData.Models.Email;

namespace DH.Helpdesk.Services.Infrastructure.Email.Concrete
{

    public sealed class CaseMailer : ICaseMailer
    {
        private readonly IEmailLogRepository emailLogRepository;

        private readonly IEmailService emailService;

        private readonly IMailTemplateService mailTemplateService;

        private readonly IEmailFactory emailFactory;

        private readonly IUserService userService;

        private readonly IWorkingGroupService workingGroupService;

        private readonly ISettingService settingService;

        private readonly IEmailSendingSettingsProvider _emailSendingSettingsProvider;

        public CaseMailer(
            IEmailLogRepository emailLogRepository, 
            IEmailService emailService, 
            IMailTemplateService mailTemplateService, 
            IEmailFactory emailFactory, 
            IUserService userService, 
            IWorkingGroupService workingGroupService,
            ISettingService settingService,
            IEmailSendingSettingsProvider emailSendingSettingsProvider)
        {
            this.emailLogRepository = emailLogRepository;
            this.emailService = emailService;
            this.mailTemplateService = mailTemplateService;
            this.emailFactory = emailFactory;
            this.userService = userService;
            this.workingGroupService = workingGroupService;
            this.settingService = settingService;
            this._emailSendingSettingsProvider = emailSendingSettingsProvider;
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
            string AbsoluterUrl)
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

                var template = this.mailTemplateService.GetMailTemplateForCustomerAndLanguage(
                                                    newCase.Customer_Id,
                                                    newCase.RegLanguage_Id,
                                                    (int)GlobalEnums.MailTemplates.InformNotifier);
                if (template == null)
                {
                    return;
                }

                var customerSetting = settingService.GetCustomerSetting(newCase.Customer_Id);
                var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

                if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
                {
                    var info = _emailSendingSettingsProvider.GetSettings();
                    smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
                }

            if (!String.IsNullOrEmpty(template.Body) && !String.IsNullOrEmpty(template.Subject))
                {
                    var to = newCase.PersonsEmail.Split(';', ',');
                    foreach (var t in to)
                    {
                        var curMail = t.Trim();
                        if (!string.IsNullOrWhiteSpace(curMail) && this.emailService.IsValidEmail(curMail))
                        {
                            string customEmailSender4 = mailSenders.DefaultOwnerWGEMail;
                            if (string.IsNullOrWhiteSpace(customEmailSender4))
                                customEmailSender4 = mailSenders.WGEmail;
                            if (string.IsNullOrWhiteSpace(customEmailSender4))
                                customEmailSender4 = mailSenders.SystemEmail;

                            var mailMessageId = this.emailService.GetMailMessageId(customEmailSender4);
                            var notifierEmailLog = this.emailFactory.CreatEmailLog(
                                                            caseHistoryId,
                                                            (int)GlobalEnums.MailTemplates.InformNotifier,
                                                            curMail,
                                                            mailMessageId);

                            string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + notifierEmailLog.EmailLogGUID.ToString();
                            var mailResponse = EmailResponse.GetEmptyEmailResponse();
                            var mailSetting = new EmailSettings(mailResponse, smtpInfo);
                            //var siteHelpdesk = AbsoluterUrl;
                            var siteHelpdesk = AbsoluterUrl + "Cases/edit/" + newCase.Id.ToString();
                            var notifierEmailItem = this.emailFactory.CreateEmailItem(
                                                            customEmailSender4,
                                                            notifierEmailLog.EmailAddress,
                                                            template.Subject,
                                                            template.Body,
                                                            fields,
                                                            notifierEmailLog.MessageId,
                                                            log.HighPriority,
                                                            files);
                            var e_res = this.emailService.SendEmail(notifierEmailItem, mailSetting, siteSelfService, siteHelpdesk);
                            notifierEmailLog.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                            var now = DateTime.Now;
                            notifierEmailLog.CreatedDate = now;
                            notifierEmailLog.ChangedDate = now;
                            this.emailLogRepository.Add(notifierEmailLog);
                            this.emailLogRepository.Commit();
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
            string AbsoluterUrl)
        {
            if (log == null || log.Id <= 0 ||
                !log.SendMailAboutCaseToNotifier ||
                dontSendMailToNotfier ||
                newCase.FinishingDate != null)
            {
                return;
            }

            var template = this.mailTemplateService.GetMailTemplateForCustomerAndLanguage(
                                                newCase.Customer_Id,
                                                newCase.RegLanguage_Id,
                                                (int)GlobalEnums.MailTemplates.InformNotifier);
            if (template == null)
            {
                return;
            }

            var customerSetting = settingService.GetCustomerSetting(newCase.Customer_Id);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            if (!String.IsNullOrEmpty(template.Body) && !String.IsNullOrEmpty(template.Subject))
            {
                var mailMessageId = this.emailService.GetMailMessageId(helpdeskMailFromAdress);

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

                var defaultWorkingGroup = this.userService.GetUserDefaultWorkingGroup(newCase.User_Id.Value, newCase.Customer_Id);
                if (defaultWorkingGroup == null ||
                    !this.emailService.IsValidEmail(defaultWorkingGroup.EMail))
                {
                    return;
                }

                var defaultWorkingGroupEmailLog = this.emailFactory.CreatEmailLog(
                                                caseHistoryId,
                                                (int)GlobalEnums.MailTemplates.InformNotifier,
                                                defaultWorkingGroup.EMail,
                                                mailMessageId);

                string site = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + defaultWorkingGroupEmailLog.EmailLogGUID.ToString();
                //var siteHelpdesk = AbsoluterUrl;
                var siteHelpdesk = AbsoluterUrl + "Cases/edit/" + newCase.Id.ToString();
                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                var mailSetting = new EmailSettings(mailResponse, smtpInfo);
                var defaultWorkingGroupEmailItem = this.emailFactory.CreateEmailItem(
                                                helpdeskMailFromAdress,
                                                defaultWorkingGroupEmailLog.EmailAddress,
                                                template.Subject,
                                                template.Body,
                                                fields,
                                                defaultWorkingGroupEmailLog.MessageId,
                                                log.HighPriority,
                                                files);
                var e_res = this.emailService.SendEmail(defaultWorkingGroupEmailItem, mailSetting, site, siteHelpdesk);
                defaultWorkingGroupEmailLog.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                var now = DateTime.Now;
                defaultWorkingGroupEmailLog.CreatedDate = now;
                defaultWorkingGroupEmailLog.ChangedDate = now;
                this.emailLogRepository.Add(defaultWorkingGroupEmailLog);
                this.emailLogRepository.Commit();
            }
        }

        public void InformAboutInternalLogIfNeeded(
            int caseHistoryId,
            List<Field> fields,
            CaseLog log,
            Case newCase,
            string helpdeskMailFromAdress,
            List<string> files, string AbsoluterUrl,
            MailSenders mailSenders)
        {
            if (log == null || log.Id <= 0 ||
                !log.SendMailAboutLog ||
                string.IsNullOrWhiteSpace(log.EmailRecepientsInternalLog))
            {
                return;
            }

            var template = this.mailTemplateService.GetMailTemplateForCustomerAndLanguage(
                                                newCase.Customer_Id,
                                                newCase.RegLanguage_Id,
                                                (int)GlobalEnums.MailTemplates.InternalLogNote);
            if (template == null)
            {
                return;
            }

            var customerSetting = settingService.GetCustomerSetting(newCase.Customer_Id);
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

                var to = log.EmailRecepientsInternalLog
                                    .Replace(" ", "")
                                    .Replace(Environment.NewLine, "|")
                                    .Split('|', ';', ',');
               
                foreach (var t in to)
                {
                    if (!string.IsNullOrWhiteSpace(t) && this.emailService.IsValidEmail(t))
                    {
                        var internalEmailLog = this.emailFactory.CreatEmailLog(
                                                        caseHistoryId,
                                                        (int)GlobalEnums.MailTemplates.InternalLogNote,
                                                        t,
                                                        this.emailService.GetMailMessageId(helpdeskMailFromAdress));
                        string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + internalEmailLog.EmailLogGUID.ToString();
                        //var siteHelpdesk = AbsoluterUrl;
                        var siteHelpdesk = AbsoluterUrl + "Cases/edit/" + newCase.Id.ToString();
                        var mailResponse = EmailResponse.GetEmptyEmailResponse();
                        var mailSetting = new EmailSettings(mailResponse, smtpInfo);
                        var internalEmail = this.emailFactory.CreateEmailItem(
                                                        customEmailSender4,
                                                        internalEmailLog.EmailAddress,
                                                        template.Subject,
                                                        template.Body,
                                                        fields,
                                                        internalEmailLog.MessageId,
                                                        log.HighPriority,
                                                        files);
                        var e_res = this.emailService.SendEmail(internalEmail, mailSetting, siteSelfService, siteHelpdesk);
                        internalEmailLog.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                        var now = DateTime.Now;
                        internalEmailLog.CreatedDate = now;
                        internalEmailLog.ChangedDate = now;
                        this.emailLogRepository.Add(internalEmailLog);
                        this.emailLogRepository.Commit();
                    }
                }
            }
        }
    }
}