namespace dhHelpdesk_NG.Data.Repositories.Notifiers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Notifiers;

    public sealed class NotifiersRepository : RepositoryBase<ComputerUser>, INotifiersRepository
    {
        public NotifiersRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public DisplayNotifierDto FindById(int notifierId)
        {
            var notifierEntity = this.DataContext.ComputerUsers.Find(notifierId);

            return new DisplayNotifierDto
            {
                PostalAddress = notifierEntity.PostalAddress,
                CellPhone = notifierEntity.Cellphone,
                ChangedDate = notifierEntity.ChangeTime,
                City = notifierEntity.City,
                Code = notifierEntity.UserCode,
                CreatedDate = notifierEntity.RegTime,
                DepartmentId = notifierEntity.Department_Id,
                DisplayName = notifierEntity.DisplayName,
                DivisionId = notifierEntity.Division_Id,
                DomainId = notifierEntity.Domain_Id,
                Email = notifierEntity.Email,
                FirstName = notifierEntity.FirstName,
                GroupId = notifierEntity.ComputerUserGroup_Id,
                Id = notifierEntity.Id,
                Initials = notifierEntity.Initials,
                IsActive = notifierEntity.Status != 0,
                LastName = notifierEntity.SurName,
                LoginName = notifierEntity.LogonName,
                ManagerId = notifierEntity.ManagerComputerUser_Id,
                Ordered = notifierEntity.OrderPermission != 0,
                OrganizationUnitId = notifierEntity.OU == null ? (int?)null : notifierEntity.OU.Id,
                Other = notifierEntity.Info,
                Password = notifierEntity.Password,
                Phone = notifierEntity.Phone,
                Place = notifierEntity.Location,
                PostalCode = notifierEntity.Postalcode,
                SynchronizationDate = notifierEntity.SyncChangedDate,
                Title = notifierEntity.Title,
                Unit = notifierEntity.SOU,
                UserId = notifierEntity.UserId
            };
        }

        public void AddNotifier(NewNotifierDto notifier)
        {
            var notifierEntity = new ComputerUser
            {
                Cellphone = notifier.CellPhone ?? string.Empty,
                City = notifier.City ?? string.Empty,
                ComputerUserGroup_Id = notifier.GroupId,
                Customer_Id = notifier.CustomerId,
                Department_Id = notifier.DepartmentId,
                DisplayName = notifier.DisplayName ?? string.Empty,
                Division_Id = notifier.DivisionId,
                Domain_Id = notifier.DomainId,
                Email = notifier.Email ?? string.Empty,
                FirstName = notifier.FirstName ?? string.Empty,
                FullName = string.Empty,
                Info = notifier.Other ?? string.Empty,
                Initials = notifier.Initials ?? string.Empty,
                Location = notifier.Place ?? string.Empty,
                LogonName = notifier.LoginName ?? string.Empty,
                ManagerComputerUser_Id = notifier.ManagerId,
                NDSpath = string.Empty,
                OU_Id = notifier.OrganizationUnitId,
                OrderPermission = notifier.Ordered ? 1 : 0,
                Password = notifier.Password ?? string.Empty,
                Phone = notifier.Phone ?? string.Empty,
                Phone2 = string.Empty,
                PostalAddress = notifier.PostalAddress ?? string.Empty,
                Postalcode = notifier.PostalCode ?? string.Empty,
                RegTime = notifier.CreatedDate,
                SOU = notifier.Unit ?? string.Empty,
                Status = notifier.IsActive ? 1 : 0,
                SurName = notifier.LastName ?? string.Empty,
                Title = notifier.Title ?? string.Empty,
                UserCode = notifier.Code ?? string.Empty,
                UserGUID = string.Empty,
                UserId = notifier.UserId ?? string.Empty,
                homeDirectory = string.Empty,
                homeDrive = string.Empty
            };

            this.DataContext.ComputerUsers.Add(notifierEntity);
            this.InitializeAfterCommit(notifier, notifierEntity);
        }

        public void UpdateNotifier(UpdatedNotifierDto notifier)
        {
            var notifierEntity = this.DataContext.ComputerUsers.Find(notifier.Id);

            notifierEntity.Cellphone = notifier.CellPhone ?? string.Empty;
            notifierEntity.ChangeTime = notifier.ChangedDate;
            notifierEntity.City = notifier.City ?? string.Empty;
            notifierEntity.UserCode = notifier.Code ?? string.Empty;
            notifierEntity.Department_Id = notifier.DepartmentId;
            notifierEntity.DisplayName = notifier.DisplayName ?? string.Empty;
            notifierEntity.Division_Id = notifier.DivisionId;
            notifierEntity.Domain_Id = notifier.DomainId;
            notifierEntity.Email = notifier.Email ?? string.Empty;
            notifierEntity.FirstName = notifier.FirstName ?? string.Empty;
            notifierEntity.ComputerUserGroup_Id = notifier.GroupId;
            notifierEntity.Initials = notifier.Initials ?? string.Empty;
            notifierEntity.Status = notifier.IsActive ? 1 : 0;
            notifierEntity.SurName = notifier.LastName ?? string.Empty;
            notifierEntity.LogonName = notifier.LoginName ?? string.Empty;
            notifierEntity.ManagerComputerUser_Id = notifier.ManagerId;
            notifierEntity.OrderPermission = notifier.Ordered ? 1 : 0;
            notifierEntity.OU_Id = notifier.OrganizationUnitId;
            notifierEntity.Info = notifier.Other ?? string.Empty;
            notifierEntity.Password = notifier.Password ?? string.Empty;
            notifierEntity.Phone = notifier.Phone ?? string.Empty;
            notifierEntity.Location = notifier.Place ?? string.Empty;
            notifierEntity.PostalAddress = notifier.PostalAddress ?? string.Empty;
            notifierEntity.Postalcode = notifier.PostalCode ?? string.Empty;
            notifierEntity.Title = notifier.Title ?? string.Empty;
            notifierEntity.SOU = notifier.Unit ?? string.Empty;
        }

        private IQueryable<ComputerUser> FindByCustomerId(int customerId)
        {
            return this.DataContext.ComputerUsers.Where(u => u.Customer_Id == customerId);
        } 

        public List<NotifierDetailedOverviewDto> FindDetailedOverviewsByCustomerId(int customerId)
        {
            return
                this.FindByCustomerId(customerId)
                    .Select(
                        n =>
                        new NotifierDetailedOverviewDto
                            {
                                PostalAddress = n.PostalAddress,
                                CellPhone = n.Cellphone,
                                ChangedDate = n.ChangeTime,
                                City = n.City,
                                Code = n.UserCode,
                                CreatedDate = n.RegTime,
                                Department = n.Department.DepartmentName,
                                Division = n.Division.Name,
                                Domain = n.Domain.Name,
                                Email = n.Email,
                                FirstName = n.FirstName,
                                Group = n.ComputerUserGroup.Name,
                                Id = n.Id,
                                Initials = n.Initials,
                                LastName = n.SurName,
                                LoginName = n.LogonName,
                                Manager = n.ManagerComputerUser.UserId,
                                DisplayName = n.DisplayName,
                                Ordered = n.OrderPermission != 0,
                                OrganizationUnit = n.OU.Name,
                                Other = n.Info,
                                Password = n.Password,
                                Phone = n.Phone,
                                Place = n.Location,
                                PostalCode = n.Postalcode,
                                SynchronizationDate = n.SyncChangedDate,
                                Title = n.Title,
                                Unit = n.SOU,
                                UserId = n.UserId
                            })
                    .ToList();
        }

        public List<ItemOverviewDto> FindOverviewsByCustomerId(int customerId)
        {
            var notifierOverviews =
                this.FindByCustomerId(customerId).Select(n => new { Id = n.Id, UserId = n.UserId }).ToList();

            return
                notifierOverviews.Select(
                    n => new ItemOverviewDto { Name = n.UserId, Value = n.Id.ToString(CultureInfo.InvariantCulture) })
                                 .ToList();
        }

        public IList<UserSearchResults> Search(int customerId, string searchFor)
        {
            var s = searchFor.ToLower();

            var query =
                from cu in DataContext.ComputerUsers
                join d in DataContext.Departments on cu.Department_Id equals d.Id into res
                from k in res.DefaultIfEmpty()
                where cu.Customer_Id == customerId
                      && (
                          cu.UserId.ToLower().Contains(s)
                          || cu.FirstName.ToLower().Contains(s)
                          || cu.SurName.ToLower().Contains(s)
                          || cu.Phone.ToLower().Contains(s)
                          || cu.Location.ToLower().Contains(s)
                          || cu.Cellphone.ToLower().Contains(s)
                          || cu.Email.ToLower().Contains(s)
                          || cu.UserCode.ToLower().Contains(s)
                          )
                select new UserSearchResults
                {
                    CellPhone = cu.Cellphone,
                    Email = cu.Email,
                    FirstName = cu.FirstName,
                    Id = cu.Id,
                    Location = cu.Location,
                    Phone = cu.Phone,
                    SurName = cu.SurName,
                    Department_Id = cu.Department_Id,
                    OU_Id = cu.OU_Id,
                    Region_Id = k.Region_Id,
                    UserCode = cu.UserCode,
                    UserId = cu.UserId
                };

            return query.OrderBy(x => x.FirstName).ThenBy(x => x.SurName).ToList();
        }

        public List<NotifierDetailedOverviewDto> SearchDetailedOverviews(
            int customerId,
            int? domainId,
            int? departmentId,
            int? divisionId,
            string pharse,
            EntityStatus status,
            int selectCount)
        {
            var searchRequest = this.FindByCustomerId(customerId);

            if (domainId.HasValue)
            {
                searchRequest = searchRequest.Where(n => n.Domain_Id == domainId);
            }

            if (departmentId.HasValue)
            {
                searchRequest = searchRequest.Where(n => n.Department_Id == departmentId);
            }

            if (divisionId.HasValue)
            {
                searchRequest = searchRequest.Where(n => n.Division_Id == divisionId);
            }

            if (!string.IsNullOrEmpty(pharse))
            {
                var pharseInLowerCase = pharse.ToLower();

                searchRequest =
                    searchRequest.Where(
                        n =>
                        n.Cellphone.ToLower().Contains(pharseInLowerCase)
                        || n.City.ToLower().Contains(pharseInLowerCase)
                        || n.ComputerUserGroup.Name.ToLower().Contains(pharseInLowerCase)
                        || n.Department.DepartmentName.ToLower().Contains(pharseInLowerCase)
                        || n.DisplayName.ToLower().Contains(pharseInLowerCase)
                        || n.Division.Name.ToLower().Contains(pharseInLowerCase)
                        || n.Domain.Name.ToLower().Contains(pharseInLowerCase)
                        || n.Email.ToLower().Contains(pharseInLowerCase)
                        || n.FirstName.ToLower().Contains(pharseInLowerCase)
                        || n.FullName.ToLower().Contains(pharseInLowerCase)
                        || n.Info.ToLower().Contains(pharseInLowerCase)
                        || n.Initials.ToLower().Contains(pharseInLowerCase)
                        || n.Location.ToLower().Contains(pharseInLowerCase)
                        || n.LogonName.ToLower().Contains(pharseInLowerCase)
                        || n.ManagerComputerUser.UserId.ToLower().Contains(pharseInLowerCase)
                        || n.OU.Name.ToLower().Contains(pharseInLowerCase)
                        || n.Phone.ToLower().Contains(pharseInLowerCase)
                        || n.PostalAddress.ToLower().Contains(pharseInLowerCase)
                        || n.Postalcode.ToLower().Contains(pharseInLowerCase)
                        || n.SOU.ToLower().Contains(pharseInLowerCase)
                        || n.SurName.ToLower().Contains(pharseInLowerCase)
                        || n.Title.ToLower().Contains(pharseInLowerCase)
                        || n.UserCode.ToLower().Contains(pharseInLowerCase)
                        || n.UserId.ToLower().Contains(pharseInLowerCase));
            }

            if (status == EntityStatus.Active)
            {
                searchRequest = searchRequest.Where(u => u.Status != 0);
            }
            else if (status == EntityStatus.Inactive)
            {
                searchRequest = searchRequest.Where(u => u.Status == 0);
            }

            searchRequest = searchRequest.Take(selectCount);

            return
                searchRequest.Select(
                    n =>
                    new NotifierDetailedOverviewDto
                        {
                            PostalAddress = n.PostalAddress,
                            CellPhone = n.Cellphone,
                            ChangedDate = n.ChangeTime,
                            City = n.City,
                            Code = n.UserCode,
                            CreatedDate = n.RegTime,
                            Department = n.Department.DepartmentName,
                            Division = n.Division.Name,
                            Domain = n.Domain.Name,
                            Email = n.Email,
                            FirstName = n.FirstName,
                            Group = n.ComputerUserGroup.Name,
                            Id = n.Id,
                            Initials = n.Initials,
                            LastName = n.SurName,
                            LoginName = n.LogonName,
                            Manager = n.ManagerComputerUser.UserId,
                            DisplayName = n.DisplayName,
                            Ordered = n.OrderPermission != 0,
                            OrganizationUnit = n.OU.Name,
                            Other = n.Info,
                            Password = n.Password,
                            Phone = n.Phone,
                            Place = n.Location,
                            PostalCode = n.Postalcode,
                            SynchronizationDate = n.SyncChangedDate,
                            Title = n.Title,
                            Unit = n.SOU,
                            UserId = n.UserId
                        }).ToList();
        }
    }
}
