namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.utils;  

    public interface ICaseService
    {
        IList<Case> GetCases();
        IList<Case> GetCasesForStartPage(int customerId);
        Case InitCase(int customerId, int userId, int languageId, string ipAddress, GlobalEnums.RegistrationSource source, Setting customerSetting, string adUser);
        Case GetCaseById(int id, bool markCaseAsRead = false);
        IList<CaseHistory> GetCaseHistoryByCaseId(int caseId);
        int SaveCase(Case cases, CaseLog caseLog, CaseMailSetting caseMailSetting, int userId, string adUser, out IDictionary<string, string> errors);
        int SaveCaseHistory(Case c, int userId, string adUser, out IDictionary<string, string> errors);
        void Commit();
        Guid Delete(int id);
    }

    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseFileRepository _caseFileRepository;
        private readonly ICaseHistoryRepository _caseHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRegionService _regionService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly ISupplierService _supplierServicee;
        private readonly IPriorityService _priorityService;
        private readonly IStatusService _statusService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IProductAreaService _productAreaService;
        private readonly UserRepository userRepository;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IEmailLogRepository _emailLogRepository;
        private readonly IEmailService _emailService;
        private readonly ISettingService _settingService;
        private readonly IFilesStorage _filesStorage;
        private readonly ILogRepository _logRepository;
        private readonly ILogFileRepository _logFileRepository;

        public CaseService(
            ICaseRepository caseRepository,
            ICaseFileRepository caseFileRepository,
            ICaseHistoryRepository caseHistoryRepository,
            ILogRepository logRepository,
            ILogFileRepository logFileRepository,
            IRegionService regionService,
            ICaseTypeService caseTypeService, 
            ISupplierService supplierService, 
            IPriorityService priorityService,
            IStatusService statusService,
            IWorkingGroupService workingGroupService,
            IProductAreaService productAreaService,
            IMailTemplateService mailTemplateService,
            IEmailLogRepository emailLogRepository,
            IEmailService emailService,
            ISettingService settingService,
            IFilesStorage filesStorage,
            IUnitOfWork unitOfWork,
            UserRepository userRepository)
        {
            this._unitOfWork = unitOfWork;
            this._caseRepository = caseRepository;
            this._caseRepository = caseRepository;
            this._regionService = regionService;
            this._caseTypeService = caseTypeService;
            this._supplierServicee = supplierService;
            this._priorityService = priorityService;
            this._statusService = statusService;
            this._workingGroupService = workingGroupService;
            this.userRepository = userRepository;
            this._caseHistoryRepository = caseHistoryRepository;
            this._productAreaService = productAreaService;
            this._mailTemplateService = mailTemplateService;
            this._emailLogRepository = emailLogRepository;
            this._emailService = emailService;
            this._settingService = settingService;
            this._caseFileRepository = caseFileRepository;
            this._filesStorage = filesStorage;
            this._logRepository = logRepository;
            this._logFileRepository = logFileRepository; 
        }

        public Case GetCaseById(int id, bool markCaseAsRead = false)
        {
            return this._caseRepository.GetCaseById(id, markCaseAsRead);
        }

        public Guid Delete(int id)
        {
            Guid ret = Guid.NewGuid(); 

            //TODO kolla om det behövs comit i de olika reposit...
            //TODO formFieldValue tabell ska deletas

            // delete log files
            var logFiles = _logFileRepository.GetLogFilesByCaseId(id); 
            if (logFiles != null)
            {
                foreach (var f in logFiles)
                {
                    _filesStorage.DeleteFile(TopicName.Log, f.Log_Id, f.FileName);
                    _logFileRepository.Delete(f);
                }
            }

            // delete logs
            var logs = this._logRepository.GetLogForCase(id);
            if (logs != null)
            {
                foreach (var l in logs)
                {
                    this._logRepository.Delete(l);  
                }
            }

            // delete email logs
            var elogs = this._emailLogRepository.GetEmailLogsByCaseId(id);
            if (elogs != null)
            {
                foreach (var l in elogs)
                {
                    this._emailLogRepository.Delete(l);
                }
            }

            // delete caseHistory
            var caseHistories = _caseHistoryRepository.GetCaseHistoryByCaseId(id);
            if (caseHistories != null)
            {
                foreach (var h in caseHistories)
                {
                    _caseHistoryRepository.Delete(h);  
                }
            }

            // delete case files
            var caseFiles = _caseFileRepository.GetCaseFilesByCaseId(id);
            if (caseFiles != null)
            {
                foreach (var f in caseFiles)
                {
                    _filesStorage.DeleteFile(TopicName.Cases, f.Case_Id, f.FileName);
                    _caseFileRepository.Delete(f);
                }
            }

            var c = this._caseRepository.GetById(id);
            ret = c.CaseGUID; 
            this._caseRepository.Delete(c);
            this.Commit();

            return ret;
        }

        public Case InitCase(int customerId, int userId, int languageId, string ipAddress, GlobalEnums.RegistrationSource source, Setting customerSetting, string adUser)
        {
            var c = new Case();

            c.Customer_Id = customerId;
            c.User_Id = userId;
            c.CaseResponsibleUser_Id = userId;
            c.IpAddress = ipAddress; 
            c.CaseGUID = Guid.NewGuid();
            c.RegLanguage_Id = languageId;
            c.RegistrationSource = (int)source;
            c.Deleted = 0;
            c.Region_Id = this._regionService.GetDefaultId(customerId);
            c.CaseType_Id = this._caseTypeService.GetDefaultId(customerId);
            c.Supplier_Id = this._supplierServicee.GetDefaultId(customerId);
            c.Priority_Id = this._priorityService.GetDefaultId(customerId);
            c.Status_Id = this._statusService.GetDefaultId(customerId);
            c.WorkingGroup_Id = this._workingGroupService.GetDefaultId(customerId);
            c.RegUserId =  adUser.GetUserFromAdPath();
            c.RegUserDomain = adUser.GetDomainFromAdPath();

            if (customerSetting != null)
            {
                if (customerSetting.SetUserToAdministrator == 1)
                    c.Performer_User_Id = userId;            
                else
                {
                    if (customerSetting.DefaultAdministrator.HasValue) 
                        c.Performer_User_Id = customerSetting.DefaultAdministrator.GetValueOrDefault();
                }
            }
            
            return c;
        }

        public IList<Case> GetCases()
        {
            return this._caseRepository.GetAll().ToList();
        }

        public IList<Case> GetCasesForStartPage(int customerId) 
        {
            return this._caseRepository.GetAll().Where(x => x.Customer_Id == customerId && x.Deleted == 0).ToList();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public int SaveCase(Case cases, CaseLog caseLog, CaseMailSetting caseMailSetting, int userId, string adUser, out IDictionary<string, string> errors)
        {
            int ret = 0;

            if (cases == null)
                throw new ArgumentNullException("cases");

            Case old = new Case();
            if (cases.Id != 0)
                old = _caseRepository.GetDetachedCaseById(cases.Id);  

            Case c = this.ValidateCaseRequiredValues(cases, caseLog); 
            errors = new Dictionary<string, string>();

            // unread/status flag update if not case is closed and not changed by adminsitrator 
            c.Unread = 0;
            if (c.Performer_User_Id != userId && !c.FinishingDate.HasValue)
                c.Unread = 1;

            if (c.Id == 0)
            {
                c.RegTime = DateTime.UtcNow;
                c.ChangeTime = DateTime.UtcNow;
                this._caseRepository.Add(c);
            }
            else
            {
                c.ChangeTime = DateTime.UtcNow;
                c.ChangeByUser_Id = userId;
                this._caseRepository.Update(c);
            }

            if (errors.Count == 0)
                this.Commit();

            // save casehistory
            ret = this.SaveCaseHistory(c, userId, adUser, out errors);

            // send email and sms
            if (caseMailSetting != null)
                SendCaseEmail(c.Id, old, caseLog, caseMailSetting, ret);   

            return ret;
        }

        public int SaveCaseHistory(Case c, int userId, string adUser, out IDictionary<string, string> errors)
        {
            if (c == null)
                throw new ArgumentNullException("caseHistory");

            errors = new Dictionary<string, string>();

            CaseHistory h = this.GenerateHistoryFromCase(c, userId, adUser);
            this._caseHistoryRepository.Add(h);

            if (errors.Count == 0)
                this.Commit();

            return h.Id;
        }

        public IList<CaseHistory> GetCaseHistoryByCaseId(int caseId)
        {
            return this._caseHistoryRepository.GetCaseHistoryByCaseId(caseId).ToList(); 
        }


        private void SendCaseEmail(int caseId, Case oldCase, CaseLog log, CaseMailSetting cms, int caseHistoryId)
        {
            if (_emailService.IsValidEmail(cms.HelpdeskMailFromAdress))
            {
                // get new case information
                var newCase = _caseRepository.GetDetachedCaseById(caseId);
                var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id); 

                // get list of fields to replace [#1] tags in the subjcet and body texts
                List<Field> fields = GetCaseFieldsForEmail(newCase, log, cms);

                // send email about new case to notifier or tblCustomer.NewCaseEmailList
                if (newCase.FinishingDate == null)
                {
                    if (oldCase.Id == 0)
                    {
                        // get mail template 
                        int mailTemplateId = (int)GlobalEnums.MailTemplates.NewCase;
                        if (newCase.ProductArea_Id.HasValue && newCase.ProductArea != null)
                        {
                            // get mail template from productArea
                            if (newCase.ProductArea.MailID.HasValue)
                                mailTemplateId = newCase.ProductArea.MailID.Value;
                        }

                        MailTemplateLanguage m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                        if (m != null)
                        {
                            if (!cms.DontSendMailToNotifier)
                            {
                                if (_emailService.IsValidEmail(newCase.PersonsEmail))
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.PersonsEmail, _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(cms.SendMailAboutNewCaseTo))
                            {
                                string[] to = cms.SendMailAboutNewCaseTo.Split(';');
                                for (int i = 0; i < to.Length; i++)
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                }
                            }
                        }
                    }
                }

                // send email to tblCase.Performer_User_Id
                if (newCase.FinishingDate == null)
                {
                    if (newCase.Performer_User_Id != oldCase.Performer_User_Id)
                    {
                        if (newCase.Administrator != null)
                        {
                            if (_emailService.IsValidEmail(newCase.Administrator.Email) && newCase.Administrator.AllocateCaseMail == 1)
                            {
                                int mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToUser;
                                MailTemplateLanguage m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                                if (m != null)
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.Administrator.Email, _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                }
                            }

                            // send sms to tblCase.Performer_User_Id 
                            if (newCase.Administrator.AllocateCaseSMS == 1 && !string.IsNullOrWhiteSpace(newCase.Administrator.CellPhone) && newCase.Customer != null)
                            {
                                int mailTemplateId = (int)GlobalEnums.MailTemplates.SmsAssignedCaseToUser;
                                MailTemplateLanguage m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                                if (m != null)
                                {
                                    var smsTo = GetSmsRecipient(customerSetting, newCase.Administrator.CellPhone);
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, smsTo, _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, GetSmsSubject(customerSetting), m.Body, fields, el.MessageId);
                                }
                            }
                        }
                    }
                }


                // send email priority has changed
                if (newCase.FinishingDate == null && newCase.Priority_Id != oldCase.Priority_Id)
                {
                    if (newCase.Priority != null)
                    {
                        if (!string.IsNullOrWhiteSpace(newCase.Priority.EMailList))
                        {
                            int mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToPriority;
                            MailTemplateLanguage m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                            if (m != null)
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.Priority.EMailList, _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                                _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                            }
                        }
                    }
                }

                // send email working group has changed
                if (newCase.FinishingDate == null && newCase.WorkingGroup_Id != oldCase.WorkingGroup_Id)
                {
                    if (newCase.Workinggroup != null)
                    {
                        int mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToWorkinggroup;
                        MailTemplateLanguage m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                        if (m != null)
                        {
                            string wgEmails = string.Empty;

                            if (newCase.Workinggroup.AllocateCaseMail == 1)
                                wgEmails = newCase.Workinggroup.EMail;
                            else
                            {
                                if (newCase.Workinggroup.UserWorkingGroups != null && newCase.Workinggroup.AllocateCaseMail == 1)
                                    foreach (var ur in newCase.Workinggroup.UserWorkingGroups)  //TODO behöver vi kolla avdelning?                                
                                    {
                                        if (ur.UserRole == 2)
                                            if (ur.User != null)
                                                if (ur.User.IsActive == 1 && ur.User.AllocateCaseMail == 1 && _emailService.IsValidEmail(ur.User.Email))
                                                {
                                                    wgEmails = wgEmails + ur.User.Email + ";";
                                                }
                                    }
                            }

                            if (!string.IsNullOrWhiteSpace(wgEmails))
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, wgEmails, _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                                _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                            }
                        }
                    }
                }

                // send email when product area is set
                if (newCase.FinishingDate == null && oldCase.ProductAreaSetDate == null && newCase.RegistrationSource == 3 && oldCase.Id > 0)
                    if (!cms.DontSendMailToNotifier && _emailService.IsValidEmail(newCase.PersonsEmail))
                        if (newCase.ProductArea != null)
                            if (newCase.ProductArea.MailID.HasValue)
                                if (newCase.ProductArea.MailID.Value > 0)
                                {
                                    int mailTemplateId = newCase.ProductArea.MailID.Value;
                                    MailTemplateLanguage m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                                    if (m != null)
                                    {
                                        var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.PersonsEmail, _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                                        _emailLogRepository.Add(el);
                                        _emailLogRepository.Commit();
                                        _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                    }
                                }

                // case closed email
                if (newCase.FinishingDate != null && newCase.Customer != null)
                {
                    int mailTemplateId = (int)GlobalEnums.MailTemplates.ClosedCase;
                    MailTemplateLanguage m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                    if (m != null)
                    {
                        if (!string.IsNullOrWhiteSpace(newCase.Customer.CloseCaseEmailList))
                        {

                            string[] to = newCase.Customer.CloseCaseEmailList.Split(';');
                            for (int i = 0; i < to.Length; i++)
                            {
                                if (_emailService.IsValidEmail(to[i]))  
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                }
                            }
                        }

                        if (!cms.DontSendMailToNotifier)
                            if (_emailService.IsValidEmail(newCase.PersonsEmail))
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.PersonsEmail, _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                                _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                            }

                        // send sms
                        if (newCase.SMS == 1 && !string.IsNullOrWhiteSpace(newCase.PersonsCellphone))
                        {
                            int smsMailTemplateId = (int)GlobalEnums.MailTemplates.SmsClosedCase;
                            MailTemplateLanguage mt = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, smsMailTemplateId);
                            if (mt != null)
                            {
                                var smsTo = GetSmsRecipient(customerSetting, newCase.PersonsCellphone);
                                var el = new EmailLog(caseHistoryId, mailTemplateId, smsTo, _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                                _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, GetSmsSubject(customerSetting), mt.Body, fields, el.MessageId);
                            }                                
                        }

                    }
                }

                // Inform notifier checkbox
                if (log != null)
                {
                    if (log.SendMailAboutCaseToNotifier && _emailService.IsValidEmail(newCase.PersonsEmail) && newCase.FinishingDate == null)
                    {
                        int mailTemplateId = (int)GlobalEnums.MailTemplates.InformNotifier;
                        MailTemplateLanguage m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                        if (m != null)
                        {
                            var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.PersonsEmail, _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                            _emailLogRepository.Add(el);
                            _emailLogRepository.Commit();
                            _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, log.HighPriority);
                        }
                    }

                    // Inform notifier checkbox
                    if (log.SendMailAboutLog && !string.IsNullOrWhiteSpace(log.EmailRecepientsInternalLog))
                    {
                        //TODO loopa värden i listan
                        int mailTemplateId = (int)GlobalEnums.MailTemplates.InternalLogNote;
                        MailTemplateLanguage m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                        if (m != null)
                        {
                            var mail = log.EmailRecepientsInternalLog.Replace(Environment.NewLine, ";"); 
                            var el = new EmailLog(caseHistoryId, mailTemplateId, mail, _emailService.GetMailMessageId(cms.HelpdeskMailFromAdress));
                            _emailLogRepository.Add(el);
                            _emailLogRepository.Commit();
                            _emailService.SendEmail(cms.HelpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, log.HighPriority);
                        }
                    }
                }

            }
        }

        private Case ValidateCaseRequiredValues(Case c, CaseLog caseLog)
        {
            Case ret = c;

            ret.PersonsCellphone = string.IsNullOrWhiteSpace(c.PersonsCellphone) ? string.Empty : c.PersonsCellphone;
            ret.PersonsEmail = string.IsNullOrWhiteSpace(c.PersonsEmail) ? string.Empty : c.PersonsEmail;
            ret.PersonsName = string.IsNullOrWhiteSpace(c.PersonsName) ? string.Empty : c.PersonsName;
            ret.PersonsPhone = string.IsNullOrWhiteSpace(c.PersonsPhone) ? string.Empty : c.PersonsPhone;
            ret.Place = string.IsNullOrWhiteSpace(c.Place) ? string.Empty : c.Place;
            ret.InventoryNumber = string.IsNullOrWhiteSpace(c.InventoryNumber) ? string.Empty : c.InventoryNumber;
            ret.InventoryType = string.IsNullOrWhiteSpace(c.InventoryType) ? string.Empty : c.InventoryType;
            ret.InventoryLocation = string.IsNullOrWhiteSpace(c.InventoryLocation) ? string.Empty : c.InventoryLocation;
            ret.InvoiceNumber = string.IsNullOrWhiteSpace(c.InvoiceNumber) ? string.Empty : c.InvoiceNumber;
            ret.Caption = string.IsNullOrWhiteSpace(c.Caption) ? string.Empty : c.Caption;
            ret.Description = string.IsNullOrWhiteSpace(c.Description) ? string.Empty : c.Description;
            ret.Miscellaneous = string.IsNullOrWhiteSpace(c.Miscellaneous) ? string.Empty : c.Miscellaneous;
            ret.Available = string.IsNullOrWhiteSpace(c.Available) ? string.Empty : c.Available;
            ret.IpAddress = string.IsNullOrWhiteSpace(c.IpAddress) ? string.Empty : c.IpAddress;

            if (caseLog != null)
                if (caseLog.FinishingType > 0 && c.FinishingDate == null)
                {
                    c.FinishingDate = caseLog.FinishingDate.HasValue ? caseLog.FinishingDate : DateTime.UtcNow;
                    // för att få med klockslag
                    if (caseLog.FinishingDate.Value.ToShortDateString() == DateTime.Today.ToShortDateString() )  
                        c.FinishingDate = DateTime.UtcNow; 
                }

            return ret;
        }

        private CaseHistory GenerateHistoryFromCase(Case c, int userId, string adUser)
        {
            CaseHistory h = new CaseHistory();

            var user = this.userRepository.GetUser(userId);

            h.AgreedDate = c.AgreedDate;
            h.ApprovedDate = c.AgreedDate;
            h.ApprovedBy_User_Id = c.ApprovedBy_User_Id; 
            h.Available = c.Available;
            h.Caption = c.Caption;
            h.Case_Id = c.Id;
            h.CaseHistoryGUID = Guid.NewGuid(); 
            h.CaseNumber = c.CaseNumber;
            h.CaseResponsibleUser_Id = c.CaseResponsibleUser_Id;
            h.CaseType_Id = c.CaseType_Id; 
            h.Category_Id = c.Category_Id; 
            h.Change_Id = c.Change_Id; 
            h.ContactBeforeAction = c.ContactBeforeAction;
            h.Cost = c.Cost;
            h.CreatedDate = DateTime.UtcNow;
            h.CreatedByUser = user.FirstName + ' ' + user.SurName; 
            h.Currency = c.Currency;
            h.Customer_Id = c.Customer_Id;
            h.Deleted = c.Deleted; 
            h.Department_Id = c.Department_Id;  
            h.Description = c.Description;
            h.ExternalTime = c.ExternalTime;
            h.FinishingDate = c.FinishingDate;
            h.FinishingDescription = c.FinishingDescription;
            h.FollowUpDate = c.FollowUpDate;
            h.Impact_Id = c.Impact_Id;
            h.InvoiceNumber = c.InvoiceNumber; 
            h.InventoryLocation = c.InventoryLocation;
            h.InventoryNumber = c.InventoryNumber;
            h.InventoryType = c.InventoryType;
            h.IpAddress = c.IpAddress;
            h.Miscellaneous = c.Miscellaneous;
            h.LockCaseToWorkingGroup_Id = c.LockCaseToWorkingGroup_Id; 
            h.OU_Id = c.OU_Id; 
            h.OtherCost = c.OtherCost;
            h.Performer_User_Id = c.Performer_User_Id; 
            h.PersonsCellphone = c.PersonsCellphone;
            h.PersonsEmail = c.PersonsEmail;
            h.PersonsName = c.PersonsName;
            h.PersonsPhone = c.PersonsPhone;
            h.Place = c.Place;
            h.PlanDate = c.PlanDate;
            h.Priority_Id = c.Priority_Id; 
            h.ProductArea_Id = c.ProductArea_Id; 
            h.ProductAreaSetDate = c.ProductAreaSetDate;
            h.Project_Id = c.Project_Id;
            h.Problem_Id = c.Problem_Id; 
            h.ReferenceNumber = c.ReferenceNumber;
            h.RegistrationSource = c.RegistrationSource;
            h.RegLanguage_Id = c.RegLanguage_Id;
            h.RegUserDomain = adUser.GetDomainFromAdPath();
            h.RegUserId = adUser.GetUserFromAdPath(); 
            h.RelatedCaseNumber = c.RelatedCaseNumber;
            h.Region_Id = c.Region_Id; 
            h.ReportedBy = c.ReportedBy;
            h.Status_Id = c.Status_Id; 
            h.StateSecondary_Id = c.StateSecondary_Id; 
            h.SMS = c.SMS;
            h.SolutionRate = c.SolutionRate;
            h.Supplier_Id = c.Supplier_Id; 
            h.System_Id = c.System_Id; 
            h.UserCode = c.UserCode;
            h.User_Id = c.User_Id;
            h.Urgency_Id = c.Urgency_Id; 
            h.Unread = c.Unread; 
            h.WatchDate = c.WatchDate;
            h.Verified = c.Verified;
            h.VerifiedDescription = c.VerifiedDescription; 
            h.WorkingGroup_Id = c.WorkingGroup_Id; 

            return h;
        }

        private List<Field> GetCaseFieldsForEmail(Case c, CaseLog l, CaseMailSetting cms)
        {
            List<Field> ret = new List<Field>();

            ret.Add(new Field { Key = "[#1]", StringValue = c.CaseNumber.ToString() } );
            ret.Add(new Field { Key = "[#16]", StringValue = c.RegTime.ToString() } ); 
            ret.Add(new Field { Key = "[#22]", StringValue = c.LastChangedByUser != null ? c.LastChangedByUser.FirstName + " " + c.LastChangedByUser.SurName : string.Empty }); 
            ret.Add(new Field { Key = "[#3]", StringValue = c.PersonsName } ); 
            ret.Add(new Field { Key = "[#8]", StringValue = c.PersonsEmail } ); 
            ret.Add(new Field { Key = "[#9]", StringValue = c.PersonsPhone } );
            ret.Add(new Field { Key = "[#18]", StringValue = c.PersonsCellphone } ); 
            ret.Add(new Field { Key = "[#2]", StringValue = c.Customer != null ? c.Customer.Name : string.Empty } ); 
            ret.Add(new Field { Key = "[#24]", StringValue = c.Place } ); 
            ret.Add(new Field { Key = "[#17]", StringValue = c.InventoryNumber } ); 
            ret.Add(new Field { Key = "[#25]", StringValue = c.CaseType != null ? c.CaseType.Name : string.Empty });
            ret.Add(new Field { Key = "[#26]", StringValue = c.Category != null ? c.Category.Name : string.Empty } ); 
            ret.Add(new Field { Key = "[#4]", StringValue = c.Caption } ); 
            ret.Add(new Field { Key = "[#5]", StringValue = c.Description } ); 
            ret.Add(new Field { Key = "[#23]", StringValue = c.Miscellaneous } ); 
            ret.Add(new Field { Key = "[#19]", StringValue = c.Available } ); 
            ret.Add(new Field { Key = "[#15]", StringValue = c.Workinggroup != null ? c.Workinggroup.WorkingGroupName : string.Empty });
            ret.Add(new Field { Key = "[#13]", StringValue = c.Workinggroup != null ? c.Workinggroup.EMail : string.Empty }); 
            ret.Add(new Field { Key = "[#6]", StringValue = c.Administrator != null ? c.Administrator.FirstName : string.Empty }); 
            ret.Add(new Field { Key = "[#7]", StringValue = c.Administrator != null ? c.Administrator.SurName : string.Empty }); 
            ret.Add(new Field { Key = "[#12]", StringValue = c.Priority != null ? c.Priority.Name : string.Empty });
            ret.Add(new Field { Key = "[#20]", StringValue = c.Priority != null ? c.Priority.Description : string.Empty });
            ret.Add(new Field { Key = "[#21]", StringValue = c.WatchDate.ToString() } );
            if (l != null)
            {
                ret.Add(new Field { Key = "[#10]", StringValue = l.TextExternal });
                ret.Add(new Field { Key = "[#11]", StringValue = l.TextInternal });
            }
            // selfservice site
            if (cms != null)
            {
                string site = cms.AbsoluterUrl + "Selfservice/ci.asp?id=" + c.CaseGUID.ToString();  
                string url = "<br><a href='" + site + "'>" + site + "</a>";
                ret.Add(new Field { Key = "[#98]", StringValue = url });
            }
            // heldesk site
            if (cms != null)
            {
                string site = cms.AbsoluterUrl + "Cases/edit/" + c.Id.ToString();
                string url = "<br><a href='" + site +  "'>" + site + "</a>";
                ret.Add(new Field { Key = "[#99]", StringValue = url });
            }

            return ret;
        }

        private string GetSmsSubject(Setting cs)
        {
            string ret = string.Empty; 
            if (cs != null)
            {
                ret = cs.SMSEMailDomainUserName.Left(11);
                if (!string.IsNullOrWhiteSpace(cs.SMSEMailDomainPassword))
                    ret = ret + " " + cs.SMSEMailDomainPassword;
            }
            return ret;
        }

        private string GetSmsRecipient(Setting cs, string phone)
        {
            string ret = string.Empty; 
            if (cs != null)
            {
                ret = phone.RemoveNonNumericValuesFromString() + (cs.SMSEMailDomain.StartsWith("@") ? string.Empty : "@") + cs.SMSEMailDomain;
            }
            return ret;
        }
    }
}
