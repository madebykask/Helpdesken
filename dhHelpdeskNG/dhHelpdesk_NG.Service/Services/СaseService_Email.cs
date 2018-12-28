using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Email;
using DH.Helpdesk.BusinessData.Models.Feedback;
using DH.Helpdesk.BusinessData.Models.MailTemplates;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
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
            List<CaseFileDto> logFiles = null,
            User currentLoggedInUser = null)
        {
            //get sender email adress
            var helpdeskMailFromAdress = string.Empty;
            var containsProductAreaMailOrNewCaseMail = false;

            if (!string.IsNullOrEmpty((cms.HelpdeskMailFromAdress)))
            {
                helpdeskMailFromAdress = cms.HelpdeskMailFromAdress.Trim();
            }

            if (!this._emailService.IsValidEmail(helpdeskMailFromAdress))
            {
                return;
            }

            // get new case information
            var newCase = _caseRepository.GetDetachedCaseById(caseId);

            var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id);
            var dontSendMailToNotfier = false;
            var isCreatingCase = oldCase == null || oldCase.Id == 0;
            var isClosingCase = newCase.FinishingDate != null;

            // get list of fields to replace [#1] tags in the subjcet and body texts
            var fields = GetCaseFieldsForEmail(newCase, log, cms, string.Empty, 0, userTimeZone);

            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort,
                customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            // if logfiles should be attached to the mail 
            List<string> files = null;
            if (logFiles != null && log != null)
                if (logFiles.Count > 0)
                {
                    var caseFiles = logFiles.Where(x => x.IsCaseFile).Select(x =>
                        _filesStorage.ComposeFilePath(ModuleName.Cases, x.ReferenceId, basePath, x.FileName)).ToList();
                    files = logFiles.Where(x => !x.IsCaseFile).Select(f =>
                        _filesStorage.ComposeFilePath(ModuleName.Log, f.ReferenceId, basePath, f.FileName)).ToList();
                    files.AddRange(caseFiles);
                }

            // sub state should not generate email to notifier
            if (newCase.StateSecondary != null)
                dontSendMailToNotfier = newCase.StateSecondary.NoMailToNotifier == 1;

            if (!isClosingCase && isCreatingCase)
            {
                #region Send email about new case to notifier or tblCustomer.NewCaseEmailList & (productarea template, priority template)

                // get mail template 
                var mailTemplateId = (int)GlobalEnums.MailTemplates.NewCase;
                var customEmailSender1 = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;

                if (string.IsNullOrWhiteSpace(customEmailSender1))
                    customEmailSender1 = cms.CustomeMailFromAddress.WGEmail;

                if (string.IsNullOrWhiteSpace(customEmailSender1))
                    customEmailSender1 = cms.CustomeMailFromAddress.SystemEmail;
                if (!string.IsNullOrEmpty(customEmailSender1))
                {
                    customEmailSender1 = customEmailSender1.Trim();
                }

                var mailTpl =
                    _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id,
                        newCase.RegLanguage_Id, mailTemplateId);
                if (mailTpl != null)
                {
                    if (!string.IsNullOrEmpty(mailTpl.Body) && !string.IsNullOrEmpty(mailTpl.Subject))
                    {
                        if (!cms.DontSendMailToNotifier && !string.IsNullOrEmpty(newCase.PersonsEmail))
                        {
                            var to = newCase.PersonsEmail.Split(';', ',').ToList();
                            var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id)
                                .Select(x => x.Follower).ToList();
                            to.AddRange(extraFollowers);
                            foreach (var t in to)
                            {
                                var curMail = t.Trim();
                                if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                {
                                    var emailLog = new EmailLog(caseHistoryId, mailTemplateId, curMail,
                                        _emailService.GetMailMessageId(customEmailSender1));
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, emailLog.EmailLogGUID.ToString(),
                                        1, userTimeZone);
                                    string siteSelfService =
                                        ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() +
                                        emailLog.EmailLogGUID.ToString();
                                    var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                    var mailSetting = new EmailSettings(mailResponse, smtpInfo,
                                        customerSetting.BatchEmail);
                                    var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();


                                    var e_res = _emailService.SendEmail(emailLog,
                                        customEmailSender1,
                                        emailLog.EmailAddress,
                                        mailTpl.Subject,
                                        mailTpl.Body,
                                        fields,
                                        mailSetting,
                                        emailLog.MessageId,
                                        false,
                                        files,
                                        siteSelfService,
                                        siteHelpdesk);

                                    emailLog.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                    var now = DateTime.Now;
                                    emailLog.CreatedDate = now;
                                    emailLog.ChangedDate = now;
                                    _emailLogRepository.Add(emailLog);
                                    _emailLogRepository.Commit();
                                    containsProductAreaMailOrNewCaseMail = true;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(cms.SendMailAboutNewCaseTo))
                        {
                            var reciepients = cms.SendMailAboutNewCaseTo.Split(';');
                            foreach (var reciepient in reciepients)
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, reciepient,
                                    _emailService.GetMailMessageId(customEmailSender1));
                                fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 2,
                                    userTimeZone);
                                string siteSelfService =
                                    ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() +
                                    el.EmailLogGUID.ToString();

                                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                var e_res = _emailService.SendEmail(el, customEmailSender1, el.EmailAddress,
                                    mailTpl.Subject, mailTpl.Body, fields, mailSetting, el.MessageId, false, files,
                                    siteSelfService, siteHelpdesk);
                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                el.ChangedDate = now;
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                            }
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
                        var mm =
                            _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id,
                                newCase.RegLanguage_Id, mailTemplateId);
                        if (mm != null)
                        {
                            if (!string.IsNullOrEmpty(mm.Body) && !string.IsNullOrEmpty(mm.Subject))
                            {
                                if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier &&
                                    !string.IsNullOrEmpty(newCase.PersonsEmail))
                                {
                                    var to = newCase.PersonsEmail.Split(';', ',').ToList();
                                    var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id)
                                        .Select(x => x.Follower).ToList();
                                    to.AddRange(extraFollowers);
                                    foreach (var t in to)
                                    {
                                        var curMail = t.Trim();
                                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                        {
                                            var el = new EmailLog(caseHistoryId, mailTemplateId, curMail,
                                                _emailService.GetMailMessageId(customEmailSender1));
                                            fields = GetCaseFieldsForEmail(newCase, log, cms,
                                                el.EmailLogGUID.ToString(), 1, userTimeZone);

                                            string siteSelfService =
                                                ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() +
                                                el.EmailLogGUID.ToString();
                                            var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                            var mailSetting = new EmailSettings(mailResponse, smtpInfo,
                                                customerSetting.BatchEmail);
                                            var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                                            var e_res = _emailService.SendEmail(el, customEmailSender1, el.EmailAddress,
                                                mm.Subject, mm.Body, fields, mailSetting, el.MessageId, false, files,
                                                siteSelfService, siteHelpdesk);
                                            el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                            var now = DateTime.Now;
                                            el.CreatedDate = now;
                                            el.ChangedDate = now;
                                            _emailLogRepository.Add(el);
                                            _emailLogRepository.Commit();
                                            containsProductAreaMailOrNewCaseMail = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region If priority has value and an emailaddress

                if (newCase.Priority != null)
                {
                    if (!string.IsNullOrWhiteSpace(newCase.Priority.EMailList))
                    {
                        SendPriorityMail(newCase, log, cms, files, helpdeskMailFromAdress, caseHistoryId, caseId,
                            customerSetting, smtpInfo, userTimeZone);
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
                    if (!string.IsNullOrWhiteSpace(newCase.Priority.EMailList) && log != null &&
                        !string.IsNullOrEmpty(log.TextExternal))
                    {
                        SendPriorityMailSpecial(newCase, log, cms, files, helpdeskMailFromAdress, caseHistoryId, caseId,
                            customerSetting, smtpInfo, userTimeZone);
                    }
                    else
                    {
                        if (log != null && (!string.IsNullOrEmpty(log.TextExternal) ||
                                            !string.IsNullOrEmpty(log.TextInternal)))
                        {
                            var caseHis = _caseHistoryRepository.GetCloneOfPenultimate(caseId);
                            if (caseHis?.Priority_Id != null)
                            {
                                var prevPriority = _priorityService.GetPriority(caseHis.Priority_Id.Value);
                                if (!string.IsNullOrWhiteSpace(prevPriority.EMailList))
                                {
                                    var copyNewCase = _caseRepository.GetDetachedCaseById(caseId);
                                    copyNewCase.Priority = prevPriority;
                                    SendPriorityMailSpecial(copyNewCase, log, cms, files, helpdeskMailFromAdress,
                                        caseHistoryId, caseId, customerSetting, smtpInfo, userTimeZone);
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
                if (newCase.ProductArea_Id.HasValue && newCase.ProductArea != null &&
                    oldCase.ProductAreaSetDate == null)
                {
                    var mailTemplateId = 0;
                    var customEmailSender1 = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;

                    if (string.IsNullOrWhiteSpace(customEmailSender1))
                        customEmailSender1 = cms.CustomeMailFromAddress.WGEmail;

                    if (string.IsNullOrWhiteSpace(customEmailSender1))
                        customEmailSender1 = cms.CustomeMailFromAddress.SystemEmail;

                    if (!string.IsNullOrEmpty(customEmailSender1))
                    {
                        customEmailSender1 = customEmailSender1.Trim();
                    }

                    // get mail template from productArea                   
                    if (newCase.ProductArea.MailTemplate != null)
                        mailTemplateId = newCase.ProductArea.MailTemplate.MailID;

                    if (mailTemplateId > 0)
                    {
                        var m =
                            _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id,
                                newCase.RegLanguage_Id, mailTemplateId);
                        if (m != null)
                        {
                            if (!string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
                            {
                                if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier &&
                                    !string.IsNullOrEmpty(newCase.PersonsEmail))
                                {
                                    var to = newCase.PersonsEmail.Split(';', ',').ToList();
                                    var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id)
                                        .Select(x => x.Follower).ToList();
                                    to.AddRange(extraFollowers);
                                    foreach (var t in to)
                                    {
                                        var curMail = t.Trim();
                                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                        {
                                            var el = new EmailLog(caseHistoryId, mailTemplateId, curMail,
                                                _emailService.GetMailMessageId(customEmailSender1));
                                            fields = GetCaseFieldsForEmail(newCase, log, cms,
                                                el.EmailLogGUID.ToString(), 1, userTimeZone);

                                            string siteSelfService =
                                                ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() +
                                                el.EmailLogGUID.ToString();
                                            var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                            var mailSetting = new EmailSettings(mailResponse, smtpInfo,
                                                customerSetting.BatchEmail);
                                            var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                                            var e_res = _emailService.SendEmail(el, customEmailSender1, el.EmailAddress,
                                                m.Subject, m.Body, fields, mailSetting, el.MessageId, false, files,
                                                siteSelfService, siteHelpdesk);
                                            el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                            var now = DateTime.Now;
                                            el.CreatedDate = now;
                                            el.ChangedDate = now;
                                            _emailLogRepository.Add(el);
                                            _emailLogRepository.Commit();
                                            containsProductAreaMailOrNewCaseMail = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Send email to tblCase.Performer_User_Id

            if (((!isClosingCase && isCreatingCase && newCase.Performer_User_Id.HasValue)
                 || (!isCreatingCase && newCase.Performer_User_Id != oldCase.Performer_User_Id))
                && newCase.Administrator != null)
            {
                if (newCase.Administrator.AllocateCaseMail == 1 &&
                    this._emailService.IsValidEmail(newCase.Administrator.Email))
                {
                    if (currentLoggedInUser != null)
                    {
                        if (currentLoggedInUser.SettingForNoMail == 1 || (currentLoggedInUser.Id ==
                                                                          newCase.Performer_User_Id &&
                                                                          currentLoggedInUser.SettingForNoMail == 1)
                                                                      || (currentLoggedInUser.Id !=
                                                                          newCase.Performer_User_Id &&
                                                                          currentLoggedInUser.SettingForNoMail == 0))
                        {
                            this.SendTemplateEmail(GlobalEnums.MailTemplates.AssignedCaseToUser, newCase, log,
                                caseHistoryId, cms, newCase.Administrator.Email, userTimeZone);
                        }
                    }
                    else
                    {
                        this.SendTemplateEmail(GlobalEnums.MailTemplates.AssignedCaseToUser, newCase, log,
                            caseHistoryId, cms, newCase.Administrator.Email, userTimeZone);
                    }
                }

                // send sms to tblCase.Performer_User_Id 
                if (newCase.Administrator.AllocateCaseSMS == 1 &&
                    !string.IsNullOrWhiteSpace(newCase.Administrator.CellPhone) && newCase.Customer != null)
                {
                    var mailTemplateId = (int)GlobalEnums.MailTemplates.SmsAssignedCaseToUser;
                    var m =
                        _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id,
                            newCase.RegLanguage_Id, mailTemplateId);
                    if (m != null)
                    {
                        if (!string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
                        {
                            var smsTo = GetSmsRecipient(customerSetting, newCase.Administrator.CellPhone);
                            var el = new EmailLog(caseHistoryId, mailTemplateId, smsTo,
                                _emailService.GetMailMessageId(helpdeskMailFromAdress));
                            fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 4,
                                userTimeZone);

                            var siteSelfService =
                                ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() +
                                el.EmailLogGUID.ToString();
                            var mailResponse = EmailResponse.GetEmptyEmailResponse();
                            var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                            var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                            var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress,
                                GetSmsSubject(customerSetting), m.Body, fields, mailSetting, el.MessageId, false, files,
                                siteSelfService, siteHelpdesk);
                            el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                            var now = DateTime.Now;
                            el.CreatedDate = now;
                            el.ChangedDate = now;
                            _emailLogRepository.Add(el);
                            _emailLogRepository.Commit();
                        }
                    }
                }
            }

            #endregion

            #region Send email priority has changed

            if (newCase.FinishingDate == null && oldCase != null && oldCase.Id > 0)
            {
                if (newCase.Priority_Id != oldCase.Priority_Id)
                {
                    if (newCase.Priority_Id != null)
                    {
                        if (!string.IsNullOrWhiteSpace(newCase.Priority.EMailList))
                        {
                            int mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToPriority;
                            var m =
                                _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id,
                                    newCase.RegLanguage_Id, mailTemplateId);
                            if (m != null)
                            {
                                if (!string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
                                {
                                    var to = newCase.Priority.EMailList.Split(';', ',').ToList();
                                    foreach (var t in to)
                                    {
                                        var curMail = t.Trim();
                                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                        {
                                            if (!string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
                                            {
                                                var el = new EmailLog(caseHistoryId, mailTemplateId, curMail,
                                                    _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                                fields = GetCaseFieldsForEmail(newCase, log, cms,
                                                    el.EmailLogGUID.ToString(), 5, userTimeZone);

                                                string siteSelfService =
                                                    ConfigurationManager.AppSettings["dh_selfserviceaddress"]
                                                        .ToString() + el.EmailLogGUID.ToString();
                                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                                var mailSetting = new EmailSettings(mailResponse, smtpInfo,
                                                    customerSetting.BatchEmail);
                                                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                                                var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress,
                                                    el.EmailAddress, m.Subject, m.Body, fields, mailSetting,
                                                    el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                                var now = DateTime.Now;
                                                el.CreatedDate = now;
                                                el.ChangedDate = now;
                                                _emailLogRepository.Add(el);
                                                _emailLogRepository.Commit();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Send email working group has changed

            if (!isClosingCase
                && newCase.Workinggroup != null
                && (isCreatingCase || (!isCreatingCase && newCase.WorkingGroup_Id != oldCase.WorkingGroup_Id)))
            {
                var mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToWorkinggroup;
                var m =
                    _mailTemplateService.GetMailTemplateForCustomerAndLanguage(
                        newCase.Customer_Id,
                        newCase.RegLanguage_Id,
                        mailTemplateId);
                if (m != null)
                {
                    if (!string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
                    {
                        string wgEmails = string.Empty;
                        if (!string.IsNullOrWhiteSpace(newCase.Workinggroup.EMail))
                            wgEmails = newCase.Workinggroup.EMail;
                        else
                        {
                            if (newCase.Workinggroup.UserWorkingGroups != null)
                                foreach (var ur in newCase.Workinggroup.UserWorkingGroups)
                                {
                                    if (ur.User != null)
                                        if (ur.User.IsActive == 1 && ur.User.AllocateCaseMail == 1 &&
                                            _emailService.IsValidEmail(ur.User.Email) &&
                                            ur.UserRole == WorkingGroupUserPermission.ADMINSTRATOR)
                                        {
                                            if (newCase.Department_Id != null && ur.User.Departments != null &&
                                                ur.User.Departments.Count > 0)
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

                        if (newCase.Workinggroup.AllocateCaseMail == 1 && !string.IsNullOrWhiteSpace(wgEmails))
                        {
                            var to = wgEmails.Split(';', ',').ToList();

                            foreach (var t in to)
                            {
                                var curMail = t.Trim();
                                if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, curMail,
                                        _emailService.GetMailMessageId(helpdeskMailFromAdress));

                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 6,
                                        userTimeZone);

                                    var siteSelfService =
                                        ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() +
                                        el.EmailLogGUID.ToString();

                                    var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                    var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                    var mailSetting = new EmailSettings(mailResponse, smtpInfo,
                                        customerSetting.BatchEmail);
                                    var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress,
                                        m.Subject, m.Body, fields, mailSetting, el.MessageId, false, files,
                                        siteSelfService, siteHelpdesk);

                                    el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                    var now = DateTime.Now;
                                    el.CreatedDate = now;
                                    el.ChangedDate = now;
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Send email when product area is set

            if (!isClosingCase && !isCreatingCase && !containsProductAreaMailOrNewCaseMail
                && oldCase.ProductAreaSetDate == null && newCase.RegistrationSource == 3
                && !cms.DontSendMailToNotifier &&
                newCase.ProductArea?.MailTemplate != null &&
                newCase.ProductArea.MailTemplate.MailID > 0 &&
                !string.IsNullOrEmpty(newCase.PersonsEmail))
            {
                var to = newCase.PersonsEmail.Split(';', ',').ToList();
                var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id)
                    .Select(x => x.Follower).ToList();
                to.AddRange(extraFollowers);
                foreach (var t in to)
                {
                    var curMail = t.Trim();
                    if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                    {
                        int mailTemplateId = newCase.ProductArea.MailTemplate.MailID;
                        var m =
                            _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id,
                                newCase.RegLanguage_Id, mailTemplateId);
                        if (m != null)
                        {
                            if (!string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, curMail,
                                    _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 7,
                                    userTimeZone);
                                var siteSelfService =
                                    ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() +
                                    el.EmailLogGUID.ToString();
                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress,
                                    m.Subject, m.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService,
                                    siteHelpdesk);
                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                el.ChangedDate = now;
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                            }
                        }
                    }
                }
            }

            #endregion

            #region Case closed email

            if (newCase.FinishingDate.HasValue && newCase.Customer != null)
            {
                SendCaseClosedEmail(newCase, cms, caseHistoryId, userTimeZone, log, fields, smtpInfo, caseId, files,
                    customerSetting, false, dontSendMailToNotfier, helpdeskMailFromAdress);
            }

            #endregion

            // State Secondary Email TODO ikea ims only??
            // Commented out for now, will be added later with a better solution decided 20150626
            //if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier && !isClosedMailSentToNotifier && oldCase != null && oldCase.Id > 0)  
            //    if (newCase.StateSecondary_Id != oldCase.StateSecondary_Id && newCase.StateSecondary_Id > 0)
            //        if (_emailService.IsValidEmail(newCase.PersonsEmail))
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
            if (!containsProductAreaMailOrNewCaseMail)
            {
                this._caseMailer.InformNotifierIfNeeded(
                    caseHistoryId,
                    fields,
                    log,
                    dontSendMailToNotfier,
                    newCase,
                    helpdeskMailFromAdress,
                    files,
                    cms.CustomeMailFromAddress, isCreatingCase, cms.DontSendMailToNotifier, cms.AbsoluterUrl);
            }

            this._caseMailer.InformAboutInternalLogIfNeeded(
                caseHistoryId,
                fields,
                log,
                newCase,
                helpdeskMailFromAdress,
                files, cms.AbsoluterUrl, cms.CustomeMailFromAddress);
        }

        public void SendProblemLogEmail(Case c, CaseMailSetting cms, int caseHistoryId, TimeZoneInfo userTimeZone, CaseLog caseLog, bool isClosedCaseSending)
        {
            var cs = _caseRepository.GetDetachedCaseById(c.Id);
            var customerSetting = _settingService.GetCustomerSetting(cs.Customer_Id);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }
            List<string> files = null;
            List<Field> fields = GetCaseFieldsForEmail(cs, caseLog, cms, string.Empty, 0, userTimeZone);

            if (isClosedCaseSending)
            {
                SendCaseClosedEmail(c, cms, caseHistoryId, userTimeZone, caseLog, fields, smtpInfo, cs.Id, files, customerSetting, true);
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
                    if (!_emailService.IsValidEmail(helpdeskMailFromAdress))
                    {
                        return;
                    }
                    _caseMailer.InformNotifierIfNeeded(caseHistoryId, fields, caseLog, false, c, helpdeskMailFromAdress, null, cms.CustomeMailFromAddress, false, cms.DontSendMailToNotifier, cms.AbsoluterUrl);
                }
            }
        }

        public void SendSelfServiceCaseLogEmail(int caseId, CaseMailSetting cms, int caseHistoryId, CaseLog log, string basePath, TimeZoneInfo userTimeZone, List<CaseFileDto> logFiles = null, bool caseIsActivated = false)
        {
            // get new case information
            var newCase = _caseRepository.GetDetachedCaseById(caseId);
            var mailTemplateId = 0;

            //get settings for smtp
            var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id);

            var performerUserEmail = string.Empty;
            var externalUpdateMail = 0;
            //get performerUser emailaddress
            if (newCase.Performer_User_Id.HasValue)
            {
                var performerUser = this._userService.GetUser(newCase.Performer_User_Id.Value);
                performerUserEmail = performerUser.Email;
                externalUpdateMail = performerUser.ExternalUpdateMail;
            }

            if (!caseIsActivated)
                mailTemplateId = (int)GlobalEnums.MailTemplates.CaseIsUpdated;
            else
                mailTemplateId = (int)GlobalEnums.MailTemplates.CaseIsActivated;

            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            if (_emailService.IsValidEmail(cms.HelpdeskMailFromAdress))
            {

                List<Field> fields = GetCaseFieldsForEmail(newCase, log, cms, string.Empty, 99, userTimeZone);

                //get sender email adress
                string helpdeskMailFromAdress = cms.HelpdeskMailFromAdress;
                if (newCase.Workinggroup != null)
                    if (!string.IsNullOrWhiteSpace(newCase.Workinggroup.EMail) && _emailService.IsValidEmail(newCase.Workinggroup.EMail))
                        helpdeskMailFromAdress = newCase.Workinggroup.EMail;

                // if logfiles should be attached to the mail 
                List<string> files = null;
                if (logFiles != null && log != null)
                    if (logFiles.Count > 0)
                        files = logFiles.Select(f => _filesStorage.ComposeFilePath(ModuleName.Log, log.Id, basePath, f.FileName)).ToList();

                if (!string.IsNullOrEmpty(performerUserEmail))
                {
                    if (log.SendMailAboutCaseToNotifier && newCase.FinishingDate == null && externalUpdateMail == 1)
                    {
                        var to = performerUserEmail.Split(';', ',').ToList();
                        foreach (var t in to)
                        {
                            var curMail = t.Trim();
                            if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                            {
                                // Inform notifier about external lognote
                                var m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                                if (m != null)
                                {
                                    if (!string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
                                    {
                                        var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                        var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                        var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                        string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                        var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                        var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, log.HighPriority, files, siteSelfService, siteHelpdesk);
                                        el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                        var now = DateTime.Now;
                                        el.CreatedDate = now;
                                        el.ChangedDate = now;
                                        _emailLogRepository.Add(el);
                                        _emailLogRepository.Commit();
                                    }
                                }
                            }
                        }
                    }
                }

                // mail about lognote to Working Group User or Working Group Mail
                if ((!string.IsNullOrEmpty(log.EmailRecepientsInternalLogTo) || !string.IsNullOrEmpty(log.EmailRecepientsInternalLogCc)) && !string.IsNullOrWhiteSpace(log.EmailRecepientsExternalLog))
                {
                    var m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                    if (m != null)
                    {
                        if (!string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
                        {
                            string[] to = log.EmailRecepientsExternalLog.Replace(Environment.NewLine, "|").Split('|');
                            for (int i = 0; i < to.Length; i++)
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, log.HighPriority, files, siteSelfService);
                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                el.ChangedDate = now;
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                            }
                        }
                    }
                }
            }
        }

        #region private

        private void SendEmail(List<string> receivers, MailTemplateLanguageEntity mailTemplate, Case currentCase,
                       CaseLog log, TimeZoneInfo userTimeZone, int caseHistoryId, string basePath,
                       int currentLanguageId, CaseMailSetting caseMailSetting,
                       List<CaseFileDto> logFiles = null)
        {

            if (mailTemplate.MailTemplate == null)
                return;

            if (!string.IsNullOrEmpty(caseMailSetting.HelpdeskMailFromAdress))
                caseMailSetting.HelpdeskMailFromAdress = caseMailSetting.HelpdeskMailFromAdress.Trim();
            else
                return;

            var customerSetting = _settingService.GetCustomerSetting(currentCase.Customer_Id);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            List<string> files = null;
            if (logFiles != null && log != null)
                if (logFiles.Count > 0)
                {
                    var caseFiles = logFiles.Where(x => x.IsCaseFile).Select(x => _filesStorage.ComposeFilePath(ModuleName.Cases, x.ReferenceId, basePath, x.FileName)).ToList();
                    files = logFiles.Where(x => !x.IsCaseFile).Select(f => _filesStorage.ComposeFilePath(ModuleName.Log, f.ReferenceId, basePath, f.FileName)).ToList();
                    files.AddRange(caseFiles);
                }

            foreach (var receiver in receivers)
            {
                var curMail = receiver.Trim();
                if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                {
                    var emailLog = new EmailLog(caseHistoryId, mailTemplate.MailTemplate.MailID, curMail, _emailService.GetMailMessageId(caseMailSetting.HelpdeskMailFromAdress));
                    var fields = GetCaseFieldsForEmail(currentCase, log, caseMailSetting, emailLog.EmailLogGUID.ToString(), 1, userTimeZone);
                    var siteSelfService = ConfigurationManager.AppSettings[AppSettingsKey.SelfServiceAddress].ToString() + emailLog.EmailLogGUID.ToString();
                    var siteHelpdesk = caseMailSetting.AbsoluterUrl + "Cases/edit/" + currentCase.Id.ToString();
                    var mailResponse = EmailResponse.GetEmptyEmailResponse();
                    var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                    var sendResult = _emailService.SendEmail(emailLog, caseMailSetting.HelpdeskMailFromAdress, emailLog.EmailAddress,
                                                             mailTemplate.Subject, mailTemplate.Body, fields,
                                                             mailSetting,
                                                             emailLog.MessageId, false, files, siteSelfService, siteHelpdesk);
                    emailLog.SetResponse(sendResult.SendTime, sendResult.ResponseMessage);
                    var now = DateTime.Now;
                    emailLog.CreatedDate = now;
                    emailLog.ChangedDate = now;
                    _emailLogRepository.Add(emailLog);
                }
            }
            _emailLogRepository.Commit();
        }

        private void SendTemplateEmail(
            GlobalEnums.MailTemplates mailTemplateEnum,
            Case case_,
            CaseLog log,
            int caseHistoryId,
            CaseMailSetting cms,
            string recipient,
            TimeZoneInfo userTimeZone)
        {

            if (!string.IsNullOrEmpty((cms.HelpdeskMailFromAdress)))
            {
                cms.HelpdeskMailFromAdress = cms.HelpdeskMailFromAdress.Trim();
            }

            var customerSetting = _settingService.GetCustomerSetting(case_.Customer_Id);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            var mailTemplateId = (int)mailTemplateEnum;
            var m = this._mailTemplateService.GetMailTemplateForCustomerAndLanguage(case_.Customer_Id, case_.RegLanguage_Id, mailTemplateId);
            if (m != null && !string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
            {
                var el = new EmailLog(
                    caseHistoryId,
                    mailTemplateId,
                    recipient,
                    this._emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));

                var fields = this.GetCaseFieldsForEmail(case_, log, cms, el.EmailLogGUID.ToString(), 3, userTimeZone);

                var siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
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
                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + case_.Id.ToString();

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
                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                var e_res = this._emailService.SendEmail(el, cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, false, null, siteSelfService, siteHelpdesk);
                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                var now = DateTime.Now;
                el.CreatedDate = now;
                el.ChangedDate = now;
                _emailLogRepository.Add(el);
                _emailLogRepository.Commit();
            }
        }

        private void SendPriorityMail(Case newCase, CaseLog log, CaseMailSetting cms, List<string> files, string helpdeskMailFromAdress, int caseHistoryId, int caseId, Setting customerSetting, MailSMTPSetting smtpInfo, TimeZoneInfo userTimeZone)
        {
            var mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToPriority;
            var mt = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
            if (mt != null)
            {
                if (!string.IsNullOrEmpty(mt.Body) && !string.IsNullOrEmpty(mt.Subject))
                {
                    var to = newCase.Priority.EMailList.Split(';', ',').ToList();
                    foreach (var t in to)
                    {
                        var curMail = t.Trim();
                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                        {
                            var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                            var fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 5, userTimeZone);

                            var siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                            var mailResponse = EmailResponse.GetEmptyEmailResponse();
                            var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                            var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                            var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, mt.Subject, mt.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                            el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                            var now = DateTime.Now;
                            el.CreatedDate = now;
                            el.ChangedDate = now;
                            _emailLogRepository.Add(el);
                            _emailLogRepository.Commit();
                        }
                    }
                }
            }
        }

        private void SendPriorityMailSpecial(Case newCase, CaseLog log, CaseMailSetting cms, List<string> files, string helpdeskMailFromAdress, int caseHistoryId, int caseId, Setting customerSetting, MailSMTPSetting smtpInfo, TimeZoneInfo userTimeZone)
        {
            var mailTemplate = new CustomMailTemplate { };

            if (newCase.Priority.MailID_Change.HasValue)
                mailTemplate = this._mailTemplateService.GetCustomMailTemplate(newCase.Priority.MailID_Change.Value);
            {

                //var mailTemplateId = mailTemplate.MailID;

                var mt = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplate.MailId);
                if (mt != null)
                {
                    if (!string.IsNullOrEmpty(mt.Body) && !string.IsNullOrEmpty(mt.Subject))
                    {
                        var to = newCase.Priority.EMailList.Split(';', ',').ToList();
                        foreach (var t in to)
                        {
                            var curMail = t.Trim();
                            if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplate.MailId, curMail, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                var fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 5, userTimeZone);

                                var siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                                var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress, mt.Subject, mt.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                el.ChangedDate = now;
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                            }
                        }
                    }
                }
            }
        }

        private void SendCaseClosedEmail(Case newCase, CaseMailSetting cms, int caseHistoryId, TimeZoneInfo userTimeZone, CaseLog log,
            List<Field> fields, MailSMTPSetting smtpInfo, int caseId, List<string> files, Setting customerSetting,
            bool isProblemSend = false, bool dontSendMailToNotfier = false, string helpdeskMailFromAdress = null)
        {
            int mailTemplateId = (int)GlobalEnums.MailTemplates.ClosedCase;

            string customEmailSender2 = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;
            if (string.IsNullOrWhiteSpace(customEmailSender2))
                customEmailSender2 = cms.CustomeMailFromAddress.WGEmail;
            if (string.IsNullOrWhiteSpace(customEmailSender2))
                customEmailSender2 = cms.CustomeMailFromAddress.SystemEmail;

            if (!string.IsNullOrEmpty(customEmailSender2))
            {
                customEmailSender2 = customEmailSender2.Trim();
            }

            var m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
            if (m != null)
            {
                if (!string.IsNullOrEmpty(m.Body) && !string.IsNullOrEmpty(m.Subject))
                {
                    if (!string.IsNullOrWhiteSpace(newCase.Customer.CloseCaseEmailList))
                    {
                        string[] to = newCase.Customer.CloseCaseEmailList.Split(';');
                        var adminEmails = newCase.Customer.UsersAvailable.Where(x => x.UserGroup_Id != UserGroups.User).Select(x => x.Email).ToList();
                        for (int i = 0; i < to.Length; i++)
                        {
                            if (_emailService.IsValidEmail(to[i]))
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(customEmailSender2));
                                fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 8, userTimeZone);

                                var identifiers = _feedbackTemplateService.FindIdentifiers(m.Body).ToList();
                                //dont send feedback to admins
                                var identifiersToDel = new List<string>();
                                var templateFields = _feedbackTemplateService.GetCustomerTemplates(identifiers, newCase.Customer_Id, newCase.RegLanguage_Id, newCase.Id, cms.AbsoluterUrl);
                                foreach (var templateField in templateFields)
                                {
                                    if (templateField.ExcludeAdministrators && adminEmails.Any(x => x.Equals(to[i])))
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
                                        m.Body = m.Body.Replace(identifier, string.Empty);
                                    }
                                }

                                string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();
                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();

                                var e_res = _emailService.SendEmail(el, customEmailSender2, el.EmailAddress, m.Subject, m.Body, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                el.ChangedDate = now;
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();

                                foreach (var field in templateFields.Where(f => !string.IsNullOrEmpty(f.StringValue)))
                                    _feedbackTemplateService.UpdateFeedbackStatus(field);
                            }
                        }
                    }

                    if (!cms.DontSendMailToNotifier)
                    {
                        var to = newCase.PersonsEmail.Split(';', ',').Select(x => new Tuple<string, bool>(x, true)).ToList();
                        var extraFollowers = _caseExtraFollowersService.GetCaseExtraFollowers(newCase.Id).Select(x => new Tuple<string, bool>(x.Follower, false)).ToList();
                        to.AddRange(extraFollowers);
                        var adminEmails = newCase.Customer.UsersAvailable.Where(x => x.UserGroup_Id != UserGroups.User).Select(x => x.Email).ToList();
                        foreach (var t in to)
                        {
                            var mailBody = m.Body;
                            var curMail = t.Item1.Trim();
                            if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, curMail, _emailService.GetMailMessageId(customEmailSender2));
                                fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 9, userTimeZone);
                                var templateFields = new List<FeedbackField>();
                                var identifiers = _feedbackTemplateService.FindIdentifiers(mailBody).ToList();
                                //dont send feedback to followers and admins
                                var identifiersToDel = new List<string>();
                                if (t.Item2)
                                {
                                    templateFields = _feedbackTemplateService.GetCustomerTemplates(identifiers, newCase.Customer_Id, newCase.RegLanguage_Id, newCase.Id, cms.AbsoluterUrl);
                                    foreach (var templateField in templateFields)
                                    {
                                        if (templateField.ExcludeAdministrators && adminEmails.Any(x => x.Equals(curMail)))
                                        {
                                            identifiersToDel.Add(templateField.Key);
                                        }
                                        else
                                        {
                                            var tf = templateField.MapToFields();
                                            fields.Add(tf);
                                        }
                                    }
                                }
                                foreach (var identifier in identifiersToDel)
                                {
                                    if (!string.IsNullOrEmpty(identifier))
                                    {
                                        mailBody = m.Body.Replace(identifier, string.Empty);
                                    }
                                }

                                string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.EmailLogGUID.ToString();

                                var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                                var e_res = _emailService.SendEmail(el, customEmailSender2, el.EmailAddress, m.Subject, mailBody, fields, mailSetting, el.MessageId, false, files, siteSelfService, siteHelpdesk);
                                el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                el.ChangedDate = now;
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();

                                foreach (var field in templateFields.Where(f => !string.IsNullOrEmpty(f.StringValue)))
                                    _feedbackTemplateService.UpdateFeedbackStatus(field);
                            }
                        }

                    }

                    if (isProblemSend)
                    {
                        // send sms
                        if (newCase.SMS == 1 && !dontSendMailToNotfier && !string.IsNullOrWhiteSpace(newCase.PersonsCellphone))
                        {
                            int smsMailTemplateId = (int)GlobalEnums.MailTemplates.SmsClosedCase;
                            var mt = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, smsMailTemplateId);
                            if (mt != null)
                            {
                                if (!string.IsNullOrEmpty(mt.Body) && !string.IsNullOrEmpty(mt.Subject))
                                {
                                    var smsTo = GetSmsRecipient(customerSetting, newCase.PersonsCellphone);
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, smsTo, _emailService.GetMailMessageId(helpdeskMailFromAdress));

                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(), 10, userTimeZone);

                                    var identifiers = _feedbackTemplateService.FindIdentifiers(mt.Body);

                                    var templateFields =
                                        _feedbackTemplateService.GetCustomerTemplates(identifiers, newCase.Customer_Id, newCase.RegLanguage_Id, newCase.Id, cms.AbsoluterUrl);

                                    fields.AddRange(templateFields.Select(tf => tf.MapToFields()));

                                    var siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"] + el.EmailLogGUID;

                                    var siteHelpdesk = cms.AbsoluterUrl + "Cases/edit/" + caseId.ToString();
                                    var mailResponse = EmailResponse.GetEmptyEmailResponse();
                                    var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);

                                    var e_res = _emailService.SendEmail(el, helpdeskMailFromAdress, el.EmailAddress,
                                        GetSmsSubject(customerSetting), mt.Body, fields, mailSetting, el.MessageId,
                                        false, files, siteSelfService, siteHelpdesk);

                                    el.SetResponse(e_res.SendTime, e_res.ResponseMessage);

                                    var now = DateTime.Now;
                                    el.CreatedDate = now;
                                    el.ChangedDate = now;
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();

                                    foreach (var field in templateFields.Where(f => !string.IsNullOrEmpty(f.StringValue)))
                                        _feedbackTemplateService.UpdateFeedbackStatus(field);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion private
    }
}
