using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Email;
using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Email;
using DH.Helpdesk.BusinessData.Models.Feedback;
using DH.Helpdesk.BusinessData.Models.MailTemplates;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.Logs;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Common.Extensions.Lists;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.MailTemplates;
using DH.Helpdesk.Services.BusinessLogic.Mappers.Feedback;
using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Services.Services
{
    public partial class CaseService
    {
        public void SendCaseEmail(
            int caseId,
            CaseMailSetting cms,
            int caseHistoryId,
            string basePath,
            TimeZoneInfo userTimeZone,
            Case oldCase = null,
            CaseLog log = null,
            List<CaseLogFileDto> logFiles = null,
            User currentLoggedInUser = null,
            string extraFollowersEmails = null)
        {
            //get sender email adress
            var helpdeskMailFromAdress = string.Empty;
            var informNotifierHasBeenSent = false;

            if (!string.IsNullOrEmpty(cms.HelpdeskMailFromAdress))
            {
                helpdeskMailFromAdress = cms.HelpdeskMailFromAdress.Trim();
            }

            if (!IsValidEmail(helpdeskMailFromAdress))
            {
                return;
            }

            // get new case information
            var newCase = _caseRepository.GetDetachedCaseById(caseId);
            var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id);
            var smtpInfo = CreateSmtpSettings(customerSetting);

            var dontSendMailToNotfier = false;
            var isCreatingCase = oldCase == null || oldCase.Id == 0;
            var isClosingCase = newCase.FinishingDate != null;

            // get list of fields to replace [#1] tags in the subjcet and body texts
            var fields = GetCaseFieldsForEmail(newCase, log, cms, string.Empty, 0, userTimeZone);

            // if logfiles should be attached to the mail 
            List<MailFile> files = null;
            if (logFiles != null && logFiles.Any() && log != null)
            {
                files = PrepareAttachedFiles(logFiles, basePath);
            }

            // sub state should not generate email to notifier
            dontSendMailToNotfier = newCase.StateSecondary?.NoMailToNotifier == 1;
            var customEmailSender1 = GetWgOrSystemEmailSender(cms);

            if (!isClosingCase && isCreatingCase)
            {
                #region Send email about new case to notifier or tblCustomer.NewCaseEmailList & (productarea template, priority template)

                informNotifierHasBeenSent = SendNewCaseMail(cms, caseHistoryId, userTimeZone, log, newCase, customerSetting,
                    customEmailSender1, files, informNotifierHasBeenSent);

                #region Send mail template from productArea if productarea has a mailtemplate

                if (newCase.ProductArea_Id.HasValue && newCase.ProductArea != null)
                {
                    informNotifierHasBeenSent = SendCaseProductAreaMail(cms, caseHistoryId, userTimeZone, log, newCase,
                        dontSendMailToNotfier,
                        customerSetting, customEmailSender1, files, informNotifierHasBeenSent);
                }

                #endregion

                #region If priority has value and an emailaddress

                if (newCase.Priority != null && !string.IsNullOrWhiteSpace(newCase.Priority.EMailList))
                {
                    SendAssignedCaseToPriorityMail(cms, caseHistoryId, userTimeZone, log, newCase, customerSetting,
                        helpdeskMailFromAdress, files, 5);
                }

                #endregion

                #endregion
            }
            else
            {
                #region Send template email if priority has value and Internal or External log is filled

                if (newCase.Priority != null)
                {
                    if (!string.IsNullOrWhiteSpace(newCase.Priority.EMailList) && log != null && !string.IsNullOrEmpty(log.TextExternal))
                    {
                        SendPriorityMailSpecial(newCase, log, cms, files, helpdeskMailFromAdress, caseHistoryId, customerSetting, smtpInfo, userTimeZone);
                    }
                    else
                    {
                        if (log != null && (!string.IsNullOrEmpty(log.TextExternal) || !string.IsNullOrEmpty(log.TextInternal)))
                        {
                            var caseHistoryPriorityId = GetLastButOneCaseHistoryPriorityId(caseId);
                            if (caseHistoryPriorityId > 0)
                            {
                                var prevPriority = _priorityService.GetPriority(caseHistoryPriorityId);
                                if (!string.IsNullOrWhiteSpace(prevPriority.EMailList))
                                {
                                    var copyNewCase = _caseRepository.GetDetachedCaseById(caseId);
                                    copyNewCase.Priority = prevPriority;

                                    SendPriorityMailSpecial(copyNewCase, log, cms, files, helpdeskMailFromAdress, caseHistoryId, customerSetting, smtpInfo, userTimeZone);
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            #region Send mail template from productArea if productarea has a mailtemplate

            if (!isCreatingCase && !isClosingCase)
            {
                if (newCase.ProductArea_Id.HasValue && newCase.ProductArea != null && oldCase.ProductAreaSetDate == null)
                {
                    informNotifierHasBeenSent = SendCaseProductAreaMail(cms, caseHistoryId, userTimeZone, log, newCase,
                        dontSendMailToNotfier,
                        customerSetting, customEmailSender1, files, informNotifierHasBeenSent);
                }
            }

            #endregion

            #region Send email to tblCase.Performer_User_Id

            if ((!isClosingCase && isCreatingCase && newCase.Performer_User_Id.HasValue ||
                 !isCreatingCase && newCase.Performer_User_Id.HasValue))
            {
                var admin = _userRepository.GetUserInfo(newCase.Performer_User_Id.Value);
                var sentToAdmin = false;

                if (oldCase == null || newCase.Performer_User_Id != oldCase.Performer_User_Id)
                {

                    if (admin.AllocateCaseMail == 1 && IsValidEmail(admin.Email))
                    {
                        var emailList = new List<string> { admin.Email };
                        if (currentLoggedInUser != null)
                        {
                            if (currentLoggedInUser.SettingForNoMail == 1 ||
                               (currentLoggedInUser.Id == newCase.Performer_User_Id && currentLoggedInUser.SettingForNoMail == 1) ||
                               (currentLoggedInUser.Id != newCase.Performer_User_Id && currentLoggedInUser.SettingForNoMail == 0))
                            {
                                SendTemplateEmail((int)GlobalEnums.MailTemplates.AssignedCaseToUser, newCase, log, caseHistoryId,
                                    customerSetting, cms, cms.HelpdeskMailFromAdress, emailList, userTimeZone, null, 3);
                                sentToAdmin = true;
                            }
                        }
                        else
                        {
                            SendTemplateEmail((int)GlobalEnums.MailTemplates.AssignedCaseToUser, newCase, log, caseHistoryId,
                                customerSetting, cms, cms.HelpdeskMailFromAdress, emailList, userTimeZone, null, 3);
                            sentToAdmin = true;
                        }
                    }
                }

                if (log.SendMailAboutCaseToPerformer && !sentToAdmin)
                {
                    this.HandleSendMailAboutCaseToPerformer(admin, currentLoggedInUser.Id, log);
                }

                // send sms to tblCase.Performer_User_Id 
                if (admin.AllocateCaseSMS == 1 &&
                    !string.IsNullOrWhiteSpace(admin.CellPhone) &&
                    newCase.Customer != null)
                {
                    var emailList = new List<string>
                    {
                        GetSmsRecipient(customerSetting, admin.CellPhone)
                    };

                    SendTemplateEmail((int)GlobalEnums.MailTemplates.SmsAssignedCaseToUser,
                        newCase,
                        log,
                        caseHistoryId,
                        customerSetting,
                        cms,
                        helpdeskMailFromAdress,
                        emailList,
                        userTimeZone,
                        files,
                        4);
                }
            }

            #endregion

            #region Send email priority has changed

            if (!isClosingCase && oldCase != null && oldCase.Id > 0 && newCase.Priority_Id != oldCase.Priority_Id)
            {
                if (newCase.Priority_Id != null && !string.IsNullOrWhiteSpace(newCase.Priority.EMailList))
                {
                    SendAssignedCaseToPriorityMail(cms, caseHistoryId, userTimeZone, log, newCase, customerSetting, helpdeskMailFromAdress, files, 1);
                }
            }

            #endregion

            #region Send email working group has changed

            if (!isClosingCase
                && newCase.Workinggroup != null
                && (isCreatingCase || !isCreatingCase && newCase.WorkingGroup_Id != oldCase.WorkingGroup_Id))
            {
                var wgEmails = GetCaseWorkingGropsEmails(newCase);

                if (newCase.Workinggroup.AllocateCaseMail == 1 && wgEmails.Any())
                {
                    SendTemplateEmail((int)GlobalEnums.MailTemplates.AssignedCaseToWorkinggroup,
                        newCase,
                        log,
                        caseHistoryId,
                        customerSetting,
                        cms,
                        helpdeskMailFromAdress,
                        wgEmails,
                        userTimeZone,
                        files,
                        6);
                }
            }

            #endregion

            #region Send email when product area is set

            var productAreaMailTemplateId = newCase?.ProductArea?.MailTemplate?.MailID ?? 0;

            if (!isClosingCase && !isCreatingCase && !informNotifierHasBeenSent
                && oldCase.ProductAreaSetDate == null && newCase.RegistrationSource == 3
                && !cms.DontSendMailToNotifier && productAreaMailTemplateId > 0
                && !string.IsNullOrEmpty(newCase.PersonsEmail))
            {
                if (productAreaMailTemplateId > 0)
                {
                    var emailList = GetCaseFollowersEmails(newCase);
                    SendTemplateEmail((int)GlobalEnums.MailTemplates.AssignedCaseToWorkinggroup,
                        newCase,
                        log,
                        caseHistoryId,
                        customerSetting,
                        cms,
                        helpdeskMailFromAdress,
                        emailList[EmailType.ToMail],
                        userTimeZone,
                        files,
                        7);
                }
            }

            #endregion

            #region Case closed email

            if (newCase.FinishingDate.HasValue && newCase.Customer != null)
            {
                SendCaseClosedEmail(newCase, cms, caseHistoryId, userTimeZone, log, files, customerSetting, false, dontSendMailToNotfier, helpdeskMailFromAdress);
            }

            #endregion

            if (!informNotifierHasBeenSent)
            {
                _caseMailer.InformNotifierIfNeeded(
                    caseHistoryId,
                    fields,
                    log,
                    dontSendMailToNotfier,
                    newCase,
                    helpdeskMailFromAdress,
                    files,
                    cms.CustomeMailFromAddress,
                    isCreatingCase,
                    cms.DontSendMailToNotifier,
                    cms.AbsoluterUrl,
                    extraFollowersEmails);
            }

            _caseMailer.InformAboutInternalLogIfNeeded(caseHistoryId,
                fields,
                log,
                newCase,
                helpdeskMailFromAdress,
                files,
                cms.AbsoluterUrl,
                cms.CustomeMailFromAddress);

            #region SelfService - Notifier updated a case 

            var isSelfService = IsSelfService();

            if (isSelfService && log != null && log.Id > 0)
            {
                var isCaseActivated = oldCase != null && oldCase.FinishingDate.HasValue;

                SendSelfServiceCaseLogEmail(
                    newCase.Id,
                    cms,
                    caseHistoryId,
                    log,
                    basePath,
                    userTimeZone,
                    logFiles,
                    isCaseActivated);
            }

            #endregion
        }

        public void SendProblemLogEmail(Case c, CaseMailSetting cms, int caseHistoryId, TimeZoneInfo userTimeZone, CaseLog caseLog, bool isClosedCaseSending)
        {
            var cs = _caseRepository.GetDetachedCaseById(c.Id);
            var customerSetting = _settingService.GetCustomerSetting(cs.Customer_Id);

            var fields = GetCaseFieldsForEmail(cs, caseLog, cms, string.Empty, 0, userTimeZone);

            if (isClosedCaseSending)
            {
                SendCaseClosedEmail(c, cms, caseHistoryId, userTimeZone, caseLog, null, customerSetting, true);
            }
            else
            {
                if (!string.IsNullOrEmpty(caseLog.TextExternal))
                {
                    var helpdeskMailFromAdress = string.Empty;
                    if (!string.IsNullOrEmpty((cms.HelpdeskMailFromAdress)))
                    {
                        helpdeskMailFromAdress = cms.HelpdeskMailFromAdress.Trim();
                    }
                    if (!IsValidEmail(helpdeskMailFromAdress))
                    {
                        return;
                    }
                    _caseMailer.InformNotifierIfNeeded(caseHistoryId, fields, caseLog, false, c, helpdeskMailFromAdress, null, cms.CustomeMailFromAddress, false, cms.DontSendMailToNotifier, cms.AbsoluterUrl);
                }
            }
        }

        public void SendMergedCaseEmail(Case mergedCase, Case mergeParent, CaseMailSetting cms, int caseHistoryId, TimeZoneInfo userTimeZone, CaseLog caseLog, IList<string> ccEmailList)
        {
            SendMergedAndClosedCaseMail(mergedCase, mergeParent, cms, caseHistoryId, userTimeZone, caseLog, ccEmailList);
        }

        public void SendSelfServiceCaseLogEmail(int caseId,
                CaseMailSetting cms,
                int caseHistoryId,
                CaseLog log,
                string basePath,
                TimeZoneInfo userTimeZone,
                List<CaseLogFileDto> logFiles = null,
                bool caseIsActivated = false)
        {
            // get new case information
            var newCase = _caseRepository.GetDetachedCaseById(caseId);
            var mailTemplateId = 0;
            var performerUserEmail = string.Empty;
            var externalUpdateMail = 0;

            //get performerUser emailaddress
            if (newCase.Performer_User_Id.HasValue)
            {
                var performerUser = _userService.GetUser(newCase.Performer_User_Id.Value);
                performerUserEmail = performerUser.Email;
                externalUpdateMail = performerUser.ExternalUpdateMail;
            }

            mailTemplateId = !caseIsActivated
                ? (int)GlobalEnums.MailTemplates.CaseIsUpdated
                : (int)GlobalEnums.MailTemplates.CaseIsActivated;

            var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id);

            var helpdeskMailFromAdress = cms.HelpdeskMailFromAdress;
            if (newCase.Workinggroup != null && !string.IsNullOrWhiteSpace(newCase.Workinggroup.EMail) && IsValidEmail(newCase.Workinggroup.EMail))
            {
                helpdeskMailFromAdress = newCase.Workinggroup.EMail;
            }

            if (IsValidEmail(helpdeskMailFromAdress))
            {
                // get sender email address
                if (!string.IsNullOrWhiteSpace(newCase.Workinggroup?.EMail) && IsValidEmail(newCase.Workinggroup.EMail))
                {
                    helpdeskMailFromAdress = newCase.Workinggroup.EMail;
                }

                // if logfiles should be attached to the mail 
                List<MailFile> files = null;
                if (logFiles != null && log != null && logFiles.Count > 0)
                {
                    files = PrepareAttachedFiles(logFiles, basePath);
                }

                // Inform notifier about external lognote
                if (!string.IsNullOrEmpty(performerUserEmail) &&
                    log != null && (!string.IsNullOrEmpty(log.TextExternal) || !string.IsNullOrEmpty(log.TextInternal)) &&
                    newCase.FinishingDate == null &&
                    externalUpdateMail == 1)
                {
                    //admin emails
                    var emailList = performerUserEmail.Split(';', ',').ToDistintList(true);

                    SendTemplateEmail(mailTemplateId,
                        newCase,
                        log,
                        caseHistoryId,
                        customerSetting,
                        cms,
                        helpdeskMailFromAdress,
                        emailList,
                        userTimeZone,
                        files,
                        99,
                        log.HighPriority);
                }

                // mail about lognote to Working Group User or Working Group Mail
                if ((!string.IsNullOrEmpty(log.EmailRecepientsInternalLogTo) || !string.IsNullOrEmpty(log.EmailRecepientsInternalLogCc)) &&
                    !string.IsNullOrWhiteSpace(log.EmailRecepientsExternalLog))
                {
                    var emailList = log.EmailRecepientsExternalLog.Replace(Environment.NewLine, "|").Split('|').ToDistintList(true);

                    SendTemplateEmail(mailTemplateId,
                        newCase,
                        log,
                        caseHistoryId,
                        customerSetting,
                        cms,
                        helpdeskMailFromAdress,
                        emailList,
                        userTimeZone,
                        files,
                        99,
                        log.HighPriority);
                }
            }
        }

        #region Private Methods

        private bool SendNewCaseMail(CaseMailSetting cms, int caseHistoryId, TimeZoneInfo userTimeZone, CaseLog log,
            Case newCase, Setting customerSetting, string customEmailSender1, List<MailFile> files, bool informNotifierHasBeenSent)
        {
            // get mail template 
            var mailTemplateId = (int)GlobalEnums.MailTemplates.NewCase;

            var mailTpl =
                _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id,
                    mailTemplateId);

            if (mailTpl != null)
            {
                if (!string.IsNullOrEmpty(mailTpl.Body) && !string.IsNullOrEmpty(mailTpl.Subject))
                {
                    if (!cms.DontSendMailToNotifier && !string.IsNullOrEmpty(newCase.PersonsEmail))
                    {
                        var emailList = GetCaseFollowersEmails(newCase, false);
                        SendTemplateEmail(mailTemplateId,
                            mailTpl,
                            newCase,
                            log,
                            caseHistoryId,
                            customerSetting,
                            cms,
                            customEmailSender1,
                            emailList[EmailType.ToMail],
                            userTimeZone,
                            files,
                            1,
                            null,
                            false,
                            emailList[EmailType.CcMail]);

                        informNotifierHasBeenSent = true;
                    }

                    if (!string.IsNullOrWhiteSpace(cms.SendMailAboutNewCaseTo))
                    {
                        var emailList = cms.SendMailAboutNewCaseTo.Split(';').ToDistintList(true);
                        SendTemplateEmail(mailTemplateId,
                            mailTpl,
                            newCase,
                            log,
                            caseHistoryId,
                            customerSetting,
                            cms,
                            customEmailSender1,
                            emailList,
                            userTimeZone,
                            files,
                            2);
                    }
                }
            }

            return informNotifierHasBeenSent;
        }

        private bool SendCaseProductAreaMail(CaseMailSetting cms, int caseHistoryId, TimeZoneInfo userTimeZone, CaseLog log,
            Case caseData, bool dontSendMailToNotfier, Setting customerSetting, string customEmailSender1, List<MailFile> files,
            bool informNotifierHasBeenSent)
        {
            if (caseData.ProductArea_Id.HasValue && caseData.ProductArea != null)
            {
                // get mail template from productArea
                var mailTemplateId = 0;

                if (caseData.ProductArea.MailTemplate != null)
                    mailTemplateId = caseData.ProductArea.MailTemplate.MailID;

                if (mailTemplateId > 0)
                {
                    if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier &&
                        !string.IsNullOrEmpty(caseData.PersonsEmail))
                    {
                        var productAreaMailTemplate =
                            _mailTemplateService.GetMailTemplateForCustomerAndLanguage(caseData.Customer_Id,
                                caseData.RegLanguage_Id, mailTemplateId);

                        if (productAreaMailTemplate != null)
                        {
                            var emailList = GetCaseFollowersEmails(caseData);
                            SendTemplateEmail(mailTemplateId,
                                productAreaMailTemplate,
                                caseData,
                                log,
                                caseHistoryId,
                                customerSetting,
                                cms,
                                customEmailSender1,
                                emailList[EmailType.ToMail],
                                userTimeZone,
                                files,
                                1);

                            informNotifierHasBeenSent = true;
                        }
                    }
                }
            }

            return informNotifierHasBeenSent;
        }

        private void SendAssignedCaseToPriorityMail(CaseMailSetting cms, int caseHistoryId, TimeZoneInfo userTimeZone, CaseLog log,
            Case caseData, Setting customerSetting, string helpdeskMailFromAdress, List<MailFile> files, int stateHelper)
        {
            if (caseData.Priority != null && !string.IsNullOrWhiteSpace(caseData.Priority.EMailList))
            {
                var emailList = caseData.Priority.EMailList.Split(';', ',').ToDistintList(true);

                SendTemplateEmail((int)GlobalEnums.MailTemplates.AssignedCaseToPriority,
                        caseData,
                        log,
                        caseHistoryId,
                        customerSetting,
                        cms,
                        helpdeskMailFromAdress,
                        emailList,
                        userTimeZone,
                        files,
                        stateHelper);
            }
        }

        private int GetLastButOneCaseHistoryPriorityId(int caseId)
        {
            return _caseHistoryRepository.GetCaseHistoryByCaseId(caseId)
                       .AsQueryable()
                       .OrderByDescending(h => h.Id)
                       .Skip(1)
                       .Select(h => h.Priority_Id)
                       .FirstOrDefault() ?? 0;
        }

        private void SendPriorityMailSpecial(Case newCase, CaseLog log, CaseMailSetting cms, List<MailFile> files, string helpdeskMailFromAdress,
            int caseHistoryId, Setting customerSetting, MailSMTPSetting smtpInfo, TimeZoneInfo userTimeZone)
        {
            if (newCase.Priority.MailID_Change.HasValue && !string.IsNullOrEmpty(newCase.Priority.EMailList))
            {
                var mailTemplateId = _mailTemplateService.GetMailTemlpateMailId(newCase.Priority.MailID_Change.Value);
                if (mailTemplateId > 0)
                {
                    var emailList = newCase.Priority.EMailList.Split(';', ',').ToDistintList(true);

                    SendTemplateEmail(mailTemplateId,
                        newCase,
                        log,
                        caseHistoryId,
                        customerSetting,
                        cms,
                        helpdeskMailFromAdress,
                        emailList,
                        userTimeZone,
                        files,
                        5);
                }
            }
        }

        private void SendMergedAndClosedCaseMail(Case mergedCase, Case mergeParent, CaseMailSetting cms, int caseHistoryId, TimeZoneInfo userTimeZone, CaseLog caseLog, IList<string> ccEmailList)
        {
            var mailTemplateId = (int)GlobalEnums.MailTemplates.MergedCase;
            var emailSender = GetWgOrSystemEmailSender(cms);
            var customerSetting = _settingService.GetCustomerSetting(mergeParent.Customer_Id);
            var emailList = new List<string> { mergedCase.PersonsEmail };

            var mailTpl = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(mergedCase.Customer_Id, mergedCase.RegLanguage_Id, mailTemplateId);
            if (mailTpl != null)
            {
                if (!string.IsNullOrEmpty(mailTpl.Body) && !string.IsNullOrEmpty(mailTpl.Subject))
                {
                    SendTemplateEmail(mailTemplateId, mergedCase, caseLog, caseHistoryId, customerSetting, cms, emailSender, emailList, userTimeZone, null, 1, false, ccEmailList, mergeParent);
                }
            }
        }
        private void SendCaseClosedEmail(Case newCase, CaseMailSetting cms, int caseHistoryId, TimeZoneInfo userTimeZone, CaseLog log, List<MailFile> files, Setting customerSetting,
            bool isProblemSend = false, bool dontSendMailToNotfier = false, string helpdeskMailFromAdress = null)
        {
            var mailTemplateId = (int)GlobalEnums.MailTemplates.ClosedCase;
            var emailSender = GetWgOrSystemEmailSender(cms);

            var mailTpl = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
            if (mailTpl != null)
            {
                if (!string.IsNullOrEmpty(mailTpl.Body) && !string.IsNullOrEmpty(mailTpl.Subject))
                {
                    if (!string.IsNullOrWhiteSpace(newCase.Customer.CloseCaseEmailList))
                    {
                        var emailList = newCase.Customer.CloseCaseEmailList.Split(';').ToDistintList(true);

                        SendTemplateEmail(mailTemplateId,
                            mailTpl,
                            newCase,
                            log,
                            caseHistoryId,
                            customerSetting,
                            cms,
                            emailSender,
                            emailList,
                            userTimeZone,
                            files,
                            8);
                    }

                    if (!cms.DontSendMailToNotifier)
                    {
                        var emailList = GetCaseFollowersEmails(newCase, false);

                        SendTemplateEmail(mailTemplateId,
                            mailTpl,
                            newCase,
                            log,
                            caseHistoryId,
                            customerSetting,
                            cms,
                            emailSender,
                            emailList[EmailType.ToMail],
                            userTimeZone,
                            files,
                            9,
                            emailList[EmailType.CcMail],
                            false,
                            emailList[EmailType.CcMail]);
                    }

                    if (isProblemSend)
                    {
                        // send sms
                        if (newCase.SMS == 1 && !dontSendMailToNotfier && !string.IsNullOrWhiteSpace(newCase.PersonsCellphone))
                        {
                            var emailList = new List<string>
                            {
                                GetSmsRecipient(customerSetting, newCase.PersonsCellphone)
                            };

                            SendTemplateEmail((int)GlobalEnums.MailTemplates.SmsClosedCase,
                                mailTpl,
                                newCase,
                                log,
                                caseHistoryId,
                                customerSetting,
                                cms,
                                helpdeskMailFromAdress,
                                emailList,
                                userTimeZone,
                                files,
                                10);
                        }
                    }
                }
            }
        }

        private void SendTemplateEmail(
            int mailTemplateId,
            Case case_,
            CaseLog log,
            int caseHistoryId,
            Setting customerSetting,
            CaseMailSetting cms,
            string senderEmail,
            IList<string> emailList,
            TimeZoneInfo userTimeZone,
            List<MailFile> files = null,
            int stateHelper = 1,
            bool highPriority = false,
            IList<string> ccEmailList = null,
            Case mergeParent = null)
        {
            var mailTpl = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(case_.Customer_Id, case_.RegLanguage_Id, mailTemplateId);

            if (!string.IsNullOrEmpty(mailTpl?.Body) && !string.IsNullOrEmpty(mailTpl.Subject))
            {
                SendTemplateEmail(mailTemplateId, mailTpl, case_, log, caseHistoryId, customerSetting, cms, senderEmail, emailList,
                    userTimeZone, files, stateHelper, null, highPriority, ccEmailList, mergeParent);

            }
        }

        private void SendTemplateEmail(
            int mailTemplateId,
            MailTemplateLanguageEntity mailTpl,
            Case case_,
            CaseLog log,
            int caseHistoryId,
            Setting customerSetting,
            CaseMailSetting cms,
            string senderEmail,
            IList<string> emailList,
            TimeZoneInfo userTimeZone,
            List<MailFile> files = null,
            int stateHelper = 1,
            IList<string> filterFieldsEmails = null,
            bool highPriority = false,
            IList<string> ccEmailList = null,
            Case mergeParent = null)
        {
            senderEmail = (senderEmail ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(senderEmail) || emailList == null || !emailList.Any())
                return;

            var smtpInfo = CreateSmtpSettings(customerSetting);

            if (mailTpl != null && !string.IsNullOrEmpty(mailTpl.Body) && !string.IsNullOrEmpty(mailTpl.Subject))
            {
                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);

                var fieldsToUpdate = new List<FeedbackField>();
                var subject = mailTpl.Subject;

                emailList = emailList.ToDistintList(true).Where(IsValidEmail).ToList();
                var emailLogs = new Dictionary<string, EmailLog>();

                if (mailTpl.MailTemplate.SendMethod == EmailSendMethod.OneEmail)
                {
                    var recepients = string.Join(",", emailList);
                    var ccRecepients = ccEmailList != null && ccEmailList.Any() ? string.Join(",", ccEmailList) : null;
                    var emailLog = new EmailLog(caseHistoryId, mailTemplateId, recepients,
                        _emailService.GetMailMessageId(senderEmail));
                    emailLog.Cc = ccRecepients;
                    emailLogs.Add(recepients, emailLog);
                    
                }
                else
                {
                    if (ccEmailList != null && ccEmailList.Any())
                        emailList = emailList.Union(ccEmailList).Where(IsValidEmail).ToList().ToDistintList(true);
                    foreach (var recepient in emailList)
                    {
                        var emailLog = new EmailLog(caseHistoryId, mailTemplateId, recepient,
                            _emailService.GetMailMessageId(senderEmail));
                        emailLogs.Add(recepient, emailLog);
                    }
                }

                foreach (var eLog in emailLogs)
                {
                    var siteSelfService = ConfigurationManager.AppSettings[AppSettingsKey.SelfServiceAddress] +
                                          eLog.Value.EmailLogGUID;

                    var globalSetting = _globalSettingService.GetGlobalSettings().First();
                    var caseEditPath = globalSetting.UseMobileRouting ?
                        CasePaths.EDIT_CASE_MOBILEROUTE :
                        CasePaths.EDIT_CASE_DESKTOP;

                    var siteHelpdesk = cms.AbsoluterUrl + caseEditPath + case_.Id;

                    var fields = stateHelper == 99
                        ? GetCaseFieldsForEmail(case_, log, cms, string.Empty, stateHelper, userTimeZone, mergeParent)
                        : GetCaseFieldsForEmail(case_, log, cms, eLog.Value.EmailLogGUID.ToString(), stateHelper,
                            userTimeZone, mergeParent);

                    if (mailTemplateId == (int)GlobalEnums.MailTemplates.SmsClosedCase)
                    {
                        var identifiers = _feedbackTemplateService.FindIdentifiers(mailTpl.Body);

                        var templateFields =
                            _feedbackTemplateService.GetCustomerTemplates(identifiers, case_.Customer_Id,
                                case_.RegLanguage_Id, case_.Id, case_.CaseType_Id, cms.AbsoluterUrl);

                        fields.AddRange(templateFields.Select(tf => tf.MapToFields()));

                        //save to list to update status later
                        fieldsToUpdate.AddRange(templateFields);
                    }

                    //change subject for sms notifications
                    if (mailTemplateId == (int)GlobalEnums.MailTemplates.SmsAssignedCaseToUser ||
                        mailTemplateId == (int)GlobalEnums.MailTemplates.SmsClosedCase)
                    {
                        subject = GetSmsSubject(customerSetting);
                    }

                    var body = mailTpl.Body;
                    if (mailTemplateId == (int)GlobalEnums.MailTemplates.ClosedCase && mailTpl.MailTemplate.IncludeLogText_External)
                    {
                        body += _caseMailer.GetExternalLogTextHistory(case_, senderEmail, log);
                    }

                    //exclude admin specific fields (fieldTemplate.ExcludeAdministrators) or those provided with filterFieldsEmails
                    // var applyFeedbackFilter = mailTpl.MailTemplate.SendMethod == EmailSendMethod.SeparateEmails;
                    var applyFeedbackFilter = true;
                    var feedBackFields = GetFeedbackFields(mailTemplateId, case_, cms, fields, eLog.Key,
                       ref body, filterFieldsEmails, applyFeedbackFilter);
                    fieldsToUpdate.AddRange(feedBackFields);

                    var siteSelfServiceMergeParent = "";
                    if (mergeParent != null)
                    {
                        siteSelfServiceMergeParent = fields.FirstOrDefault(x => x.Key == "[#MP98Link]").StringValue; ;
                    }
                    var res =
                        _emailService.SendEmail(eLog.Value,
                            senderEmail,
                            eLog.Value.EmailAddress,
                            eLog.Value.Cc,
                            subject,
                            body,
                            fields,
                            mailSetting,
                            eLog.Value.MessageId,
                            highPriority,
                            files,
                            siteSelfService,
                            siteHelpdesk,
                            EmailType.ToMail,
                            siteSelfServiceMergeParent);
                    SaveEmailLog(eLog.Value, res);
                }

                UpdateFeedbackStatus(fieldsToUpdate);
            }
        }

        public List<FeedbackField> GetFeedbackFields(int mailTemplateId, Case newCase, CaseMailSetting cms, List<Field> fields,
            string recepient, ref string body, IList<string> filterFieldsEmails, bool applyFeedbackFilter)
        {
            var templateFields = new List<FeedbackField>();
            var emailsToCheck = filterFieldsEmails ?? new List<string>();

            var initiatorEmail = newCase.PersonsEmail;

            if (mailTemplateId == (int)GlobalEnums.MailTemplates.ClosedCase)
            {
                var adminEmails = newCase.Customer.Users.Where(x => x.UserGroup_Id != UserGroups.User)
                    .Select(x => x.Email)
                    .Distinct()
                    .ToList();

                var identifiers = _feedbackTemplateService.FindIdentifiers(body).ToList();

                //dont send feedback to admins
                var identifiersToDel = new List<string>();
                templateFields = _feedbackTemplateService.GetCustomerTemplates(identifiers, newCase.Customer_Id, newCase.RegLanguage_Id,
                    newCase.Id, newCase.CaseType_Id, cms.AbsoluterUrl);

                foreach (var templateField in templateFields)
                {
                    if (applyFeedbackFilter &&
                        (emailsToCheck.Contains(recepient, StringComparer.OrdinalIgnoreCase) ||
                         templateField.ExcludeAdministrators && adminEmails.Any(x => x.Equals(recepient))))
                    {
                        identifiersToDel.Add(templateField.Key);
                    }
                    else
                    {
                        var tf = templateField.MapToFields();
                        fields.Add(tf);
                    }
                }

                foreach (var identifier in identifiersToDel)
                {
                    if (!string.IsNullOrEmpty(identifier))
                    {
                        body = body.Replace(identifier, string.Empty);
                    }
                }
            }

            return templateFields;
        }

        private IDictionary<EmailType, IList<string>> GetCaseFollowersEmails(Case case_, bool allInTo = true)
        {
            var emails = new Dictionary<EmailType, IList<string>>();

            var emailList = case_.PersonsEmail.Split(';', ',').ToList();
            var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(case_.Id).Select(x => x.Follower).ToList();

            if (allInTo)
                emailList.AddRange(extraFollowers);
            else
                emails.Add(EmailType.CcMail, extraFollowers.ToDistintList(true));

            emails.Add(EmailType.ToMail, emailList.ToDistintList(true));

            return emails;
        }

        public List<string> GetCaseWorkingGropsEmails(Case newCase)
        {
            var wgEmails = string.Empty;
            if (!string.IsNullOrWhiteSpace(newCase.Workinggroup.EMail))
            {
                wgEmails = newCase.Workinggroup.EMail;
            }
            else
            {
                if (newCase.WorkingGroup_Id.HasValue)
                {
                    var usersWorkingGroups = _userService.GetUserWorkingGroupsByWorkgroup(newCase.WorkingGroup_Id.Value);
                    foreach (var ur in usersWorkingGroups)
                    {
                        if (ur.User != null)
                        {
                            if (ur.User.IsActive == 1 && ur.User.AllocateCaseMail == 1 &&
                                IsValidEmail(ur.User.Email) &&
                                ur.UserRole == WorkingGroupUserPermission.ADMINSTRATOR)
                            {
                                if (newCase.Department_Id != null && ur.User.Departments != null && ur.User.Departments.Count > 0)
                                {
                                    if (ur.User.Departments.Any(x => x.Id == newCase.Department_Id.Value))
                                    {
                                        wgEmails = wgEmails + ur.User.Email + ";";
                                    }
                                }
                                else
                                {
                                    wgEmails = wgEmails + ur.User.Email + ";";
                                }
                            }
                        }
                    }
                }
            }

            var emailList = wgEmails.Split(';', ',').ToDistintList(true);
            return emailList.ToList();
        }

        public void UpdateFeedbackStatus(List<FeedbackField> templateFields)
        {

            if (templateFields != null && templateFields.Any())
            {
                // get fields distinct list by fieldId
                var distinctList =
                    templateFields.Where(f => !string.IsNullOrEmpty(f.StringValue)).GroupBy(x => x.FeedbackId).Select(g => g.FirstOrDefault()).ToList();

                foreach (var field in distinctList)
                {
                    _feedbackTemplateService.UpdateFeedbackStatus(field);
                }
            }
        }

        private void SaveEmailLog(EmailLog emailLog, EmailResponse response)
        {
            emailLog.SetResponse(response.SendTime, response.ResponseMessage);
            var now = DateTime.Now;
            emailLog.CreatedDate = now;
            emailLog.ChangedDate = now;
            _emailLogRepository.Add(emailLog);
            _emailLogRepository.Commit();
        }

        private MailSMTPSetting CreateSmtpSettings(Setting customerSetting)
        {
            var smtpInfo =
                new MailSMTPSetting(customerSetting.SMTPServer,
                    customerSetting.SMTPPort,
                    customerSetting.SMTPUserName,
                    customerSetting.SMTPPassWord,
                    customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            return smtpInfo;
        }

        private List<MailFile> PrepareAttachedFiles(IList<CaseLogFileDto> logFiles, string basePath)
        {
            List<MailFile> files = null;
            if (logFiles != null && logFiles.Any())
            {
                var caseFiles =
                    logFiles.Where(x => x.IsCaseFile)
                        .Select(x => new MailFile()
                        {
                            FileName = x.FileName,
                            FilePath = _filesStorage.ComposeFilePath(ModuleName.Cases, x.ReferenceId, basePath, x.FileName),
                            IsInternal = x.LogType == LogFileType.Internal
                        }).ToList();

                files =
                    logFiles.Where(x => !x.IsCaseFile)
                        .Select(f => new MailFile()
                        {
                            FileName = f.FileName,
                            FilePath = _filesStorage.ComposeFilePath((f.ParentLogType ?? f.LogType).GetFolderPrefix(),
                                f.ReferenceId, basePath, f.FileName),
                            IsInternal = f.LogType == LogFileType.Internal
                        }).ToList();

                files.AddRange(caseFiles);
            }

            return files;
        }

        private string GetWgOrSystemEmailSender(CaseMailSetting cms)
        {
            //Default Owner WG email
            var senderEmail = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;

            //WG email
            if (string.IsNullOrWhiteSpace(senderEmail))
                senderEmail = cms.CustomeMailFromAddress.WGEmail;

            //System email
            if (string.IsNullOrWhiteSpace(senderEmail))
                senderEmail = cms.CustomeMailFromAddress.SystemEmail;

            return senderEmail?.Trim();
        }

        private bool IsValidEmail(string email)
        {
            return _emailService.IsValidEmail(email);
        }

        private bool IsSelfService()
        {
            bool isSelfService = false;
            var curApp = ConfigurationManager.AppSettings["CurrentApplication"];
            if (!string.IsNullOrEmpty(curApp))
            {
                return curApp.Equals("selfservice", StringComparison.OrdinalIgnoreCase) || curApp.Equals("linemanager", StringComparison.OrdinalIgnoreCase);
            }
            return isSelfService;
        }

        #endregion
    }
}
