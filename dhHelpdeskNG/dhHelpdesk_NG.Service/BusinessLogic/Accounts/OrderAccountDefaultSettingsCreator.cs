namespace DH.Helpdesk.Services.BusinessLogic.Accounts
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Enums.Accounts.Fields;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Accounts;

    public interface IOrderAccountDefaultSettingsCreator
    {
        void Create(int activityTypeId, OperationContext context);
    }

    public class OrderAccountDefaultSettingsCreator : IOrderAccountDefaultSettingsCreator
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public OrderAccountDefaultSettingsCreator(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Create(int activityTypeId, OperationContext context)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.Create())
            {
                IRepository<AccountFieldSettings> settingsRepository = uow.GetRepository<AccountFieldSettings>();

                IQueryable<AccountFieldSettings> settings = settingsRepository.GetAll();
                List<string> settingNames =
                    settings.GetByCustomer(context.CustomerId)
                        .GetActivityTypeSettings(activityTypeId)
                        .Select(x => x.AccountField)
                        .ToList();

                var missing = new List<string>();

                CreateMissingOrdererFieldSettings(settingNames, missing);
                CreateMissingUserFieldSettings(settingNames, missing);
                CreateMissingAccountInformationFieldSettings(settingNames, missing);
                CreateMissingProgramFieldSettings(settingNames, missing);
                CreateMissingOtherFieldSettings(settingNames, missing);
                CreateMissingDeliveryInformationFieldSettings(settingNames, missing);
                CreateMissingContactFieldSettings(settingNames, missing);

                foreach (var fieldName in missing)
                {
                    var entity = CreateDefaultSetting(activityTypeId, fieldName, context);
                    settingsRepository.Add(entity);
                }

                uow.Save();
            }
        }

        private static void CreateMissingContactFieldSettings(
            List<string> settingNames,
            List<string> missing)
        {
            CollectMissingField(ContactFields.Id, settingNames, missing);
            CollectMissingField(ContactFields.Name, settingNames, missing);
            CollectMissingField(ContactFields.Phone, settingNames, missing);
            CollectMissingField(ContactFields.Email, settingNames, missing);
        }

        private static void CreateMissingDeliveryInformationFieldSettings(
            List<string> settingNames,
            List<string> missing)
        {
            CollectMissingField(DeliveryInformationFields.Name, settingNames, missing);
            CollectMissingField(DeliveryInformationFields.Phone, settingNames, missing);
            CollectMissingField(DeliveryInformationFields.Address, settingNames, missing);
            CollectMissingField(DeliveryInformationFields.PostalAddress, settingNames, missing);
        }

        private static void CreateMissingOtherFieldSettings(
            List<string> settingNames,
            List<string> missing)
        {
            CollectMissingField(OtherFields.CaseNumber, settingNames, missing);
            CollectMissingField(OtherFields.FileName, settingNames, missing);
            CollectMissingField(OtherFields.Info, settingNames, missing);
        }

        private static void CreateMissingProgramFieldSettings(
            List<string> settingNames,
            List<string> missing)
        {
            CollectMissingField(ProgramFields.Programs, settingNames, missing);
            CollectMissingField(ProgramFields.InfoProduct, settingNames, missing);
        }

        private static void CreateMissingAccountInformationFieldSettings(
            List<string> settingNames,
            List<string> missing)
        {
            CollectMissingField(AccountInformationFields.StartedDate, settingNames, missing);
            CollectMissingField(AccountInformationFields.FinishDate, settingNames, missing);
            CollectMissingField(AccountInformationFields.EMailTypeId, settingNames, missing);
            CollectMissingField(AccountInformationFields.HomeDirectory, settingNames, missing);
            CollectMissingField(AccountInformationFields.Profile, settingNames, missing);
            CollectMissingField(AccountInformationFields.InventoryNumber, settingNames, missing);
            CollectMissingField(AccountInformationFields.AccountTypeId, settingNames, missing);
            CollectMissingField(AccountInformationFields.AccountType2, settingNames, missing);
            CollectMissingField(AccountInformationFields.AccountType3, settingNames, missing);
            CollectMissingField(AccountInformationFields.AccountType4, settingNames, missing);
            CollectMissingField(AccountInformationFields.AccountType5, settingNames, missing);
            CollectMissingField(AccountInformationFields.Info, settingNames, missing);
        }

        private static void CreateMissingUserFieldSettings(
            List<string> settingNames,
            List<string> missing)
        {
            CollectMissingField(UserFields.Ids, settingNames, missing);
            CollectMissingField(UserFields.FirstName, settingNames, missing);
            CollectMissingField(UserFields.Initials, settingNames, missing);
            CollectMissingField(UserFields.LastName, settingNames, missing);
            CollectMissingField(UserFields.PersonalIdentityNumber, settingNames, missing);
            CollectMissingField(UserFields.Phone, settingNames, missing);
            CollectMissingField(UserFields.Extension, settingNames, missing);
            CollectMissingField(UserFields.EMail, settingNames, missing);
            CollectMissingField(UserFields.Title, settingNames, missing);
            CollectMissingField(UserFields.Location, settingNames, missing);
            CollectMissingField(UserFields.RoomNumber, settingNames, missing);
            CollectMissingField(UserFields.PostalAddress, settingNames, missing);
            CollectMissingField(UserFields.EmploymentType, settingNames, missing);
            CollectMissingField(UserFields.DepartmentId, settingNames, missing);
            CollectMissingField(UserFields.UnitId, settingNames, missing);
            CollectMissingField(UserFields.DepartmentId2, settingNames, missing);
            CollectMissingField(UserFields.Info, settingNames, missing);
            CollectMissingField(UserFields.Responsibility, settingNames, missing);
            CollectMissingField(UserFields.Activity, settingNames, missing);
            CollectMissingField(UserFields.Manager, settingNames, missing);
            CollectMissingField(UserFields.ReferenceNumber, settingNames, missing);
        }

        private static void CreateMissingOrdererFieldSettings(
            List<string> settingNames,
            List<string> missing)
        {
            CollectMissingField(OrdererFields.Id, settingNames, missing);
            CollectMissingField(OrdererFields.FirstName, settingNames, missing);
            CollectMissingField(OrdererFields.LastName, settingNames, missing);
            CollectMissingField(OrdererFields.Phone, settingNames, missing);
            CollectMissingField(OrdererFields.Email, settingNames, missing);
        }

        private static AccountFieldSettings CreateDefaultSetting(
            int activityTypeId,
            string fieldName,
            OperationContext context)
        {
            return new AccountFieldSettings
                       {
                           AccountField = fieldName,
                           CreatedDate = context.DateAndTime,
                           ChangedDate = context.DateAndTime, // todo
                           AccountActivity_Id = activityTypeId,
                           Customer_Id = context.CustomerId,
                           Label = fieldName,
                           Required = 0,
                           Show = 0,
                           ShowExternal = 0,
                           ShowInList = 0,
                           FieldHelp = string.Empty,
                           MultiValue = 0,
                       };
        }

        private static void CollectMissingField(string fieldName, List<string> existing, List<string> missingFields)
        {
            if (existing.All(f => f.ToLower() != fieldName.ToLower()))
            {
                missingFields.Add(fieldName);
            }
        }
    }
}
