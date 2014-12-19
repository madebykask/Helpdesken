namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write;
    using DH.Helpdesk.Dal.Enums.Accounts;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Accounts;

    public class OrderAccountSettingsService : IOrderAccountSettingsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public OrderAccountSettingsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public HeadersFieldSettings GetHeadersFieldSettings(int accountActivityId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var accountActivityRepository = uow.GetRepository<AccountActivity>();

                return accountActivityRepository.GetAll().GetById(accountActivityId).ExtractHeadersFieldSettings();
            }
        }

        public void UpdateHeadersFieldSettings(int accountActivityId, HeadersFieldSettings dto)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var accountActivityRepository = uow.GetRepository<AccountActivity>();

                AccountActivity entiity = accountActivityRepository.GetById(accountActivityId);

                entiity.OrdererInfo = !string.IsNullOrEmpty(dto.OrderLabel) ? dto.OrderLabel : Headers.Orderer;
                entiity.UserInfo = !string.IsNullOrEmpty(dto.OrderLabel) ? dto.UserLabel : Headers.User;
                entiity.AccountInfo = !string.IsNullOrEmpty(dto.OrderLabel)
                                          ? dto.AccountLabel
                                          : Headers.AccountInformation;
                entiity.ContactInfo = !string.IsNullOrEmpty(dto.OrderLabel) ? dto.ContactLabel : Headers.Contact;
                entiity.DeliveryInfo = !string.IsNullOrEmpty(dto.OrderLabel)
                                           ? dto.DeliveryLabel
                                           : Headers.DeliveryInformation;
                entiity.ProgramInfo = !string.IsNullOrEmpty(dto.OrderLabel) ? dto.ProgramLabel : Headers.Program;

                uow.Save();
            }
        }

        public AccountFieldsSettingsForEdit GetFieldsSettingsForEdit(int accountActivityId, OperationContext context)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<AccountFieldSettings>();

                return
                    fieldSettingsRep.GetAll()
                        .GetByCustomer(context.CustomerId)
                        .GetActivityTypeSettings(accountActivityId)
                        .ExtractOrdersFieldSettingsForEdit();
            }
        }

        public AccountFieldsSettingsForModelEdit GetFieldsSettingsForModelEdit(
            int accountActivityId,
            OperationContext context)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<AccountFieldSettings>();

                return
                    fieldSettingsRep.GetAll()
                        .GetByCustomer(context.CustomerId)
                        .GetActivityTypeSettings(accountActivityId)
                        .ExtractOrdersFieldSettingsForModelEdit();
            }
        }

        public AccountFieldsSettingsOverview GetFieldsSettingsOverview(int accountActivityId, OperationContext context)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<AccountFieldSettings>();

                return
                    fieldSettingsRep.GetAll()
                        .GetByCustomer(context.CustomerId)
                        .GetActivityTypeSettings(accountActivityId)
                        .ExtractOrdersFieldSettingsOverview();
            }
        }

        public List<AccountFieldsSettingsOverviewWithActivity> GetFieldsSettingsOverviews(OperationContext context)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<AccountFieldSettings>();

                return fieldSettingsRep.GetAll().GetByCustomer(context.CustomerId).ExtractOrdersFieldSettingsOverviews();
            }
        }

        public AccountFieldsSettingsForProcessing GetFieldsSettingsForProcessing(
            int accountActivityId,
            OperationContext context)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<AccountFieldSettings>();

                return
                    fieldSettingsRep.GetAll()
                        .GetByCustomer(context.CustomerId)
                        .GetActivityTypeSettings(accountActivityId)
                        .ExtractOrdersFieldSettingsForProcessing();
            }
        }

        public void Update(AccountFieldsSettingsForUpdate dto, OperationContext context)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<AccountFieldSettings>();

                fieldSettingsRep.GetAll()
                    .GetByCustomer(context.CustomerId)
                    .GetActivityTypeSettings(dto.ActivityId)
                    .MapToDomainEntity(dto);

                uow.Save();
            }
        }
    }
}