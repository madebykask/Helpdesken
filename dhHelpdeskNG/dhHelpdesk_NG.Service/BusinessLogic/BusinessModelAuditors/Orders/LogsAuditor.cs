namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors.Orders
{
    using System.Linq;
    using System.Net.Mail;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.MailTools;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.Services;

    using EmailLog = DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields.EmailLog;
    using DH.Helpdesk.BusinessData.Models.Email;
    using Infrastructure;

    public sealed class LogsAuditor : IBusinessModelAuditor<UpdateOrderRequest, OrderAuditData>
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IMailTemplateServiceNew mailTemplateService;

        private readonly IEmailService emailService;

        private readonly IMailTemplateFormatter<UpdateOrderRequest> mailTemplateFormatter;

        private readonly IMailUniqueIdentifierProvider mailUniqueIdentifierProvider;

        private readonly ISettingService settingService;

        private readonly IEmailSendingSettingsProvider _emailSendingSettingsProvider;

        public LogsAuditor(
                IUnitOfWorkFactory unitOfWorkFactory, 
                IMailTemplateServiceNew mailTemplateService, 
                IEmailService emailService, 
                IMailTemplateFormatter<UpdateOrderRequest> mailTemplateFormatter, 
                IMailUniqueIdentifierProvider mailUniqueIdentifierProvider,
                ISettingService settingService,
                IEmailSendingSettingsProvider emailSendingSettingsProvider)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.mailTemplateService = mailTemplateService;
            this.emailService = emailService;
            this.mailTemplateFormatter = mailTemplateFormatter;
            this.mailUniqueIdentifierProvider = mailUniqueIdentifierProvider;
            this.settingService = settingService;
            _emailSendingSettingsProvider = emailSendingSettingsProvider;
        }

        public void Audit(UpdateOrderRequest businessModel, OrderAuditData optionalData)
        {
            if (businessModel.NewLogs == null || !businessModel.NewLogs.Any())
            {
                return;
            }

            using (var uow = this.unitOfWorkFactory.Create())
            {
                var customerRep = uow.GetRepository<Customer>();
                var orderEmailLogsRep = uow.GetRepository<OrderEMailLog>();
                var customerSettingsRep = uow.GetRepository<Setting>();

                var customerSetting = customerSettingsRep.GetById(businessModel.CustomerId);
                var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

                if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
                {
                    var info = _emailSendingSettingsProvider.GetSettings();
                    smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
                }
                
                foreach (var log in businessModel.NewLogs)
                {
                    if (!log.Emails.Any())
                    {
                        continue;
                    }

                    var template = this.mailTemplateService.GetTemplate(
                                                                (int)OrderEmailTemplate.Logs,
                                                                businessModel.CustomerId,
                                                                businessModel.LanguageId,
                                                                uow);
                    if (template == null)
                    {
                        continue;
                    }

                    var mail = this.mailTemplateFormatter.Format(
                                                                template,
                                                                businessModel,
                                                                businessModel.CustomerId,
                                                                businessModel.LanguageId);
                    if (mail == null)
                    {
                        continue;
                    }

                    var customerEmail = customerRep.GetAll()
                                .GetById(businessModel.CustomerId)
                                .Select(c => c.HelpdeskEmail)
                                .FirstOrDefault();
                    if (string.IsNullOrEmpty(customerEmail) || !EmailHelper.IsValid(customerEmail))
                    {
                        continue;
                    }

                    var from = new MailAddress(customerEmail);
                    var mailResponse = EmailResponse.GetEmptyEmailResponse();
                    var mailSetting = new EmailSettings(mailResponse, smtpInfo);

                    this.emailService.SendEmail(from, log.Emails, mail, mailSetting);

                    var mailUniqueIdentifier = this.mailUniqueIdentifierProvider.Provide(
                                                                businessModel.DateAndTime,
                                                                from);

                    var emailLog = EmailLog.CreateNew(
                                            businessModel.Order.Id,
                                            optionalData.HistoryId,
                                            log.Emails,
                                            (int)OrderEmailTemplate.Logs,
                                            mailUniqueIdentifier,
                                            businessModel.DateAndTime);

                    var emailLogEntity = OrderEmailLogMapper.MapToEntity(emailLog);
                    orderEmailLogsRep.Add(emailLogEntity);
                    uow.Save();

                    log.OrderEmailLogId = emailLogEntity.Id;
                }                
            }
        }
    }
}