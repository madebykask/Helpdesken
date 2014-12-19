namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.BusinessData.Models.Reports.Output;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.MailTemplates;
    using DH.Helpdesk.Domain.Problems;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Customers;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Customers;
    using DH.Helpdesk.Services.Infrastructure.Email;
    using DH.Helpdesk.Services.utils;
    using DH.Helpdesk.BusinessData.Models.User.Input;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

    public interface ICaseService
    {
        IList<Case> GetCases();

        IList<Case> GetCasesByCustomers(IEnumerable<int> customerIds);
        
        Case InitCase(int customerId, int userId, int languageId, string ipAddress, GlobalEnums.RegistrationSource source, Setting customerSetting, string adUser);
        Case Copy(int copyFromCaseid, int userId, int languageId, string ipAddress, GlobalEnums.RegistrationSource source, string adUser);
        Case GetCaseById(int id, bool markCaseAsRead = false);
        Case GetDetachedCaseById(int id);
        Case GetCaseByGUID(Guid GUID);
        Case GetCaseByEMailGUID(Guid GUID);
        EmailLog GetEMailLogByGUID(Guid GUID);             
        IList<CaseHistory> GetCaseHistoryByCaseId(int caseId);
        List<DynamicCase> GetAllDynamicCases();

        int SaveCase(
            Case cases, 
            CaseLog caseLog, 
            CaseMailSetting caseMailSetting, 
            int userId, 
            string adUser,           
            out IDictionary<string, string> errors,
            CaseInvoice[] invoices = null);

        int SaveCaseHistory(Case c, int userId, string adUser, out IDictionary<string, string> errors, string defaultUser = "");
        void SendCaseEmail(int caseId, CaseMailSetting cms, int caseHistoryId, Case oldCase = null, CaseLog log = null, List<CaseFileDto> logFiles = null);
        void UpdateFollowUpDate(int caseId, DateTime? time);
        void MarkAsUnread(int caseId);
        void SendSelfServiceCaseLogEmail(int caseId, CaseMailSetting cms, int caseHistoryId, CaseLog log, List<CaseFileDto> logFiles = null);
        void Activate(int caseId, int userId, string adUser, out IDictionary<string, string> errors);
        IList<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user);
        void Commit();
        Guid Delete(int id);

        /// <summary>
        /// The get case overview.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <returns>
        /// The <see cref="CaseOverview"/>.
        /// </returns>
        CaseOverview GetCaseOverview(int caseId);

        IEnumerable<RegistratedCasesCaseTypeItem> GetRegistratedCasesCaseTypeItems(
                                                int customerId,
                                                int[] workingGroups,
                                                int[] caseTypes,
                                                int? productArea,
                                                DateTime perionFrom,
                                                DateTime perionUntil);

        IEnumerable<RegistratedCasesDayItem> GetRegistratedCasesDayItems(
                                            int customerId,
                                            int? departmentId,
                                            int[] caseTypesIds,
                                            int? workingGroupId,
                                            int? administratorId,
                                            DateTime period);

        MyCase[] GetMyCases(int userId, int? count = null);

        CustomerCases[] GetCustomersCases(int[] customerIds, int userId);
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
        private readonly UserRepository userRepository;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IEmailLogRepository _emailLogRepository;
        private readonly IEmailService _emailService;
        private readonly ISettingService _settingService;
        private readonly IFilesStorage _filesStorage;
        private readonly ILogRepository _logRepository;
        private readonly ILogFileRepository _logFileRepository;
        private readonly IFormFieldValueRepository _formFieldValueRepository;

        private readonly ICaseMailer caseMailer;

        private readonly IInvoiceArticleService invoiceArticleService;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private ISurveyService surveyService;

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
            IMailTemplateService mailTemplateService,
            IEmailLogRepository emailLogRepository,
            IEmailService emailService,
            ISettingService settingService,
            IFilesStorage filesStorage,
            IUnitOfWork unitOfWork,
            IFormFieldValueRepository formFieldValueRepository,
            UserRepository userRepository, 
            ICaseMailer caseMailer, 
            IInvoiceArticleService invoiceArticleService, 
            IUnitOfWorkFactory unitOfWorkFactory,
            ISurveyService surveyService)
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
            this.caseMailer = caseMailer;
            this.invoiceArticleService = invoiceArticleService;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this._caseHistoryRepository = caseHistoryRepository;
            this._mailTemplateService = mailTemplateService;
            this._emailLogRepository = emailLogRepository;
            this._emailService = emailService;
            this._settingService = settingService;
            this._caseFileRepository = caseFileRepository;
            this._filesStorage = filesStorage;
            this._logRepository = logRepository;
            this._logFileRepository = logFileRepository;
            this._formFieldValueRepository = formFieldValueRepository;
            this.surveyService = surveyService;
        }

        public Case GetCaseById(int id, bool markCaseAsRead = false)
        {
            return this._caseRepository.GetCaseById(id, markCaseAsRead);
        }

        public Case GetDetachedCaseById(int id)
        {
            return this._caseRepository.GetDetachedCaseById(id);
        }

        public Case GetCaseByGUID(Guid GUID)
        {
            return this._caseRepository.GetCaseByGUID(GUID);
        }

        public Case GetCaseByEMailGUID(Guid GUID)
        {
            return this._caseRepository.GetCaseByEmailGUID(GUID);
        }

        public EmailLog GetEMailLogByGUID(Guid GUID)
        {
            return _emailLogRepository.GetEmailLogsByGuid(GUID);
        }

        public List<DynamicCase> GetAllDynamicCases()
        {
            return _caseRepository.GetAllDynamicCases();
        }

        public Guid Delete(int id)
        {
            Guid ret = Guid.Empty; 

            // delete form field values
            var ffv = this._formFieldValueRepository.GetFormFieldValuesByCaseId(id);
            if (ffv != null)
            {
                foreach (var v in ffv)
                {
                    this._formFieldValueRepository.Delete(v);
                }
                this._formFieldValueRepository.Commit();  
            }

            // delete log files
            var logFiles = this._logFileRepository.GetLogFilesByCaseId(id); 
            if (logFiles != null)
            {
                foreach (var f in logFiles)
                {
                    this._filesStorage.DeleteFile(ModuleName.Log, f.Log_Id, f.FileName);
                    this._logFileRepository.Delete(f);
                }
                this._logFileRepository.Commit();  
            }

            // delete logs
            var logs = this._logRepository.GetLogForCase(id);
            if (logs != null)
            {
                foreach (var l in logs)
                {
                    this._logRepository.Delete(l);  
                }
                this._logRepository.Commit();  
            }

            // delete email logs
            var elogs = this._emailLogRepository.GetEmailLogsByCaseId(id);
            if (elogs != null)
            {
                foreach (var l in elogs)
                {
                    this._emailLogRepository.Delete(l);
                }
                this._emailLogRepository.Commit(); 
            }

            // delete caseHistory
            var caseHistories = this._caseHistoryRepository.GetCaseHistoryByCaseId(id);
            if (caseHistories != null)
            {
                foreach (var h in caseHistories)
                {
                    this._caseHistoryRepository.Delete(h);  
                }
                this._caseHistoryRepository.Commit(); 
            }

            // delete case files
            var caseFiles = this._caseFileRepository.GetCaseFilesByCaseId(id);
            if (caseFiles != null)
            {
                foreach (var f in caseFiles)
                {
                    this._filesStorage.DeleteFile(ModuleName.Cases, f.Case_Id, f.FileName);
                    this._caseFileRepository.Delete(f);
                }
                this._caseFileRepository.Commit(); 
            }

            this.invoiceArticleService.DeleteCaseInvoices(id);

            var c = this._caseRepository.GetById(id);
            ret = c.CaseGUID; 
            this._caseRepository.Delete(c);
            this._caseRepository.Commit();

            return ret;
        }

        /// <summary>
        /// The get case overview.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <returns>
        /// The <see cref="CaseOverview"/>.
        /// </returns>
        public CaseOverview GetCaseOverview(int caseId)
        {
            return this._caseRepository.GetCaseOverview(caseId);
        }

        public IEnumerable<RegistratedCasesCaseTypeItem> GetRegistratedCasesCaseTypeItems(
            int customerId,
            int[] workingGroups,
            int[] caseTypes,
            int? productArea,
            DateTime perionFrom,
            DateTime perionUntil)
        {
            return this._caseRepository.GetRegistratedCasesCaseTypeItems(
                                    customerId,
                                    workingGroups,
                                    caseTypes,
                                    productArea,
                                    perionFrom,
                                    perionUntil);
        }

        public IEnumerable<RegistratedCasesDayItem> GetRegistratedCasesDayItems(
            int customerId,
            int? departmentId,
            int[] caseTypesIds,
            int? workingGroupId,
            int? administratorId,
            DateTime period)
        {
            return this._caseRepository.GetRegistratedCasesDayItems(
                                    customerId,
                                    departmentId,
                                    caseTypesIds,
                                    workingGroupId,
                                    administratorId,
                                    period);
        }

        public MyCase[] GetMyCases(int userId, int? count = null)
        {
            return this._caseRepository.GetMyCases(userId, count);
        }

        public CustomerCases[] GetCustomersCases(int[] customerIds, int userId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var customerRepository = uow.GetRepository<Customer>();
                var problemsRep = uow.GetRepository<Problem>();

                var customerCases = customerRepository.GetAll()
                                    .GetByIds(customerIds)
                                    .MapToCustomerCases(problemsRep.GetAll(), userId);

                return customerCases;
            }
        }

        public Case Copy(int copyFromCaseid, int userId, int languageId, string ipAddress, GlobalEnums.RegistrationSource source, string adUser)
        {
            var c = this._caseRepository.GetDetachedCaseById(copyFromCaseid);
            c.IpAddress = ipAddress;
            c.CaseGUID = Guid.NewGuid();
            c.Id = 0;
            c.CaseNumber = 0;
            c.CaseResponsibleUser_Id = userId;
            c.FinishingDate = null;
            c.RegistrationSource = (int)source;
            c.RegUserId = adUser.GetUserFromAdPath();
            c.RegUserDomain = adUser.GetDomainFromAdPath();
            c.CaseFiles = null;
            return c;
        }

        public void MarkAsUnread(int caseId)
        {
            this._caseRepository.MarkCaseAsUnread(caseId);
        }

        public IList<CaseRelation> GetRelatedCases(int id, int customerId, string reportedBy, UserOverview user)
        {
            return this._caseRepository.GetRelatedCases(id, customerId, reportedBy, user).OrderByDescending(c => c.Id).ToList();      
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
            c.WorkingGroup_Id = this._workingGroupService.GetDefaultId(customerId, userId);
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

        public IList<Case> GetCasesByCustomers(IEnumerable<int> customerIds) 
        {
            return this._caseRepository
                .GetMany(x => customerIds.Contains(x.Customer_Id) && 
                         x.Deleted == 0)
                 .ToList();
        }

        public void UpdateFollowUpDate(int caseId, DateTime? time)
        {
            this._caseRepository.UpdateFollowUpDate(caseId, time);  
        }

        public void Activate(int caseId, int userId, string adUser, out IDictionary<string, string> errors)
        {
            this._caseRepository.Activate(caseId);
            var c = _caseRepository.GetDetachedCaseById(caseId);
            SaveCaseHistory(c, userId, adUser, out errors);  
        }

        public void SendSelfServiceCaseLogEmail(int caseId, CaseMailSetting cms, int caseHistoryId, CaseLog log, List<CaseFileDto> logFiles = null)
        {
            if (_emailService.IsValidEmail(cms.HelpdeskMailFromAdress))
            {
                // get new case information
                var newCase = _caseRepository.GetDetachedCaseById(caseId);                

                List<Field> fields = GetCaseFieldsForEmail(newCase, log, cms, string.Empty, 99);

                //get sender email adress
                string helpdeskMailFromAdress = cms.HelpdeskMailFromAdress;
                if (newCase.Workinggroup != null)
                    if (!string.IsNullOrWhiteSpace(newCase.Workinggroup.EMail) && _emailService.IsValidEmail(newCase.Workinggroup.EMail))
                        helpdeskMailFromAdress = newCase.Workinggroup.EMail;

                // if logfiles should be attached to the mail 
                List<string> files = null;
                if (logFiles != null && log != null)
                    if (logFiles.Count > 0)
                        files = logFiles.Select(f => _filesStorage.ComposeFilePath(ModuleName.Log, log.Id, f.FileName)).ToList();

                if (newCase.Administrator != null)
                {
                    if (log.SendMailAboutCaseToNotifier && _emailService.IsValidEmail(newCase.Administrator.Email) && newCase.FinishingDate == null)
                    {
                        // Inform notifier about external lognote
                        int mailTemplateId = (int)GlobalEnums.MailTemplates.CaseIsUpdated;
                        MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                        if (m != null)
                        {
                            var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.Administrator.Email, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                            _emailLogRepository.Add(el);
                            _emailLogRepository.Commit();
                            _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, log.HighPriority, files);
                        }
                    }
                }

                // mail about lognote to Working Group User or Working Group Mail
                if (log.SendMailAboutLog && !string.IsNullOrWhiteSpace(log.EmailRecepientsExternalLog) )
                {
                    int mailTemplateId = (int)GlobalEnums.MailTemplates.CaseIsUpdated;
                    MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                    if (m != null)
                    {
                        string[] to = log.EmailRecepientsExternalLog.Replace(Environment.NewLine, "|").Split('|');
                        for (int i = 0; i < to.Length; i++)
                        {
                            var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(helpdeskMailFromAdress));
                            _emailLogRepository.Add(el);
                            _emailLogRepository.Commit();
                            _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId, log.HighPriority, files);
                        }
                    }
                }
            }
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public int SaveCase(
                Case cases, 
                CaseLog caseLog, 
                CaseMailSetting caseMailSetting, 
                int userId, 
                string adUser, 
                out IDictionary<string, string> errors,
                CaseInvoice[] invoices = null)
        {
            int ret = 0;

            if (cases == null)
                throw new ArgumentNullException("cases");

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
                if (userId == 0) c.ChangeByUser_Id = null;
                else
                 c.ChangeByUser_Id = userId;

                this._caseRepository.Update(c);
            }

            if (errors.Count == 0)
                this._caseRepository.Commit();

            // save casehistory
            if (userId == 0)
                ret = this.SaveCaseHistory(c, userId, adUser, out errors, adUser);    
            else            
                ret = this.SaveCaseHistory(c, userId, adUser, out errors);

            if (invoices != null)
            {
                this.invoiceArticleService.SaveCaseInvoices(invoices, cases.Id);                
            }
            
            return ret;
        }

        public int SaveCaseHistory(Case c, int userId, string adUser, out IDictionary<string, string> errors, string defaultUser = "")
        {
            if (c == null)
                throw new ArgumentNullException("caseHistory");

            errors = new Dictionary<string, string>();

            CaseHistory h = this.GenerateHistoryFromCase(c, userId, adUser, defaultUser);
            this._caseHistoryRepository.Add(h);

            if (errors.Count == 0)
                this._caseHistoryRepository.Commit();

            return h.Id;
        }

        public IList<CaseHistory> GetCaseHistoryByCaseId(int caseId)
        {
            return this._caseHistoryRepository.GetCaseHistoryByCaseId(caseId).ToList(); 
        }

        public void SendCaseEmail(int caseId, CaseMailSetting cms, int caseHistoryId, Case oldCase = null, CaseLog log = null, List<CaseFileDto> logFiles = null)
        {
            if (_emailService.IsValidEmail(cms.HelpdeskMailFromAdress))
            {
                // get new case information
                var newCase = _caseRepository.GetDetachedCaseById(caseId);
                var customerSetting = _settingService.GetCustomerSetting(newCase.Customer_Id);
                bool dontSendMailToNotfier = false;

                // get list of fields to replace [#1] tags in the subjcet and body texts
                List<Field> fields = GetCaseFieldsForEmail(newCase, log, cms, string.Empty , 0);

                //get sender email adress
                string helpdeskMailFromAdress = cms.HelpdeskMailFromAdress;
                if (newCase.Workinggroup != null)
                    if (!string.IsNullOrWhiteSpace(newCase.Workinggroup.EMail) && _emailService.IsValidEmail(newCase.Workinggroup.EMail))  
                        helpdeskMailFromAdress = newCase.Workinggroup.EMail;

                // if logfiles should be attached to the mail 
                List<string> files = null;
                if (logFiles != null && log != null)
                    if (logFiles.Count > 0)
                        files = logFiles.Select(f => _filesStorage.ComposeFilePath(ModuleName.Log, log.Id, f.FileName)).ToList();

                // sub state should not generate email to notifier
                if (newCase.StateSecondary != null)
                    dontSendMailToNotfier = newCase.StateSecondary.NoMailToNotifier == 1 ? true : false; 

                // send email about new case to notifier or tblCustomer.NewCaseEmailList
                if (newCase.FinishingDate == null && oldCase != null)
                {
                    if (oldCase.Id == 0)
                    {
                        // get mail template 
                        int mailTemplateId = (int)GlobalEnums.MailTemplates.NewCase;
                        string customEmailSender1 = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;

                        if (string.IsNullOrWhiteSpace(customEmailSender1))
                            customEmailSender1 = cms.CustomeMailFromAddress.WGEmail;

                        if (string.IsNullOrWhiteSpace(customEmailSender1))
                            customEmailSender1 = cms.CustomeMailFromAddress.SystemEmail;

                        if (newCase.ProductArea_Id.HasValue && newCase.ProductArea != null)
                        {
                            // get mail template from productArea
                            if (newCase.ProductArea.MailID.HasValue)
                                mailTemplateId = newCase.ProductArea.MailID.Value;
                        }

                        MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                        if (m != null)
                        {
                            if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier)
                            {
                                if (_emailService.IsValidEmail(newCase.PersonsEmail))
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.PersonsEmail, _emailService.GetMailMessageId(customEmailSender1));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(),1);
                                   
                                    _emailService.SendEmail(customEmailSender1, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);                                    
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(cms.SendMailAboutNewCaseTo))
                            {
                                string[] to = cms.SendMailAboutNewCaseTo.Split(';');
                                for (int i = 0; i < to.Length; i++)
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(customEmailSender1));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(),2);
                                    _emailService.SendEmail(customEmailSender1, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                }
                            }
                        }
                    }
                }

                // send email to tblCase.Performer_User_Id
                if (newCase.FinishingDate == null && oldCase != null)
                {
                    if (newCase.Performer_User_Id != oldCase.Performer_User_Id)
                    {
                        if (newCase.Administrator != null)
                        {
                            if (_emailService.IsValidEmail(newCase.Administrator.Email) && newCase.Administrator.AllocateCaseMail == 1)
                            {
                                int mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToUser;
                                MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                                if (m != null)
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.Administrator.Email, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(),3);
                                    _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                }
                            }

                            // send sms to tblCase.Performer_User_Id 
                            if (newCase.Administrator.AllocateCaseSMS == 1 && !string.IsNullOrWhiteSpace(newCase.Administrator.CellPhone) && newCase.Customer != null)
                            {
                                int mailTemplateId = (int)GlobalEnums.MailTemplates.SmsAssignedCaseToUser;
                                MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                                if (m != null)
                                {
                                    var smsTo = GetSmsRecipient(customerSetting, newCase.Administrator.CellPhone);
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, smsTo, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(),4);
                                    _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, GetSmsSubject(customerSetting), m.Body, fields, el.MessageId);
                                }
                            }
                        }
                    }
                }

                // send email priority has changed
                if (newCase.FinishingDate == null && oldCase != null)
                    if (newCase.Priority_Id != oldCase.Priority_Id)
                    {
                        if (newCase.Priority != null)
                        {
                            if (!string.IsNullOrWhiteSpace(newCase.Priority.EMailList))
                            {
                                int mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToPriority;
                                MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                                if (m != null)
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.Priority.EMailList, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(),5);
                                    _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                }
                            }
                        }
                    }

                // send email working group has changed
                if (newCase.FinishingDate == null && oldCase != null)
                    if (newCase.WorkingGroup_Id != oldCase.WorkingGroup_Id)
                    {
                        if (newCase.Workinggroup != null)
                        {
                            int mailTemplateId = (int)GlobalEnums.MailTemplates.AssignedCaseToWorkinggroup;
                            MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
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
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, wgEmails, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(),6);
                                    _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                }
                            }
                        }
                    }

                // send email when product area is set
                if (newCase.FinishingDate == null && oldCase != null)
                    if (oldCase.ProductAreaSetDate == null && newCase.RegistrationSource == 3 && oldCase.Id > 0)
                        if (!cms.DontSendMailToNotifier && _emailService.IsValidEmail(newCase.PersonsEmail))
                            if (newCase.ProductArea != null)
                                if (newCase.ProductArea.MailID.HasValue)
                                    if (newCase.ProductArea.MailID.Value > 0)
                                    {
                                        int mailTemplateId = newCase.ProductArea.MailID.Value;
                                        MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                                        if (m != null)
                                        {
                                            var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.PersonsEmail, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                            _emailLogRepository.Add(el);
                                            _emailLogRepository.Commit();
                                            fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(),7);
                                            _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                        }
                                    }

                // case closed email
                if (newCase.FinishingDate.HasValue && newCase.Customer != null)
                {
                    int mailTemplateId = (int)GlobalEnums.MailTemplates.ClosedCase;

                    string customEmailSender2 = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;
                    if (string.IsNullOrWhiteSpace(customEmailSender2))
                        customEmailSender2 = cms.CustomeMailFromAddress.WGEmail;
                    if (string.IsNullOrWhiteSpace(customEmailSender2))
                        customEmailSender2 = cms.CustomeMailFromAddress.SystemEmail;

                    MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                    if (m != null)
                    {
                        if (!string.IsNullOrWhiteSpace(newCase.Customer.CloseCaseEmailList))
                        {
                            string[] to = newCase.Customer.CloseCaseEmailList.Split(';');
                            for (int i = 0; i < to.Length; i++)
                            {
                                if (_emailService.IsValidEmail(to[i]))  
                                {
                                    var el = new EmailLog(caseHistoryId, mailTemplateId, to[i], _emailService.GetMailMessageId(customEmailSender2));
                                    _emailLogRepository.Add(el);
                                    _emailLogRepository.Commit();
                                    fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(),8);
                                    _emailService.SendEmail(customEmailSender2, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                                }
                            }
                        }

                        if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier)
                            if (_emailService.IsValidEmail(newCase.PersonsEmail))
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.PersonsEmail, _emailService.GetMailMessageId(customEmailSender2));
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                                fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(),9);
                                _emailService.SendEmail(customEmailSender2, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                            }

                        // send sms
                        if (newCase.SMS == 1 &&  !dontSendMailToNotfier && !string.IsNullOrWhiteSpace(newCase.PersonsCellphone))
                        {
                            int smsMailTemplateId = (int)GlobalEnums.MailTemplates.SmsClosedCase;
                            MailTemplateLanguageEntity mt = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, smsMailTemplateId);
                            if (mt != null)
                            {
                                var smsTo = GetSmsRecipient(customerSetting, newCase.PersonsCellphone);
                                var el = new EmailLog(caseHistoryId, mailTemplateId, smsTo, _emailService.GetMailMessageId(helpdeskMailFromAdress));
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                                fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(),10);
                                _emailService.SendEmail(helpdeskMailFromAdress, el.EmailAddress, GetSmsSubject(customerSetting), mt.Body, fields, el.MessageId);
                            }                                
                        }

                    }
                }

                // State Secondary Email TODO ikea ims only?? 
                if (!cms.DontSendMailToNotifier && !dontSendMailToNotfier && oldCase != null)  
                    if (newCase.StateSecondary_Id != oldCase.StateSecondary_Id && newCase.StateSecondary_Id > 0)
                        if (_emailService.IsValidEmail(newCase.PersonsEmail))
                        {
                            int mailTemplateId = (int)GlobalEnums.MailTemplates.ClosedCase;

                            string customEmailSender3 = cms.CustomeMailFromAddress.DefaultOwnerWGEMail;
                            if (string.IsNullOrWhiteSpace(customEmailSender3))
                                customEmailSender3 = cms.CustomeMailFromAddress.WGEmail;
                            if (string.IsNullOrWhiteSpace(customEmailSender3))
                                customEmailSender3 = cms.CustomeMailFromAddress.SystemEmail;

                            MailTemplateLanguageEntity m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(newCase.Customer_Id, newCase.RegLanguage_Id, mailTemplateId);
                            if (m != null)
                            {
                                var el = new EmailLog(caseHistoryId, mailTemplateId, newCase.PersonsEmail, _emailService.GetMailMessageId(customEmailSender3));
                                _emailLogRepository.Add(el);
                                _emailLogRepository.Commit();
                                fields = GetCaseFieldsForEmail(newCase, log, cms, el.EmailLogGUID.ToString(),11);
                                _emailService.SendEmail(customEmailSender3, el.EmailAddress, m.Subject, m.Body, fields, el.MessageId);
                            }
                        }

                this.caseMailer.InformNotifierIfNeeded(
                                            caseHistoryId,
                                            fields,
                                            log,
                                            dontSendMailToNotfier,
                                            newCase,
                                            helpdeskMailFromAdress,
                                            files,
                                            cms.CustomeMailFromAddress);

                //this.caseMailer.InformOwnerDefaultGroupIfNeeded(
                //                            caseHistoryId,
                //                            fields,
                //                            log,
                //                            dontSendMailToNotfier,
                //                            newCase,
                //                            helpdeskMailFromAdress,
                //                            files);

                this.caseMailer.InformAboutInternalLogIfNeeded(
                                            caseHistoryId,
                                            fields,
                                            log,
                                            newCase,
                                            helpdeskMailFromAdress,
                                            files);
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
                    if (!caseLog.FinishingDate.HasValue) 
                        caseLog.FinishingDate  = DateTime.UtcNow;    
                    c.FinishingDate = caseLog.FinishingDate.HasValue ? caseLog.FinishingDate : DateTime.UtcNow;
                    // för att få med klockslag
                    if (caseLog.FinishingDate.Value.ToShortDateString() == DateTime.Today.ToShortDateString() )  
                        c.FinishingDate = DateTime.UtcNow; 
                }

            return ret;
        }

        private CaseHistory GenerateHistoryFromCase(Case c, int userId, string adUser, string defaultUser="")
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
            if (defaultUser != "") h.CreatedByUser = defaultUser; // used for Self Service Project
            else
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
            h.CausingPartId = c.CausingPartId;
            h.DefaultOwnerWG_Id = c.DefaultOwnerWG_Id;

            return h;
        }

        private List<Field> GetCaseFieldsForEmail(Case c, CaseLog l, CaseMailSetting cms, string emailLogGuid,int stateHelper)
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
                if (emailLogGuid == string.Empty)
                    emailLogGuid = " >> *" + stateHelper.ToString() + "*";
                string site = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + emailLogGuid;  
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

            // Survey template
            if (cms != null)
            {
                /// if case is closed and was no vote in survey - add HTML inormation about survey
                if (c.IsClosed() && (this.surveyService.GetByCaseId(c.Id) == null))
                {
                    var template = new SurveyTemplate()
                    {
                        VoteBadLink =
                            string.Format(
                                "{0}Survey/vote/{1}?voteId=bad",
                                cms.AbsoluterUrl,
                                c.Id),
                        VoteBadText = "Bad",
                        VoteNormalLink =
                            string.Format(
                                "{0}Survey/vote/{1}?voteId=normal",
                                cms.AbsoluterUrl,
                                c.Id),
                        VoteNormalText = "Normal",
                        VoteGoodLink =
                            string.Format(
                                "{0}Survey/vote/{1}?voteId=good",
                                cms.AbsoluterUrl,
                                c.Id),
                        VoteGoodText = "Good",
                    };
                    ret.Add(new Field { Key = "[#777]", StringValue = template.TransformText() });
                }
                else
                {
                    ret.Add(new Field { Key = "[#777]", StringValue = string.Empty });
                }
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
