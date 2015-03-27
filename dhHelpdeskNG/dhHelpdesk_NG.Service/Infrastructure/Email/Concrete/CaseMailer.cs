namespace DH.Helpdesk.Services.Infrastructure.Email.Concrete
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Dal.Infrastructure.ModelFactories.Email;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using System.Configuration;

    public sealed class CaseMailer : ICaseMailer
    {
        private readonly IEmailLogRepository emailLogRepository;

        private readonly IEmailService emailService;

        private readonly IMailTemplateService mailTemplateService;

        private readonly IEmailFactory emailFactory;

        private readonly IUserService userService;

        private readonly IWorkingGroupService workingGroupService;

        public CaseMailer(
            IEmailLogRepository emailLogRepository, 
            IEmailService emailService, 
            IMailTemplateService mailTemplateService, 
            IEmailFactory emailFactory, 
            IUserService userService, 
            IWorkingGroupService workingGroupService)
        {
            this.emailLogRepository = emailLogRepository;
            this.emailService = emailService;
            this.mailTemplateService = mailTemplateService;
            this.emailFactory = emailFactory;
            this.userService = userService;
            this.workingGroupService = workingGroupService;
        }

        public void InformNotifierIfNeeded(
            int caseHistoryId,
            List<Field> fields,
            CaseLog log, 
            bool dontSendMailToNotfier, 
            Case newCase, 
            string helpdeskMailFromAdress, 
            List<string> files,
            MailSenders mailSenders)
        {
            if (log == null || log.Id <= 0 || string.IsNullOrWhiteSpace(log.TextExternal) ||
                !log.SendMailAboutCaseToNotifier ||
                dontSendMailToNotfier ||
                !this.emailService.IsValidEmail(newCase.PersonsEmail) ||
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

            string customEmailSender4 = mailSenders.DefaultOwnerWGEMail;
            if (string.IsNullOrWhiteSpace(customEmailSender4))
                customEmailSender4 = mailSenders.WGEmail;
            if (string.IsNullOrWhiteSpace(customEmailSender4))
                customEmailSender4 = mailSenders.SystemEmail;

            var mailMessageId = this.emailService.GetMailMessageId(customEmailSender4);
            var notifierEmailLog = this.emailFactory.CreatEmailLog(
                                            caseHistoryId,
                                            (int)GlobalEnums.MailTemplates.InformNotifier,
                                            newCase.PersonsEmail,
                                            mailMessageId);

            string site = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + notifierEmailLog.EmailLogGUID.ToString();  
            string url = "<br><a href='" + site + "'>" + site + "</a>";
            foreach (var field in fields)
                if (field.Key == "[#98]")
                    field.StringValue = url;            

            var notifierEmailItem = this.emailFactory.CreateEmailItem(
                                            customEmailSender4,
                                            notifierEmailLog.EmailAddress,
                                            template.Subject,
                                            template.Body,
                                            fields,
                                            notifierEmailLog.MessageId,
                                            log.HighPriority,
                                            files);
            this.emailService.SendEmail(notifierEmailItem);
            this.emailLogRepository.Add(notifierEmailLog);
            this.emailLogRepository.Commit();
        }

        public void InformOwnerDefaultGroupIfNeeded(
            int caseHistoryId,
            List<Field> fields,
            CaseLog log,
            bool dontSendMailToNotfier,
            Case newCase,
            string helpdeskMailFromAdress,
            List<string> files)
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

            var mailMessageId = this.emailService.GetMailMessageId(helpdeskMailFromAdress);

            var caseOwner = this.userService.GetUser(newCase.User_Id);
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
            }

            var defaultWorkingGroupEmailLog = this.emailFactory.CreatEmailLog(
                                            caseHistoryId,
                                            (int)GlobalEnums.MailTemplates.InformNotifier,
                                            defaultWorkingGroup.EMail,
                                            mailMessageId);

            string site = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + defaultWorkingGroupEmailLog.EmailLogGUID.ToString();
            string url = "<br><a href='" + site + "'>" + site + "</a>";
            foreach (var field in fields)
                if (field.Key == "[#98]")
                    field.StringValue = url;

            var defaultWorkingGroupEmailItem = this.emailFactory.CreateEmailItem(
                                            helpdeskMailFromAdress,
                                            defaultWorkingGroupEmailLog.EmailAddress,
                                            template.Subject,
                                            template.Body,
                                            fields,
                                            defaultWorkingGroupEmailLog.MessageId,
                                            log.HighPriority,
                                            files);
            this.emailService.SendEmail(defaultWorkingGroupEmailItem);
            this.emailLogRepository.Add(defaultWorkingGroupEmailLog);
            this.emailLogRepository.Commit();
        }

        public void InformAboutInternalLogIfNeeded(
            int caseHistoryId,
            List<Field> fields,
            CaseLog log,
            Case newCase,
            string helpdeskMailFromAdress,
            List<string> files)
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

            var to = log.EmailRecepientsInternalLog
                                .Replace(Environment.NewLine, "|")
                                .Split('|');
            foreach (var t in to)
            {
                if (!string.IsNullOrWhiteSpace(t) && this.emailService.IsValidEmail(t))
                {
                    var internalEmailLog = this.emailFactory.CreatEmailLog(
                                                    caseHistoryId,
                                                    (int)GlobalEnums.MailTemplates.InternalLogNote,
                                                    t,
                                                    this.emailService.GetMailMessageId(helpdeskMailFromAdress));
                    string site = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + internalEmailLog.EmailLogGUID.ToString();
                    string url = "<br><a href='" + site + "'>" + site + "</a>";
                    foreach (var field in fields)
                        if (field.Key == "[#98]")
                            field.StringValue = url;

                    var internalEmail = this.emailFactory.CreateEmailItem(
                                                    helpdeskMailFromAdress,
                                                    internalEmailLog.EmailAddress,
                                                    template.Subject,
                                                    template.Body,
                                                    fields,
                                                    internalEmailLog.MessageId,
                                                    log.HighPriority,
                                                    files);
                    this.emailService.SendEmail(internalEmail);
                    this.emailLogRepository.Add(internalEmailLog);
                    this.emailLogRepository.Commit();
                }
            }
        }
    }
}