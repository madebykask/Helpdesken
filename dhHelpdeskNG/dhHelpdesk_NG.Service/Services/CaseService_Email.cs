using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

            if (!isClosingCase && isCreatingCase)
            {
                #region Send email about new case to notifier or tblCustomer.NewCaseEmailList & (productarea template, priority template)
                
                var customEmailSender1 = GetWgOrSystemEmailSender(cms);

                // get mail template 
                var mailTemplateId = (int)GlobalEnums.MailTemplates.NewCase;

                var mailTpl =
                    _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);

                if (mailTpl != null)
                {
                    if (!string.IsNullOrEmpty(mailTpl.Body) && !string.IsNullOrEmpty(mailTpl.Subject))
                    {
                        if (!cms.DontSendMailToNotifier && !string.IsNullOrEmpty(newCase.PersonsEmail))
                        {
                            var emailList = GetCaseFollowersEmails(newCase);
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
                                1);

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

                #region Send mail template from productArea if productarea has a mailtemplate

                if (newCase.ProductArea_Id.HasValue && newCase.ProductArea != null)
                {
                    // get mail template from productArea
                    mailTemplateId = 0;

                    if (newCase.ProductArea.MailTemplate != null)
                        mailTemplateId = newCase.ProductArea.MailTemplate.MailID;

                    if (mailTemplateId > 0)
                    {
                        if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier && !string.IsNullOrEmpty(newCase.PersonsEmail))
                        {
							var productAreaMailTemplate = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);

							if (productAreaMailTemplate != null)
							{
								var emailList = GetCaseFollowersEmails(newCase);
								SendTemplateEmail(mailTemplateId,
									productAreaMailTemplate,
									newCase,
									log,
									caseHistoryId,
									customerSetting,
									cms,
									customEmailSender1,
									emailList,
									userTimeZone,
									files,
									1);

								informNotifierHasBeenSent = true;
							}
                        }
                    }
                }

                #endregion

                #region If priority has value and an emailaddress

                if (newCase.Priority != null && !string.IsNullOrWhiteSpace(newCase.Priority.EMailList))
                {
                    var emailList = newCase.Priority.EMailList.Split(';', ',').ToDistintList(true);
					var priorityMailTemplate = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, (int)GlobalEnums.MailTemplates.AssignedCaseToPriority);

					if (priorityMailTemplate != null)
					{
						SendTemplateEmail((int)GlobalEnums.MailTemplates.AssignedCaseToPriority,
						priorityMailTemplate,
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
                    var mailTemplateId = 0;
                    var emailSender = GetWgOrSystemEmailSender(cms);

                    // get mail template from productArea                   
                    if (newCase.ProductArea.MailTemplate != null)
                        mailTemplateId = newCase.ProductArea.MailTemplate.MailID;

                    if (mailTemplateId > 0)
                    {
                        if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier && !string.IsNullOrEmpty(newCase.PersonsEmail))
                        {
                            var emailList = GetCaseFollowersEmails(newCase);

                            SendTemplateEmail(mailTemplateId,
                                newCase,
                                log,
                                caseHistoryId,
                                customerSetting,
                                cms,
                                emailSender,
                                emailList,
                                userTimeZone,
                                files,
                                1);

                            informNotifierHasBeenSent = true;
                        }
                    }
                }
            }

            #endregion

            #region Send email to tblCase.Performer_User_Id

            if ((!isClosingCase && isCreatingCase && newCase.Performer_User_Id.HasValue || 
                 !isCreatingCase && newCase.Performer_User_Id.HasValue && newCase.Performer_User_Id != oldCase.Performer_User_Id))
            {
                var admin = _userRepository.GetUserInfo(newCase.Performer_User_Id.Value);
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
                        }
                    }
                    else
                    {
                        SendTemplateEmail((int)GlobalEnums.MailTemplates.AssignedCaseToUser, newCase, log, caseHistoryId, 
                            customerSetting, cms, cms.HelpdeskMailFromAdress, emailList, userTimeZone, null, 3);
                    }
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
                    var emailList = newCase.Priority.EMailList.Split(';', ',').ToDistintList(true);
                    SendTemplateEmail((int)GlobalEnums.MailTemplates.AssignedCaseToPriority,
                        newCase,
                        log,
                        caseHistoryId,
                        customerSetting,
                        cms,
                        helpdeskMailFromAdress,
                        emailList,
                        userTimeZone,
                        files,
                        1);
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
                var mailTemplateId = newCase.ProductArea.MailTemplate.MailID;
                if (mailTemplateId > 0)
                {
                    var emailList = GetCaseFollowersEmails(newCase);
                    SendTemplateEmail((int)GlobalEnums.MailTemplates.AssignedCaseToWorkinggroup,
                        newCase,
                        log,
                        caseHistoryId,
                        customerSetting,
                        cms,
                        helpdeskMailFromAdress,
                        emailList,
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

            // State Secondary Email TODO ikea ims only??
            #region commented 
            // Commented out for now, will be added later with a better solution decided 20150626
            //if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier && !isClosedMailSentToNotifier && oldCase != null && oldCase.Id > 0)  
            //    if (newCase.StateSecondary_Id != oldCase.StateSecondary_Id && newCase.StateSecondary_Id > 0)
            //        if (IsValidEmail(newCase.PersonsEmail))
            //        {
            //            int mailTemplateId = (int)GlobalEnums.MailTemplates.ClosedCase;

            //            string customEmailSender3 = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;
            //            if (string.IsNullOrWhiteSpace(customEmailSender3))
            //                customEmailSender3 = cms.CustomeMailFromAddress.WGEmail;
            //            if (string.IsNullOrWhiteSpace(customEmailSender3))
            //                customEmailSender3 = cms.CustomeMailFromAddress.SystemEmail;

            //            var m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
            //            if (m != null)
            //            {
            //                if (!string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
            //                {
            //                    var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.PersonsEmail, _emailService.GetMailMessageId(customEmailSender3));
            //                    _emailLogRepository.Add(el);
            //                    _emailLogRepository.Commit();
            //                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 11);
            //                    _emailService.SendEmail(customEmailSender3, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
            //                }
            //            }
            //        }
            #endregion

            if (!informNotifierHasBeenSent)
            {
                //todo: check fields values - could be that they are get overrided in one of the send methods
                //todo: move to ProductArea or new case mail ?
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

            //todo: check fields values - could be that they are get overrided in one of the send methods
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

        private int GetLastButOneCaseHistoryPriorityId(int caseId)
        {
            return _caseHistoryRepository.GetCaseHistoryByCaseId(caseId)
                       .AsQueryable()
                       .OrderByDescending(h => h.Id)
                       .Skip(1)
                       .Select(h => h.Priority_Id)
                       .FirstOrDefault() ?? 0;
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
                ? (int) GlobalEnums.MailTemplates.CaseIsUpdated
                : (int) GlobalEnums.MailTemplates.CaseIsActivated;

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

        private void SendPriorityMailSpecial(Case newCase, CaseLog log, CaseMailSetting cms, List<MailFile> files, string helpdeskMailFromAdress, int caseHistoryId, Setting customerSetting, MailSMTPSetting smtpInfo, TimeZoneInfo userTimeZone)
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
                        var followerEmails = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id).Select(x => x.Follower).ToList();//newCase.PersonsEmail.Split(';', ',').ToList();
						var emailList = GetCaseFollowersEmails(newCase).ToDistintList(true);
                        
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
                            9,
                            followerEmails);
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
            bool highPriority = false)
        {
            var mailTpl = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(case_.Customer_Id, case_.RegLanguage_Id, mailTemplateId);

            if (!string.IsNullOrEmpty(mailTpl?.Body) && !string.IsNullOrEmpty(mailTpl.Subject))
            {
                SendTemplateEmail(mailTemplateId, mailTpl, case_, log, caseHistoryId, customerSetting, cms, senderEmail, emailList, userTimeZone, files, stateHelper, null, highPriority);
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
            List<string> filterFieldsEmails = null,
            bool highPriority = false)
        {
            senderEmail = (senderEmail ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(senderEmail))
                return;

            var smtpInfo = CreateSmtpSettings(customerSetting);

            if (mailTpl != null && !string.IsNullOrEmpty(mailTpl.Body) && !string.IsNullOrEmpty(mailTpl.Subject))
            {
                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);

                var fieldsToUpdate = new List<FeedbackField>();
                var subject = mailTpl.Subject;

                foreach (var recepient in emailList.ToDistintList(true))
                {
                    if (!IsValidEmail(recepient))
                        continue;

                    var emailLog = new EmailLog(caseHistoryId, mailTemplateId, recepient, _emailService.GetMailMessageId(senderEmail));
                    var siteSelfService = ConfigurationManager.AppSettings[AppSettingsKey.SelfServiceAddress] + emailLog.EmailLogGUID;
                    var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + case_.Id;

                    var fields = stateHelper == 99 ?
                        GetCaseFieldsForEmail(case_, log, cms, string.Empty, stateHelper, userTimeZone) :
                        GetCaseFieldsForEmail(case_, log, cms, emailLog.EmailLogGUID.ToString(), stateHelper, userTimeZone);

                    #region legacy code 

                    //string urlSelfService;
                    //if (m.Body.Contains("[/#98]"))
                    //{
                    //    string str1 = "[#98]";
                    //    string str2 = "[/#98]";
                    //    string LinkText;

                    //    int Pos1 = m.Body.IndexOf(str1) + str1.Length;
                    //    int Pos2 = m.Body.IndexOf(str2);
                    //    LinkText = m.Body.Substring(Pos1, Pos2 - Pos1);

                    //    urlSelfService = "<a href='" + siteSelfService + "'>" + LinkText + "</a>";

                    //}
                    //else
                    //{
                    //    urlSelfService = "<a href='" + siteSelfService + "'>" + siteSelfService + "</a>";
                    //}

                    //foreach (var field in fields)
                    //    if (field.Key == "[#98]")
                    //        field.StringValue = urlSelfService;

                    //var urlHelpdesk = "";
                    //var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + case_.Id;
                    //if (m.Body.Contains("[/#99]"))
                    //{
                    //    string str1 = "[#99]";
                    //    string str2 = "[/#99]";
                    //    string LinkText;

                    //    int Pos1 = m.Body.IndexOf(str1) + str1.Length;
                    //    int Pos2 = m.Body.IndexOf(str2);
                    //    LinkText = m.Body.Substring(Pos1, Pos2 - Pos1);

                    //    urlHelpdesk = "<a href='" + siteHelpdesk + "'>" + LinkText + "</a>";

                    //}
                    //else
                    //{
                    //    urlHelpdesk = "<a href='" + siteHelpdesk + "'>" + siteHelpdesk + "</a>";
                    //}

                    //foreach (var field in fields)
                    //    if (field.Key == "[#99]")
                    //        field.StringValue = urlHelpdesk;
                    #endregion

                    if (mailTemplateId == (int) GlobalEnums.MailTemplates.SmsClosedCase)
                    {
                        var identifiers = _feedbackTemplateService.FindIdentifiers(mailTpl.Body);

                        var templateFields =
                            _feedbackTemplateService.GetCustomerTemplates(identifiers, case_.Customer_Id, case_.RegLanguage_Id, case_.Id, cms.AbsoluterUrl);
                    
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


					//exclude admin specific fields (fieldTemplate.ExcludeAdministrators) or those provided with filterFieldsEmails
					var body = mailTpl.Body;
                    var feedBackFields = ApplyFieldsExcludeFilter(mailTemplateId, case_, cms, fields, recepient, ref body, filterFieldsEmails);
                    fieldsToUpdate.AddRange(feedBackFields);

                    var res =
                        _emailService.SendEmail(emailLog,
                            senderEmail,
                            emailLog.EmailAddress,
                            subject,
                            body,
                            fields,
                            mailSetting,
                            emailLog.MessageId,
                            highPriority, 
                            files,
                            siteSelfService,
                            siteHelpdesk);

                    SaveEmailLog(emailLog, res);
                }

                UpdateFeedbackStatus(fieldsToUpdate);
            }
        }

        private List<FeedbackField> ApplyFieldsExcludeFilter(int mailTemplateId, Case newCase, CaseMailSetting cms, List<Field> fields, string recepient, ref string body , List<string> filterFieldsEmails)
        {
            var templateFields = new List<FeedbackField>();
            var emailsToCheck = filterFieldsEmails ?? new List<string>();

            if (mailTemplateId == (int)GlobalEnums.MailTemplates.ClosedCase)
            {
                var adminEmails = newCase.Customer.UsersAvailable.Where(x => x.UserGroup_Id != UserGroups.User).Select(x => x.Email).ToList();
                var identifiers = _feedbackTemplateService.FindIdentifiers(body).ToList();

                //dont send feedback to admins
                var identifiersToDel = new List<string>();
                templateFields = _feedbackTemplateService.GetCustomerTemplates(identifiers, newCase.Customer_Id, newCase.RegLanguage_Id, newCase.Id, cms.AbsoluterUrl);

                foreach (var templateField in templateFields)
                {
                    if (emailsToCheck.Contains(recepient, StringComparer.OrdinalIgnoreCase) || templateField.ExcludeAdministrators && adminEmails.Any(x => x.Equals(recepient)))
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

        private IList<string> GetCaseFollowersEmails(Case case_)
        {
            var emailList = case_.PersonsEmail.Split(';', ',').ToList();
            var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(case_.Id).Select(x => x.Follower).ToList();
            emailList.AddRange(extraFollowers);

            var distinctedEmails = emailList.ToDistintList(true);
            return distinctedEmails;
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

        private void UpdateFeedbackStatus(List<FeedbackField> templateFields)
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
