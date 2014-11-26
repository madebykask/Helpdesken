namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure.Concrete;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Accounts;
    using DH.Helpdesk.Services.Requests.Account;

    using User = DH.Helpdesk.Domain.User;

    public class OrderAccountService : IOrderAccountService
    {
        private readonly UnitOfWorkFactory unitOfWorkFactory;

        public OrderAccountService(UnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public List<AccountOverview> GetOverviews(AccountFilter filter, OperationContext context)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var accountRepository = uof.GetRepository<Account>();

                IQueryable<EmploymentType> epmloimentTypes = uof.GetRepository<EmploymentType>().GetAll();
                IQueryable<AccountType> accountTypes = uof.GetRepository<AccountType>().GetAll();

                List<AccountOverview> overviews =
                    accountRepository.GetAll()
                        .GetActivityTypeAccounts(filter.ActivityTypeId)
                        .GetAdministratorAccounts(filter.AdministratorTypeId)
                        .GetAccountsBySearchString(filter.SearchString)
                        .GetStateAccounts(filter.AccountState)
                        .MapToAccountOverview(epmloimentTypes, accountTypes);

                return overviews;
            }
        }

        public AccountForEdit Get(int id)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var accountRepository = uof.GetRepository<Account>();
                IQueryable<User> users = uof.GetRepository<User>().GetAll();

                AccountForEdit dto = accountRepository.GetAll().ExtractAccountDto(users);

                return dto;
            }
        }

        public void Update(AccountForUpdate dto, OperationContext context)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var accountRepository = uof.GetRepository<Account>();

                var domainEntity = new Account();
                this.Map(domainEntity, dto);

                domainEntity.ChangedDate = context.DateAndTime;
                domainEntity.ChangedByUser_Id = context.UserId;

                domainEntity.Programs.Clear();
                AddPrograms(dto, domainEntity);

                accountRepository.Update(domainEntity);

                uof.Save();
            }
        }

        public void Add(AccountForInsert dto, OperationContext context)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var accountRepository = uof.GetRepository<Account>();

                var domainEntity = new Account();
                this.Map(domainEntity, dto);

                domainEntity.CreatedDate = context.DateAndTime;
                domainEntity.ChangedDate = context.DateAndTime;

                domainEntity.CreatedByUser_Id = context.UserId;

                AddPrograms(dto, domainEntity);
                accountRepository.Add(domainEntity);

                uof.Save();
            }
        }

        public void Delete(int id)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var accountRepository = uof.GetRepository<Account>();

                Account account = accountRepository.GetById(id);
                account.Programs.Clear();

                var accountEMailLogRepository = uof.GetRepository<AccountEMailLog>();
                accountEMailLogRepository.DeleteWhere(x => x.Account_Id == id);

                accountRepository.DeleteById(id);

                uof.Save();
            }
        }

        public void Map(Account domainEntity, AccountForWrite dto)
        {
            domainEntity.OrdererId = dto.Orderer.Id;
            domainEntity.OrdererFirstName = dto.Orderer.FirstName;
            domainEntity.OrdererLastName = dto.Orderer.LastName;
            domainEntity.OrdererPhone = dto.Orderer.Phone;
            domainEntity.OrdererEmail = dto.Orderer.Email;

            domainEntity.UserId = dto.User.Ids != null ? string.Join(";", dto.User.Ids) : string.Empty;
            domainEntity.UserFirstName = dto.User.FirstName;
            domainEntity.UserInitials = dto.User.Initials;
            domainEntity.UserLastName = dto.User.LastName;
            domainEntity.UserPersonalIdentityNumber = dto.User.PersonalIdentityNumber;
            domainEntity.UserPhone = dto.User.Phone;
            domainEntity.UserExtension = dto.User.Extension;
            domainEntity.UserEMail = dto.User.EMail;
            domainEntity.UserTitle = dto.User.Title;
            domainEntity.UserLocation = dto.User.Location;
            domainEntity.UserRoomNumber = dto.User.RoomNumber;
            domainEntity.UserPostalAddress = dto.User.PostalAddress;
            domainEntity.EmploymentType = dto.User.EmploymentType;
            domainEntity.Department_Id = dto.User.DepartmentId;
            domainEntity.OU_Id = dto.User.UnitId;
            domainEntity.Department_Id2 = dto.User.DepartmentId2;
            domainEntity.InfoUser = dto.User.Info;
            domainEntity.Responsibility = dto.User.Responsibility;
            domainEntity.Activity = dto.User.Activity;
            domainEntity.Manager = dto.User.Manager;
            domainEntity.ReferenceNumber = dto.User.ReferenceNumber;

            domainEntity.AccountStartDate = dto.AccountInformation.StartedDate;
            domainEntity.AccountEndDate = dto.AccountInformation.FinishDate;
            domainEntity.EMailType = (int)dto.AccountInformation.EMailTypeId;
            domainEntity.HomeDirectory = dto.AccountInformation.HomeDirectory.ToInt();
            domainEntity.Profile = dto.AccountInformation.Profile.ToInt();
            domainEntity.InventoryNumber = dto.AccountInformation.InventoryNumber;
            domainEntity.AccountType_Id = dto.AccountInformation.AccountTypeId;
            domainEntity.AccountType2 = dto.AccountInformation.AccountType2 != null
                                            ? string.Join(",", dto.AccountInformation.AccountType2)
                                            : string.Empty;
            domainEntity.AccountType3 = dto.AccountInformation.AccountType3;
            domainEntity.AccountType4 = dto.AccountInformation.AccountType4;
            domainEntity.AccountType5 = dto.AccountInformation.AccountType5;
            domainEntity.Info = dto.AccountInformation.Info;

            domainEntity.ContactId = dto.Contact.Ids != null ? string.Join(";", dto.Contact.Ids) : string.Empty;
            domainEntity.ContactName = dto.Contact.Name;
            domainEntity.ContactPhone = dto.Contact.Phone;
            domainEntity.ContactEMail = dto.Contact.Email;

            domainEntity.DeliveryName = dto.DeliveryInformation.Name;
            domainEntity.DeliveryPhone = dto.DeliveryInformation.Phone;
            domainEntity.DeliveryAddress = dto.DeliveryInformation.Address;
            domainEntity.DeliveryPostalAddress = dto.DeliveryInformation.PostalAddress;

            domainEntity.InfoProduct = dto.Program.InfoProduct;

            domainEntity.AccountFileName = dto.Other.FileName;
            domainEntity.AccountFileContentType = ""; // todo
            domainEntity.AccountFile = dto.Other.Content;
            domainEntity.InfoOther = dto.Other.Info;
        }

        private static void AddPrograms(AccountForWrite dto, Account domainEntity)
        {
            foreach (var id in dto.Program.ProgramIds)
            {
                domainEntity.Programs.Add(new Helpdesk.Domain.Program() { Id = id });
            }
        }
    }
}
