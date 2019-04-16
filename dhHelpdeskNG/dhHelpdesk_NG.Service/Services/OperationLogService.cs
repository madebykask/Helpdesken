using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using DH.Helpdesk.BusinessData.Models.OperationLog.Output;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.OperationLogs;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.OperationLog;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Services.Infrastructure.Email;
    using DH.Helpdesk.BusinessData.OldComponents;
    using System.Configuration;
    using DH.Helpdesk.BusinessData.Models.Email;
    using DH.Helpdesk.Services.Infrastructure;

    public interface IOperationLogService
    {
        IList<OperationLog> GetOperationLogs(int customerId);
        OperationLog GetOperationLog(int id);
        IList<OperationLog> GetAllOpertionLogs();
        IList<OperationLogList> GetListForIndexPage();
        IList<OperationLogList> SearchAndGenerateOperationLog(int customerId, IOperationLogSearch SearchOperationLogs);
        void SaveOperationLog(OperationLog operationlog, int[] wgs, out IDictionary<string, string> errors);
        DeleteMessage DeleteOperationLog(int id);
        void Commit();
        int GetOperationLogId();

        void SendOperationLogEmail(OperationLog operationLogId, OperationLogList operationLogList, Customer customer);
        void SendOperationLogSMS(OperationLog operationLogId, string SMSRecipients, string txtSMS, Customer customer);
        /// <summary>
        /// The get operation log overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="forStartPage">
        /// The for Start Page.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<OperationLogOverview> GetOperationLogOverviews(int[] customers, int? count, bool forStartPage);

        IList<OperationLog> GetRssOperationLogs(int customerId);
        Rss20FeedFormatter CreateRssFeed(string url, string title, IList<OperationLog> logs);
        void SaveRssFeed(Rss20FeedFormatter rss, string path);
    }

    public class OperationLogService : IOperationLogService
    {
        private readonly IOperationLogRepository _operationLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkingGroupRepository _workingGroupRepository;

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private readonly IWorkContext _workContext;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IEmailService _emailService;
        private readonly IOperationLogEMailLogRepository _operationLogEmailLogRepository;
        private readonly ISettingService _settingService;
        private readonly IEmailSendingSettingsProvider _emailSendingSettingsProvider;

        public OperationLogService(
            IOperationLogRepository operationLogRepository,
            IUnitOfWork unitOfWork,
            IWorkingGroupRepository workingGroupRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            IMailTemplateService mailTemplateService,
            IEmailService emailService,
            IOperationLogEMailLogRepository operationLogEmailLogRepository,
            ISettingService settingService,
            IWorkContext workContext,
            IEmailSendingSettingsProvider emailSendingSettingsProvider)
        {
            this._operationLogRepository = operationLogRepository;
            this._unitOfWork = unitOfWork;
            this._workingGroupRepository = workingGroupRepository;
            this._unitOfWorkFactory = unitOfWorkFactory;
            this._workContext = workContext;
            this._mailTemplateService = mailTemplateService;
            this._emailService = emailService;
            this._operationLogEmailLogRepository = operationLogEmailLogRepository;
            this._settingService = settingService;
            _emailSendingSettingsProvider = emailSendingSettingsProvider;
        }

        public IList<OperationLog> GetOperationLogs(int customerId)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var operationLogRep = uow.GetRepository<OperationLog>();

                return operationLogRep.GetAll()
                        .RestrictByWorkingGroupsAndUsers(this._workContext)
                        .GetByCustomer(customerId)
                        .ToList();
            }
        }

        public IList<OperationLogList> SearchAndGenerateOperationLog(int customerId, IOperationLogSearch SearchOperationLogs)
        {
            int CID = customerId; // Current CustomerID
            if (SearchOperationLogs.CustomerId > 0)
                CID = SearchOperationLogs.CustomerId;

            var query = (from c in this.GetListForIndexPage().Where(x => x.Customer_Id == CID)
                         select c);

            if (!string.IsNullOrEmpty(SearchOperationLogs.Text_Filter))
                query = query.Where(x => x.OperationLogAction.ToLower().Contains(SearchOperationLogs.Text_Filter.ToLower())
                                      || x.OperationLogDescription.ToLower().Contains(SearchOperationLogs.Text_Filter.ToLower())
                                      || x.OperationLogCategoryName.ToLower().Contains(SearchOperationLogs.Text_Filter.ToLower())
                                      || x.OperationObjectName.ToLower().Contains(SearchOperationLogs.Text_Filter.ToLower())
                                   );

            if (SearchOperationLogs.OperationObject_Filter != null)
                query = query.Where(x => SearchOperationLogs.OperationObject_Filter.Contains(x.OperationObject_ID));

            if (SearchOperationLogs.OperationCategory_Filter != null)
                query = query.Where(x => SearchOperationLogs.OperationCategory_Filter.Contains(x.OperationCategoriy_ID));

            if (SearchOperationLogs.PeriodFrom != null)
                query = query.Where(x => x.CreatedDate >= SearchOperationLogs.PeriodFrom);

            if (SearchOperationLogs.PeriodTo != null)
                query = query.Where(x => x.CreatedDate <= SearchOperationLogs.PeriodTo);

            if (!string.IsNullOrEmpty(SearchOperationLogs.SortBy) && (SearchOperationLogs.SortBy != "undefined"))
            {
                if (SearchOperationLogs.Ascending)
                    query = query.OrderBy(x => x.GetType().GetProperty(SearchOperationLogs.SortBy).GetValue(x, null));
                else
                    query = query.OrderByDescending(x => x.GetType().GetProperty(SearchOperationLogs.SortBy).GetValue(x, null));
            }

            return query.ToList();


        }

        public OperationLog GetOperationLog(int id)
        {
            OperationLog ret;
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var operationLogRep = uow.GetRepository<OperationLog>();

                ret = operationLogRep.GetAll()
                        .RestrictByWorkingGroupsAndUsers(this._workContext)
                        .GetById(id)
                        .IncludePath(o => o.Us)
                        .IncludePath(o => o.WGs)
                        .SingleOrDefault();
            }
            return ret;
        }

        public int GetOperationLogId()
        {
            return this._operationLogRepository.GetOperationLogId();
        }

        public IList<OperationLog> GetAllOpertionLogs()
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var operationLogRep = uow.GetRepository<OperationLog>();

                return operationLogRep.GetAll()
                        .RestrictByWorkingGroupsAndUsers(this._workContext)
                        .ToList();
            }
        }

        public IList<OperationLog> GetRssOperationLogs(int customerId)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var operationLogRep = uow.GetRepository<OperationLog>();
                var currDate = DateTime.UtcNow;
                var untilDate = currDate.AddDays(-1);

                return operationLogRep.GetAll()
                    .GetByCustomer(customerId)
                    .Where(l => l.PublicInformation == 1 &&
                            (!l.ShowUntilDate.HasValue || l.ShowUntilDate.Value >= untilDate) &&
                            (!l.ShowDate.HasValue || l.ShowDate <= currDate))
                    .ToList();
            }
        }

        public DeleteMessage DeleteOperationLog(int id)
        {
            try
            {
                using (var uow = this._unitOfWorkFactory.Create())
                {
                    var rep = uow.GetRepository<OperationLog>();

                    var entity = rep.GetAll()
                                  .RestrictByWorkingGroupsAndUsers(this._workContext)
                                  .GetById(id)
                                  .SingleOrDefault();

                    if (entity == null)
                    {
                        return DeleteMessage.Error;
                    }

                    entity.Us.Clear();
                    entity.WGs.Clear();
                    entity.EmailLogs.Clear();

                    rep.DeleteById(id);

                    uow.Save();
                    return DeleteMessage.Success;
                }
            }
            catch
            {
                return DeleteMessage.UnExpectedError;
            }
        }


        public void SaveOperationLog(OperationLog operationlog, int[] wgs, out IDictionary<string, string> errors)
        {
            if (operationlog == null)
            {
                throw new ArgumentNullException("operationlog");
            }

            errors = new Dictionary<string, string>();
            if (errors.Any())
            {
                return;
            }

            using (var uow = this._unitOfWorkFactory.Create())
            {
                var operationLogRep = uow.GetRepository<OperationLog>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();

                OperationLog entity;
                if (operationlog.IsNew())
                {
                    entity = new OperationLog();
                    OperationLogMapper.MapToEntity(operationlog, entity);
                    //entity.CreatedDate = DateTime.UtcNow;
                    entity.ChangedDate = DateTime.UtcNow;
                    operationLogRep.Add(entity);
                }
                else
                {
                    entity = operationLogRep.GetById(operationlog.Id);
                    OperationLogMapper.MapToEntity(operationlog, entity);
                    entity.ChangedDate = DateTime.UtcNow;
                    operationLogRep.Update(entity);
                }

                entity.WGs.Clear();
                if (wgs != null)
                {
                    foreach (var wg in wgs)
                    {
                        var workingGroupEntity = workingGroupRep.GetById(wg);
                        entity.WGs.Add(workingGroupEntity);
                    }
                }

                uow.Save();
            }
        }

        public void SendOperationLogEmail(OperationLog operationLog, OperationLogList operationLogList, Customer customer)
        {
            var helpdeskMailFromAdress = customer.HelpdeskEmail;

            // get list of fields to replace [#1] tags in the subjcet and body texts
            var fields = GetFieldsForEmail(operationLogList);

            if (operationLog == null || operationLog.Id <= 0 || string.IsNullOrWhiteSpace(operationLogList.EmailRecepientsOperationLog))
            {
                return;
            }

            var template = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(
                                                operationLog.Customer_Id,
                                                operationLogList.Language_Id,
                                                (int)GlobalEnums.MailTemplates.OperationLog);
            if (template == null)
            {
                return;
            }

            var customerSetting = _settingService.GetCustomerSetting(customer.Id);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            if (!string.IsNullOrEmpty(template.Body) && !string.IsNullOrEmpty(template.Subject))
            {
                var to = operationLogList.EmailRecepientsOperationLog
                                    .Replace(" ", "")
                                    .Replace(Environment.NewLine, "|")
                                    .Split('|', ';', ',');

                foreach (var t in to)
                {
                    var curMail = t.Trim();
                    if (!string.IsNullOrWhiteSpace(t) && _emailService.IsValidEmail(t))
                    {
                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                        {
                            var el = new OperationLogEMailLog(operationLog.Id, string.Empty, t);
                            fields = GetFieldsForEmail(operationLogList);
                            var mailResponse = EmailResponse.GetEmptyEmailResponse();
                            var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);

                            var e_res =_emailService.SendEmail(
                                helpdeskMailFromAdress, 
                                el.Recipients, 
                                template.Subject,
                                template.Body, 
                                fields, 
                                mailSetting, 
                                null, 
                                false, 
                                null);

                            //el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                            var now = DateTime.Now;
                            el.CreatedDate = now;
                            //el.ChangedDate = now;
                            _operationLogEmailLogRepository.Add(el);
                            _operationLogEmailLogRepository.Commit();
                        }
                    }
                }
            }
        }

        public void SendOperationLogSMS(OperationLog operationLog, string SMSRecipients, string txtSMS, Customer customer)
        {
            var setting = this._settingService.GetCustomerSetting(customer.Id);
            var helpdeskMailFromAdress = customer.HelpdeskEmail;
            var smsEmailDomain = setting.SMSEMailDomain;
            var smsEMailDomainUserId = string.Empty;
            var smsEMailDomainUserName = string.Empty;
            var smsEMailDomainPassword = string.Empty;
            var smsSubject = string.Empty;

            if (setting.SMSEMailDomainUserId != null)
                smsEMailDomainUserId = setting.SMSEMailDomainUserId;
            else
                smsEMailDomainUserId = "";

            if (setting.SMSEMailDomainUserName != null)
                smsEMailDomainUserName = setting.SMSEMailDomainUserName;
            else
                smsEMailDomainUserName = "";

            if (setting.SMSEMailDomainPassword != null)
                smsEMailDomainPassword = setting.SMSEMailDomainPassword;
            else
                smsEMailDomainPassword = "";

            smsSubject = smsEMailDomainUserName;
            if (smsEMailDomainPassword != "")
                smsSubject = smsSubject + " " + smsEMailDomainPassword;

            var customerSetting = _settingService.GetCustomerSetting(customer.Id);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }

            if (smsEMailDomainUserId != "")
            {
                var to = SMSRecipients
                                   .Replace(" ", "")
                                   .Replace(Environment.NewLine, "|")
                                   .Replace("-", "")
                                   .Split('|', ';', ',');

                foreach (var t in to)
                {
                    var curMail = t.Trim();
                    if (!string.IsNullOrWhiteSpace(t) && this._emailService.IsValidEmail(t))
                    {
                        if (!string.IsNullOrWhiteSpace(curMail) && _emailService.IsValidEmail(curMail))
                        {
                            var el = new OperationLogEMailLog(operationLog.Id, txtSMS, t);
                            var mailResponse = EmailResponse.GetEmptyEmailResponse();
                            var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                            var e_res = _emailService.SendEmail(helpdeskMailFromAdress, el.Recipients, smsSubject, el.SMSText, null, mailSetting, null, false, null);

                            var now = DateTime.Now;
                            el.CreatedDate = now;
                            this._operationLogEmailLogRepository.Add(el);
                            this._operationLogEmailLogRepository.Commit();
                        }

                    }
                }
            }

        }

        private List<Field> GetFieldsForEmail(OperationLogList loglist)
        {
            List<Field> ret = new List<Field>();

            ret.Add(new Field { Key = "[#1]", StringValue = loglist.OperationObjectName });
            ret.Add(new Field { Key = "[#2]", StringValue = loglist.OperationLogCategoryName });
            ret.Add(new Field { Key = "[#3]", StringValue = loglist.OperationLogDescription });
            ret.Add(new Field { Key = "[#4]", StringValue = loglist.OperationLogAction });
            //ret.Add(new Field { Key = "[#5]", StringValue = loglist. });
            
            return ret;
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<OperationLogOverview> GetOperationLogOverviews(int[] customers, int? count, bool forStartPage)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var operationLogRepository = uow.GetRepository<OperationLog>();

                var query = operationLogRepository.GetAll();

                query = query
                       .GetFromDate()
                       .GetUntilDate();

                return query
                        .RestrictByWorkingGroupsAndUsers(this._workContext)
                        .GetForStartPage(customers, count, forStartPage)
                        .MapToOverviews();
            }
        }

        public IList<OperationLogList> GetListForIndexPage()
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var operationLogRep = uow.GetRepository<OperationLog>();
                var operationLogCategoryRep = uow.GetRepository<OperationLogCategory>();
                var operationObjectRep = uow.GetRepository<OperationObject>();
                var userRep = uow.GetRepository<User>();

                var query = from ol in operationLogRep.GetAll().RestrictByWorkingGroupsAndUsers(this._workContext)
                            join olc in operationLogCategoryRep.GetAll() on ol.OperationLogCategory_Id equals olc.Id into gj
                            from x in gj.DefaultIfEmpty()
                            join ob in operationObjectRep.GetAll() on ol.OperationObject_Id equals ob.Id
                            join u in userRep.GetAll() on ol.User_Id equals u.Id
                            group ol by new
                            {
                                ol.Id,
                                OLCName = x == null ? string.Empty : x.OLCName,
                                ob.Name,
                                u.UserID,
                                ol.LogText,
                                ol.LogAction,
                                ol.CreatedDate,
                                ol.Customer_Id,
                                OOI = ob.Id,
                                OLCID = x == null ? 0 : x.Id
                            }
                    into g
                            select new OperationLogList
                            {
                                OperationLogAction = g.Key.LogAction,
                                OperationLogAdmin = g.Key.UserID,
                                CreatedDate = g.Key.CreatedDate,
                                OperationLogCategoryName = g.Key.OLCName,
                                OperationLogDescription = g.Key.LogText,
                                OperationObjectName = g.Key.Name,
                                Id = g.Key.Id,
                                Customer_Id = g.Key.Customer_Id,
                                OperationObject_ID = g.Key.OOI,
                                OperationCategoriy_ID = g.Key.OLCID
                            };

                return query.OrderByDescending(x => x.CreatedDate).ToList();
            }
        }


        public Rss20FeedFormatter CreateRssFeed(string url, string title, IList<OperationLog> logs)
        {
            logs = logs ?? new List<OperationLog>();
            title = title ?? "";
            const string description = "Driftinformation";
            var feed =
                new SyndicationFeed($"{title} - {description}", description, new Uri(url))
                {
                    Language = "sv",
                    LastUpdatedTime = DateTimeOffset.Now
                };

            var items = logs.Select(log => new SyndicationItem()
            {
                Summary = new TextSyndicationContent(log.LogTextExternal),
                PublishDate = log.ChangedDate
            })
                .ToList();

            feed.Items = items;

            return new Rss20FeedFormatter(feed);
        }

        public void SaveRssFeed(Rss20FeedFormatter rss, string path)
        {
            path = path + "/rss";
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory {path} does not exist");

            using (var rssWriter = XmlWriter.Create($"{path}/OperationLog.xml"))
            {
                rss.WriteTo(rssWriter);
                rssWriter.Close();
            }
        }
    }
}
