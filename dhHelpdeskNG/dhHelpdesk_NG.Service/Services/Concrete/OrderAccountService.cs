namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.NewInfrastructure.Concrete;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Accounts;
    using DH.Helpdesk.Services.Requests.Account;

    using Program = DH.Helpdesk.Domain.Program;
    using User = DH.Helpdesk.Domain.User;

    public class OrderAccountService : IOrderAccountService
    {
        private readonly UnitOfWorkFactory unitOfWorkFactory;

        public OrderAccountService(
            UnitOfWorkFactory unitOfWorkFactory)
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

                AccountForEdit dto = accountRepository.GetAll(x => x.Programs).GetById(id).ExtractAccountDto(users);

                return dto;
            }
        }

        public void Update(AccountForUpdate dto, OperationContext context)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                IRepository<Account> accountRepository = uof.GetRepository<Account>();
                IRepository<Program> programRepository = uof.GetRepository<Program>();

                var domainEntity = accountRepository.Find(x => x.Id == dto.Id, x => x.Programs).Single();
                this.Map(domainEntity, dto);

                domainEntity.ChangedDate = context.DateAndTime;
                domainEntity.ChangedByUser_Id = context.UserId;

                foreach (var program in domainEntity.Programs.ToList())
                {
                    if (!dto.Program.ProgramIds.Contains(program.Id))
                    {
                        domainEntity.Programs.Remove(program);
                    }
                }

                foreach (var newId in dto.Program.ProgramIds)
                {
                    if (!domainEntity.Programs.Any(r => r.Id == newId))
                    {
                        var program = new Program { Id = newId };

                        programRepository.Attach(program);
                        domainEntity.Programs.Add(program);
                    }
                }

                uof.Save();
            }
        }

        public int Add(AccountForInsert dto, OperationContext context)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                IRepository<Account> accountRepository = uof.GetRepository<Account>();
                IRepository<Program> programRepository = uof.GetRepository<Program>();

                var domainEntity = new Account();
                this.Map(domainEntity, dto);

                domainEntity.AccountActivity_Id = dto.ActivityId;
                domainEntity.Customer_Id = context.CustomerId;
                domainEntity.CreatedDate = context.DateAndTime;
                domainEntity.ChangedDate = context.DateAndTime;

                domainEntity.CreatedByUser_Id = context.UserId;

                if (dto.Program.ProgramIds != null && dto.Program.ProgramIds.Any())
                {
                    foreach (var id in dto.Program.ProgramIds)
                    {
                        var program = new Program { Id = id };

                        programRepository.Attach(program);
                        domainEntity.Programs.Add(program);
                    }
                }

                accountRepository.Add(domainEntity);

                uof.Save();

                return domainEntity.Id;
            }
        }

        public void Delete(int id)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                IRepository<Account> accountRepository = uof.GetRepository<Account>();

                Account account = accountRepository.GetById(id);
                account.Programs.Clear();

                var accountEMailLogRepository = uof.GetRepository<AccountEMailLog>();
                accountEMailLogRepository.DeleteWhere(x => x.Account_Id == id);

                accountRepository.DeleteById(id);

                uof.Save();
            }
        }

        public List<ItemOverview> GetAccountActivivties()
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var accountActivityRepository = uow.GetRepository<AccountActivity>();

                List<ItemOverview> overviews = accountActivityRepository.GetAll().MapAccountActivitiesToItemOverview();
                return overviews;
            }
        }

        public IdAndNameOverview GetAccountActivityItemOverview(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var accountActivityRepository = uow.GetRepository<AccountActivity>();

                IdAndNameOverview overview = accountActivityRepository.GetAll().ExtractIdAndNameOverview(id);
                return overview;
            }
        }

        public List<int> GetAccountActivivtieIds()
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var accountActivityRepository = uow.GetRepository<AccountActivity>();

                List<int> overviews = accountActivityRepository.GetAll().Select(x => x.Id).ToList();
                return overviews;
            }
        }

        public List<ItemOverview> GetEmploymentTypes()
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var employmentTypeRepository = uow.GetRepository<EmploymentType>();

                List<ItemOverview> overviews = employmentTypeRepository.GetAll().MapEmploymentTypesToItemOverview();
                return overviews;
            }
        }

        public List<AccountTypeOverview> GetAccountTypes(int activityTypeId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var accountTypeRepository = uow.GetRepository<AccountType>();

                List<AccountTypeOverview> overviews =
                    accountTypeRepository.GetAll().MapAccountTypesToItemOverview(activityTypeId);
                return overviews;
            }
        }

        public List<ItemOverview> GetAccountPrograms()
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var accountTypeRepository = uow.GetRepository<Program>();

                List<ItemOverview> overviews =
                    accountTypeRepository.GetAll().MapProgramsToItemOverview();
                return overviews;
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
            domainEntity.UserPersonalIdentityNumber = dto.User.PersonalIdentityNumber != null ? string.Join(";", dto.User.PersonalIdentityNumber) : string.Empty;
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
            domainEntity.EMailType = dto.AccountInformation.EMailTypeId;
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
    }
}
