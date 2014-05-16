﻿namespace DH.Helpdesk.Dal.Repositories.Notifiers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.SearchRequestBuilders.Notifiers;
    using DH.Helpdesk.Domain.Computers;

    public sealed class NotifierRepository : RepositoryBase<ComputerUser>, INotifierRepository
    {
        private readonly IBusinessModelToEntityMapper<CaseNotifier, ComputerUser> caseNotifierToEntityMapper;

        #region Constructors and Destructors

        public NotifierRepository(
            IDatabaseFactory databaseFactory, 
            IBusinessModelToEntityMapper<CaseNotifier, ComputerUser> caseNotifierToEntityMapper)
            : base(databaseFactory)
        {
            this.caseNotifierToEntityMapper = caseNotifierToEntityMapper;
        }

        #endregion

        #region Public Methods and Operators

        public void AddNotifier(Notifier notifier)
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
                                         Password = string.Empty,
                                         Phone = notifier.Phone ?? string.Empty, 
                                         Phone2 = string.Empty, 
                                         PostalAddress = notifier.PostalAddress ?? string.Empty, 
                                         Postalcode = notifier.PostalCode ?? string.Empty, 
                                         RegTime = notifier.CreatedDateAndTime, 
                                         SOU = notifier.Unit ?? string.Empty, 
                                         Status = notifier.IsActive ? 1 : 0, 
                                         SurName = notifier.LastName ?? string.Empty, 
                                         Title = notifier.Title ?? string.Empty, 
                                         UserCode = notifier.Code ?? string.Empty, 
                                         UserGUID = string.Empty, 
                                         UserId = notifier.UserId ?? string.Empty, 
                                         homeDirectory = string.Empty, 
                                         homeDrive = string.Empty,
                                         ChangeTime = DateTime.Now
                                     };
            
            this.DataContext.ComputerUsers.Add(notifierEntity);
            this.InitializeAfterCommit(notifier, notifierEntity);
        }

        public void DeleteById(int notifierId)
        {
            var notifier = this.DataContext.ComputerUsers.Find(notifierId);
            this.DataContext.ComputerUsers.Remove(notifier);
        }

        public List<NotifierDetailedOverview> FindDetailedOverviewsByCustomerIdOrderedByUserIdAndFirstNameAndLastName
            (int customerId)
        {
            var notifiers =
                this.FindByCustomerIdCore(customerId)
                    .OrderBy(n => n.UserId)
                    .ThenBy(n => n.FirstName)
                    .ThenBy(n => n.SurName);

            var overviews =
                notifiers.Select(
                    n =>
                    new
                        {
                            PostalAddress = n.PostalAddress != string.Empty ? n.PostalAddress : null, 
                            CellPhone = n.Cellphone != string.Empty ? n.Cellphone : null, 
                            ChangedDate = n.ChangeTime, 
                            City = n.City != string.Empty ? n.City : null, 
                            Code = n.UserCode != string.Empty ? n.UserCode : null, 
                            CreatedDate = n.RegTime, 
                            Department = n.Department.DepartmentName, 
                            Division = n.Division.Name, 
                            Domain = n.Domain.Name, 
                            Email = n.Email != string.Empty ? n.Email : null, 
                            FirstName = n.FirstName != string.Empty ? n.FirstName : null, 
                            Group = n.ComputerUserGroup.Name != string.Empty ? n.ComputerUserGroup.Name : null, 
                            Id = n.Id, 
                            Initials = n.Initials != string.Empty ? n.Initials : null, 
                            LastName = n.SurName != string.Empty ? n.SurName : null, 
                            LoginName = n.LogonName != string.Empty ? n.LogonName : null, 
                            Manager = n.ManagerComputerUser.UserId, 
                            DisplayName = n.DisplayName != string.Empty ? n.DisplayName : null, 
                            Ordered = n.OrderPermission != 0, 
                            OrganizationUnit = n.OU.Name, 
                            Other = n.Info != string.Empty ? n.Info : null, 
                            Phone = n.Phone != string.Empty ? n.Phone : null, 
                            Place = n.Location != string.Empty ? n.Location : null, 
                            PostalCode = n.Postalcode != string.Empty ? n.Postalcode : null, 
                            SynchronizationDate = n.SyncChangedDate, 
                            Title = n.Title != string.Empty ? n.Title : null, 
                            Unit = n.SOU != string.Empty ? n.SOU : null, 
                            UserId = n.UserId
                        }).ToList();

            return
                overviews.Select(
                    o =>
                    new NotifierDetailedOverview(
                        o.Id, 
                        o.UserId, 
                        o.Domain, 
                        o.LoginName, 
                        o.FirstName, 
                        o.Initials, 
                        o.LastName, 
                        o.DisplayName, 
                        o.Place, 
                        o.Phone, 
                        o.CellPhone, 
                        o.Email, 
                        o.Code, 
                        o.PostalAddress, 
                        o.PostalCode, 
                        o.City, 
                        o.Title, 
                        o.Department, 
                        o.Unit, 
                        o.OrganizationUnit, 
                        o.Division, 
                        o.Manager, 
                        o.Group, 
                        o.Other, 
                        o.Ordered, 
                        o.CreatedDate, 
                        o.ChangedDate, 
                        o.SynchronizationDate)).ToList();
        }

        public Notifier FindExistingNotifierById(int notifierId)
        {
            var notifierEntity = this.DataContext.ComputerUsers.Find(notifierId);

            return Notifier.CreateForEdit(
                notifierEntity.UserId != string.Empty ? notifierEntity.UserId : null,
                notifierEntity.Domain_Id,
                notifierEntity.LogonName != string.Empty ? notifierEntity.LogonName : null,
                notifierEntity.FirstName != string.Empty ? notifierEntity.FirstName : null,
                notifierEntity.Initials != string.Empty ? notifierEntity.Initials : null,
                notifierEntity.SurName != string.Empty ? notifierEntity.SurName : null,
                notifierEntity.DisplayName != string.Empty ? notifierEntity.DisplayName : null,
                notifierEntity.Location != string.Empty ? notifierEntity.Location : null,
                notifierEntity.Phone != string.Empty ? notifierEntity.Phone : null,
                notifierEntity.Cellphone != string.Empty ? notifierEntity.Cellphone : null,
                notifierEntity.Email != string.Empty ? notifierEntity.Email : null,
                notifierEntity.UserCode != string.Empty ? notifierEntity.UserCode : null,
                notifierEntity.PostalAddress != string.Empty ? notifierEntity.PostalAddress : null,
                notifierEntity.Postalcode != string.Empty ? notifierEntity.Postalcode : null,
                notifierEntity.City != string.Empty ? notifierEntity.City : null,
                notifierEntity.Title != string.Empty ? notifierEntity.Title : null,
                notifierEntity.Department_Id,
                notifierEntity.SOU != string.Empty ? notifierEntity.SOU : null,
                notifierEntity.OU_Id,
                notifierEntity.Division_Id,
                notifierEntity.ManagerComputerUser_Id,
                notifierEntity.ComputerUserGroup_Id,
                notifierEntity.Info != string.Empty ? notifierEntity.Info : null,
                notifierEntity.OrderPermission != 0,
                notifierEntity.Status != 0,
                notifierEntity.RegTime,
                notifierEntity.ChangeTime,
                notifierEntity.SyncChangedDate);
        }

        public NotifierDetails FindNotifierDetailsById(int notifierId)
        {
            var notifierEntity = this.DataContext.ComputerUsers.Find(notifierId);

            return new NotifierDetails(
                notifierEntity.Id, 
                notifierEntity.UserId != string.Empty ? notifierEntity.UserId : null, 
                notifierEntity.Domain_Id, 
                notifierEntity.LogonName != string.Empty ? notifierEntity.LogonName : null, 
                notifierEntity.FirstName != string.Empty ? notifierEntity.FirstName : null, 
                notifierEntity.Initials != string.Empty ? notifierEntity.Initials : null, 
                notifierEntity.SurName != string.Empty ? notifierEntity.SurName : null, 
                notifierEntity.DisplayName != string.Empty ? notifierEntity.DisplayName : null, 
                notifierEntity.Location != string.Empty ? notifierEntity.Location : null, 
                notifierEntity.Phone != string.Empty ? notifierEntity.Phone : null, 
                notifierEntity.Cellphone != string.Empty ? notifierEntity.Cellphone : null, 
                notifierEntity.Email != string.Empty ? notifierEntity.Email : null, 
                notifierEntity.UserCode != string.Empty ? notifierEntity.UserCode : null, 
                notifierEntity.PostalAddress != string.Empty ? notifierEntity.PostalAddress : null, 
                notifierEntity.Postalcode != string.Empty ? notifierEntity.Postalcode : null, 
                notifierEntity.City != string.Empty ? notifierEntity.City : null, 
                notifierEntity.Title != string.Empty ? notifierEntity.Title : null, 
                notifierEntity.Department_Id, 
                notifierEntity.SOU != string.Empty ? notifierEntity.SOU : null, 
                notifierEntity.OU == null ? (int?)null : notifierEntity.OU.Id, 
                notifierEntity.Division_Id, 
                notifierEntity.ManagerComputerUser_Id, 
                notifierEntity.ComputerUserGroup_Id, 
                notifierEntity.Info != string.Empty ? notifierEntity.Info : null, 
                notifierEntity.OrderPermission != 0, 
                notifierEntity.Status != 0, 
                notifierEntity.RegTime, 
                notifierEntity.ChangeTime, 
                notifierEntity.SyncChangedDate);
        }

        public List<ItemOverview> FindOverviewsByCustomerId(int customerId)
        {
            var notifiers = this.FindByCustomerIdCore(customerId);

            var notifiersWithUserId =
                notifiers.Where(n => !string.IsNullOrEmpty(n.UserId))
                         .Select(n => new { Id = n.Id, Name = n.UserId })
                         .OrderBy(x => x.Name)
                         .ToList();

            var notifiersWithEmail =
                notifiers.Where(n => string.IsNullOrEmpty(n.UserId) && !string.IsNullOrEmpty(n.Email))
                         .Select(n => new { Id = n.Id, Name = n.Email })
                         .OrderBy(x => x.Name)
                         .ToList();

            var notifierWithUserIdOverviews =
                notifiersWithUserId.Select(n => new ItemOverview(n.Name, n.Id.ToString(CultureInfo.InvariantCulture)))
                                   .OrderBy(x => x.Name)
                                   .ToList();

            var notifierWithEmailOverviews =
                notifiersWithEmail.Select(n => new ItemOverview(n.Name, n.Id.ToString(CultureInfo.InvariantCulture)))
                                  .OrderBy(x => x.Name)
                                  .ToList();

            notifierWithUserIdOverviews.AddRange(notifierWithEmailOverviews);
            return notifierWithUserIdOverviews;
        }

        public IList<UserSearchResults> Search(int customerId, string searchFor)
        {
            var s = searchFor.ToLower();

            var query = from cu in this.DataContext.ComputerUsers
                        join d in this.DataContext.Departments on cu.Department_Id equals d.Id into res
                        from k in res.DefaultIfEmpty()
                        where
                            cu.Customer_Id == customerId
                            && (cu.UserId.ToLower().Contains(s) || cu.FirstName.ToLower().Contains(s)
                                || cu.SurName.ToLower().Contains(s) || cu.Phone.ToLower().Contains(s)
                                || cu.Location.ToLower().Contains(s) || cu.Cellphone.ToLower().Contains(s)
                                || cu.Email.ToLower().Contains(s) || cu.UserCode.ToLower().Contains(s))
                        select
                            new UserSearchResults
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
                                    UserId = (cu.UserId != null ? cu.UserId : string.Empty) 
                                };

            return query.OrderBy(x => x.FirstName).ThenBy(x => x.SurName).ToList();
        }

        public SearchResult Search(SearchParameters parameters)
        {
            var notifierEntities = this.FindByCustomerIdCore(parameters.CustomerId);
            var requestBuilder = new NotifiersSearchRequestBuilder(notifierEntities);

            requestBuilder.FilterByStatus(parameters.Status);

            if (parameters.DomainId.HasValue)
            {
                requestBuilder.FilterByDomainId(parameters.DomainId.Value);
            }

            if (parameters.DepartmentId.HasValue)
            {
                requestBuilder.FilterByDepartmentId(parameters.DepartmentId.Value);
            }
            else if (parameters.RegionId.HasValue)
            {
                requestBuilder.FilterByRegionId(parameters.RegionId.Value);
            }

            if (parameters.DivisionId.HasValue)
            {
                requestBuilder.FilterByDivisionId(parameters.DivisionId.Value);
            }

            if (!string.IsNullOrEmpty(parameters.Pharse))
            {
                requestBuilder.FilterByPharse(parameters.Pharse);
            }

            var countingRequest = requestBuilder.Build();
            var notifiersFound = countingRequest.Count();

            if (parameters.SortField == null)
            {
                requestBuilder.OrderByDefault();
            }
            else
            {
                requestBuilder.OrderBy(parameters.SortField);
            }

            requestBuilder.TakeCount(parameters.SelectCount);
            var notifiersRequest = requestBuilder.Build();

            var searchResult =
                notifiersRequest.Select(
                    r =>
                    new
                        {
                            PostalAddress = r.PostalAddress != string.Empty ? r.PostalAddress : null,
                            CellPhone = r.Cellphone != string.Empty ? r.Cellphone : null,
                            ChangedDate = r.ChangeTime,
                            City = r.City != string.Empty ? r.City : null,
                            Code = r.UserCode != string.Empty ? r.UserCode : null,
                            CreatedDate = r.RegTime,
                            Department = r.Department.DepartmentName,
                            Division = r.Division.Name,
                            Domain = r.Domain.Name,
                            Email = r.Email != string.Empty ? r.Email : null,
                            FirstName = r.FirstName != string.Empty ? r.FirstName : null,
                            Group = r.ComputerUserGroup.Name != string.Empty ? r.ComputerUserGroup.Name : null,
                            Id = r.Id,
                            Initials = r.Initials != string.Empty ? r.Initials : null,
                            LastName = r.SurName != string.Empty ? r.SurName : null,
                            LoginName = r.LogonName != string.Empty ? r.LogonName : null,
                            Manager = r.ManagerComputerUser.UserId,
                            DisplayName = r.DisplayName != string.Empty ? r.DisplayName : null,
                            Ordered = r.OrderPermission != 0,
                            OrganizationUnit = r.OU.Name,
                            Other = r.Info != string.Empty ? r.Info : null,
                            Password = r.Password != string.Empty ? r.Password : null,
                            Phone = r.Phone != string.Empty ? r.Phone : null,
                            Place = r.Location != string.Empty ? r.Location : null,
                            PostalCode = r.Postalcode != string.Empty ? r.Postalcode : null,
                            SynchronizationDate = r.SyncChangedDate,
                            Title = r.Title != string.Empty ? r.Title : null,
                            Unit = r.SOU != string.Empty ? r.SOU : null,
                            UserId = r.UserId
                        }).ToList();

            var notifiers =
                searchResult.Select(
                    r =>
                    new NotifierDetailedOverview(
                        r.Id,
                        r.UserId,
                        r.Domain,
                        r.LoginName,
                        r.FirstName,
                        r.Initials,
                        r.LastName,
                        r.DisplayName,
                        r.Place,
                        r.Phone,
                        r.CellPhone,
                        r.Email,
                        r.Code,
                        r.PostalAddress,
                        r.PostalCode,
                        r.City,
                        r.Title,
                        r.Department,
                        r.Unit,
                        r.OrganizationUnit,
                        r.Division,
                        r.Manager,
                        r.Group,
                        r.Other,
                        r.Ordered,
                        r.CreatedDate,
                        r.ChangedDate,
                        r.SynchronizationDate)).ToList();

            return new SearchResult(notifiersFound, notifiers);
        }

        public void UpdateCaseNotifier(CaseNotifier caseNotifier)
        {
            var notifierEntity = this.Table
                                .Where(n => n.Customer_Id == caseNotifier.CustomerId && n.UserId == caseNotifier.UserId)
                                .ToList()
                                .FirstOrDefault();

            this.caseNotifierToEntityMapper.Map(caseNotifier, notifierEntity);
        }

        public void UpdateNotifier(Notifier notifier)
        {
            var notifierEntity = this.GetById(notifier.Id);

            notifierEntity.UserId = notifier.UserId;
            notifierEntity.Cellphone = notifier.CellPhone ?? string.Empty;
            notifierEntity.ChangeTime = notifier.ChangedDateAndTime;
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
            notifierEntity.Phone = notifier.Phone ?? string.Empty;
            notifierEntity.Location = notifier.Place ?? string.Empty;
            notifierEntity.PostalAddress = notifier.PostalAddress ?? string.Empty;
            notifierEntity.Postalcode = notifier.PostalCode ?? string.Empty;
            notifierEntity.Title = notifier.Title ?? string.Empty;
            notifierEntity.SOU = notifier.Unit ?? string.Empty;

            this.Update(notifierEntity);
        }

        #endregion

        #region Methods

        private IQueryable<ComputerUser> FindByCustomerIdCore(int customerId)
        {
            return this.DataContext.ComputerUsers.Where(u => u.Customer_Id == customerId);
        }

        #endregion
    }
}