namespace DH.Helpdesk.Services.BusinessLogic.Specifications.Accounts
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Accounts;
    using DH.Helpdesk.BusinessData.Enums.Accounts.Fields;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;

    public static class AccountSpecifications
    {
        public static IQueryable<Account> GetActivityTypeAccounts(this IQueryable<Account> query, int? activitTypeId)
        {
            if (activitTypeId.HasValue)
            {
                query = query.Where(x => x.AccountActivity_Id == activitTypeId);
            }

            return query;
        }

        public static IQueryable<Account> GetAdministratorAccounts(this IQueryable<Account> query, int? userId)
        {
            if (userId.HasValue)
            {
            }

            return query;
        }

        public static IQueryable<Account> GetAccountsBySearchString(this IQueryable<Account> query, string searchString)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                string str = searchString.Trim().ToLower();

                query =
                    query.Where(
                        x =>
                        x.OrdererId.Trim().ToLower().Contains(str) || x.OrdererFirstName.Trim().ToLower().Contains(str)
                        || x.OrdererLastName.Trim().ToLower().Contains(str)
                        || x.OrdererPhone.Trim().ToLower().Contains(str)
                        || x.OrdererEmail.Trim().ToLower().Contains(str) || x.UserId.Trim().ToLower().Contains(str)
                        || x.UserFirstName.Trim().ToLower().Contains(str)
                        || x.UserInitials.Trim().ToLower().Contains(str)
                        || x.UserLastName.Trim().ToLower().Contains(str)
                        || x.UserPersonalIdentityNumber.Trim().ToLower().Contains(str)
                        || x.UserPhone.Trim().ToLower().Contains(str) || x.UserExtension.Trim().ToLower().Contains(str)
                        || x.UserEMail.Trim().ToLower().Contains(str) || x.UserTitle.Trim().ToLower().Contains(str)
                        || x.UserLocation.Trim().ToLower().Contains(str)
                        || x.UserRoomNumber.Trim().ToLower().Contains(str)
                        || x.Department.DepartmentName.Trim().ToLower().Contains(str)
                        || x.OU.Name.Trim().ToLower().Contains(str)
                        || x.Department2.DepartmentName.Trim().ToLower().Contains(str)
                        || x.InfoUser.Trim().ToLower().Contains(str) || x.Responsibility.Trim().ToLower().Contains(str)
                        || x.Activity.Trim().ToLower().Contains(str) || x.Manager.Trim().ToLower().Contains(str)
                        || x.ReferenceNumber.Trim().ToLower().Contains(str)
                        || x.InventoryNumber.Trim().ToLower().Contains(str) || x.Info.Trim().ToLower().Contains(str)
                        || x.ContactId.Trim().ToLower().Contains(str) || x.ContactName.Trim().ToLower().Contains(str)
                        || x.ContactPhone.Trim().ToLower().Contains(str)
                        || x.ContactEMail.Trim().ToLower().Contains(str) || x.ContactId.Trim().ToLower().Contains(str)
                        || x.ContactName.Trim().ToLower().Contains(str) || x.ContactPhone.Trim().ToLower().Contains(str)
                        || x.ContactEMail.Trim().ToLower().Contains(str)
                        || x.DeliveryName.Trim().ToLower().Contains(str)
                        || x.DeliveryAddress.Trim().ToLower().Contains(str)
                        || x.DeliveryPhone.Trim().ToLower().Contains(str)
                        || x.DeliveryPostalAddress.Trim().ToLower().Contains(str)
                        || x.InfoProduct.Trim().ToLower().Contains(str) || x.InfoOther.Trim().ToLower().Contains(str)
                        || x.AccountFileName.Trim().ToLower().Contains(str));
            }

            return query;
        }

        public static IQueryable<Account> GetStateAccounts(this IQueryable<Account> query, AccountStates accountState)
        {
            if (accountState == AccountStates.All)
            {
                return query;
            }

            query = accountState == AccountStates.Active
                        ? query.Where(x => x.FinishingDate == null)
                        : query.Where(x => x.FinishingDate != null);

            return query;
        }

        public static IQueryable<Account> Sort(
            this IQueryable<Account> defaultQuery,
            IQueryable<EmploymentType> employmentTypes,
            IQueryable<AccountType> accountTypes,
            SortField sort)
        {
            if (sort == null)
            {
                return defaultQuery;
            }

            var query = from r3 in (from r2 in (from r1 in (from account in defaultQuery
                                                            join etype in employmentTypes on account.EmploymentType
                                                                equals etype.Id into group1
                                                            from g1 in group1.DefaultIfEmpty()
                                                            select
                                                                new { Account = account, EmploymentTypeName = g1.Name })
                                                join account2 in accountTypes on r1.Account.AccountType3 equals
                                                    account2.Id into group2
                                                from g2 in group2.DefaultIfEmpty()
                                                select
                                                    new
                                                        {
                                                            r1.Account,
                                                            r1.EmploymentTypeName,
                                                            AccountType3Name = g2.Name
                                                        })
                                    join account3 in accountTypes on r2.Account.AccountType4 equals account3.Id into
                                        group3
                                    from g3 in group3.DefaultIfEmpty()
                                    select
                                        new
                                            {
                                                r2.Account,
                                                r2.EmploymentTypeName,
                                                r2.AccountType3Name,
                                                AccountType4Name = g3.Name
                                            })
                        join account4 in accountTypes on r3.Account.AccountType5 equals account4.Id into group4
                        from g4 in group4.DefaultIfEmpty()
                        select
                            new
                                {
                                    r3.Account,
                                    r3.EmploymentTypeName,
                                    r3.AccountType3Name,
                                    r3.AccountType4Name,
                                    AccountType5Name = g4.Name
                                };

            switch (sort.SortBy)
            {
                case SortBy.Ascending:
                    // OrdererFields
                    if (sort.Name == OrdererFields.Id)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.OrdererId);
                    }
                    else if (sort.Name == OrdererFields.FirstName)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.OrdererFirstName);
                    }
                    else if (sort.Name == OrdererFields.LastName)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.OrdererLastName);
                    }
                    else if (sort.Name == OrdererFields.Email)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.OrdererEmail);
                    }
                    else if (sort.Name == OrdererFields.Phone)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.OrdererPhone);
                    }

                        // UserFields
                    else if (sort.Name == UserFields.Ids)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserId);
                    }
                    else if (sort.Name == UserFields.PersonalIdentityNumber)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserPersonalIdentityNumber);
                    }
                    else if (sort.Name == UserFields.FirstName)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserFirstName);
                    }
                    else if (sort.Name == UserFields.Initials)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserInitials);
                    }
                    else if (sort.Name == UserFields.LastName)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserLastName);
                    }
                    else if (sort.Name == UserFields.Phone)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserPhone);
                    }
                    else if (sort.Name == UserFields.Extension)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserExtension);
                    }
                    else if (sort.Name == UserFields.EMail)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserEMail);
                    }
                    else if (sort.Name == UserFields.Title)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserTitle);
                    }
                    else if (sort.Name == UserFields.Location)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserLocation);
                    }
                    else if (sort.Name == UserFields.RoomNumber)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserRoomNumber);
                    }
                    else if (sort.Name == UserFields.PostalAddress)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.UserPostalAddress);
                    }
                    else if (sort.Name == UserFields.EmploymentType)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.EmploymentTypeName);
                    }
                    else if (sort.Name == UserFields.DepartmentId)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.Department.DepartmentName);
                    }
                    else if (sort.Name == UserFields.UnitId)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.OU.Id);
                    }
                    else if (sort.Name == UserFields.DepartmentId2)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.Department2.DepartmentName);
                    }
                    else if (sort.Name == UserFields.Info)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.InfoUser);
                    }
                    else if (sort.Name == UserFields.Responsibility)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.Responsibility);
                    }
                    else if (sort.Name == UserFields.Activity)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.Activity);
                    }
                    else if (sort.Name == UserFields.Manager)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.Manager);
                    }
                    else if (sort.Name == UserFields.ReferenceNumber)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.ReferenceNumber);
                    }

                        // Account Information
                    else if (sort.Name == AccountInformationFields.StartedDate)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.AccountStartDate);
                    }
                    else if (sort.Name == AccountInformationFields.FinishDate)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.FinishingDate);
                    }
                    else if (sort.Name == AccountInformationFields.EMailTypeId)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.EMailType);
                    }
                    else if (sort.Name == AccountInformationFields.HomeDirectory)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.HomeDirectory);
                    }
                    else if (sort.Name == AccountInformationFields.Profile)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.Profile);
                    }
                    else if (sort.Name == AccountInformationFields.InventoryNumber)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.InventoryNumber);
                    }
                    else if (sort.Name == AccountInformationFields.AccountTypeId)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.AccountType.Name);
                    }
                    else if (sort.Name == AccountInformationFields.AccountType2)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.AccountType2); // todo
                    }
                    else if (sort.Name == AccountInformationFields.AccountType3)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.AccountType3Name);
                    }
                    else if (sort.Name == AccountInformationFields.AccountType4)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.AccountType3Name);
                    }
                    else if (sort.Name == AccountInformationFields.AccountType5)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.AccountType5Name);
                    }

                        // Contact
                    else if (sort.Name == ContactFields.Id)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.ContactId);
                    }
                    else if (sort.Name == ContactFields.Name)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.ContactName);
                    }
                    else if (sort.Name == ContactFields.Phone)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.ContactPhone);
                    }
                    else if (sort.Name == ContactFields.Email)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.ContactEMail);
                    }

                        // Delivery Information
                    else if (sort.Name == DeliveryInformationFields.Name)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.DeliveryName);
                    }
                    else if (sort.Name == DeliveryInformationFields.Phone)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.DeliveryPhone);
                    }
                    else if (sort.Name == DeliveryInformationFields.Address)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.DeliveryAddress);
                    }
                    else if (sort.Name == DeliveryInformationFields.PostalAddress)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.DeliveryPostalAddress);
                    }

                        // Delivery Information
                    else if (sort.Name == OtherFields.CaseNumber)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.CaseNumber);
                    }
                    else if (sort.Name == OtherFields.Info)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.InfoOther);
                    }
                    else if (sort.Name == OtherFields.FileName)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenBy(o => o.Account.AccountFileName);
                    }

                    break;

                case SortBy.Descending:
                    // OrdererFields
                    if (sort.Name == OrdererFields.Id)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.OrdererId);
                    }
                    else if (sort.Name == OrdererFields.FirstName)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.OrdererFirstName);
                    }
                    else if (sort.Name == OrdererFields.LastName)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.OrdererLastName);
                    }
                    else if (sort.Name == OrdererFields.Email)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.OrdererEmail);
                    }
                    else if (sort.Name == OrdererFields.Phone)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.OrdererPhone);
                    }

                        // UserFields
                    else if (sort.Name == UserFields.Ids)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserId);
                    }
                    else if (sort.Name == UserFields.PersonalIdentityNumber)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserPersonalIdentityNumber);
                    }
                    else if (sort.Name == UserFields.FirstName)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserFirstName);
                    }
                    else if (sort.Name == UserFields.Initials)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserInitials);
                    }
                    else if (sort.Name == UserFields.LastName)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserLastName);
                    }
                    else if (sort.Name == UserFields.Phone)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserPhone);
                    }
                    else if (sort.Name == UserFields.Extension)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserExtension);
                    }
                    else if (sort.Name == UserFields.EMail)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserEMail);
                    }
                    else if (sort.Name == UserFields.Title)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserTitle);
                    }
                    else if (sort.Name == UserFields.Location)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserLocation);
                    }
                    else if (sort.Name == UserFields.RoomNumber)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserRoomNumber);
                    }
                    else if (sort.Name == UserFields.PostalAddress)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.UserPostalAddress);
                    }
                    else if (sort.Name == UserFields.EmploymentType)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.EmploymentTypeName);
                    }
                    else if (sort.Name == UserFields.DepartmentId)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.Department.DepartmentName);
                    }
                    else if (sort.Name == UserFields.UnitId)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.OU.Id);
                    }
                    else if (sort.Name == UserFields.DepartmentId2)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.Department2.DepartmentName);
                    }
                    else if (sort.Name == UserFields.Info)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.InfoUser);
                    }
                    else if (sort.Name == UserFields.Responsibility)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.Responsibility);
                    }
                    else if (sort.Name == UserFields.Activity)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.Activity);
                    }
                    else if (sort.Name == UserFields.Manager)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.Manager);
                    }
                    else if (sort.Name == UserFields.ReferenceNumber)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.ReferenceNumber);
                    }

                        // Account Information
                    else if (sort.Name == AccountInformationFields.StartedDate)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.AccountStartDate);
                    }
                    else if (sort.Name == AccountInformationFields.FinishDate)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.FinishingDate);
                    }
                    else if (sort.Name == AccountInformationFields.EMailTypeId)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.EMailType);
                    }
                    else if (sort.Name == AccountInformationFields.HomeDirectory)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.HomeDirectory);
                    }
                    else if (sort.Name == AccountInformationFields.Profile)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.Profile);
                    }
                    else if (sort.Name == AccountInformationFields.InventoryNumber)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.InventoryNumber);
                    }
                    else if (sort.Name == AccountInformationFields.AccountTypeId)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.AccountType.Name);
                    }
                    else if (sort.Name == AccountInformationFields.AccountType2)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.AccountType2); // todo
                    }
                    else if (sort.Name == AccountInformationFields.AccountType3)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.AccountType3Name);
                    }
                    else if (sort.Name == AccountInformationFields.AccountType4)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.AccountType3Name);
                    }
                    else if (sort.Name == AccountInformationFields.AccountType5)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.AccountType5Name);
                    }

                        // Contact
                    else if (sort.Name == ContactFields.Id)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.ContactId);
                    }
                    else if (sort.Name == ContactFields.Name)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.ContactName);
                    }
                    else if (sort.Name == ContactFields.Phone)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.ContactPhone);
                    }
                    else if (sort.Name == ContactFields.Email)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.ContactEMail);
                    }

                        // Delivery Information
                    else if (sort.Name == DeliveryInformationFields.Name)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.DeliveryName);
                    }
                    else if (sort.Name == DeliveryInformationFields.Phone)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.DeliveryPhone);
                    }
                    else if (sort.Name == DeliveryInformationFields.Address)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.DeliveryAddress);
                    }
                    else if (sort.Name == DeliveryInformationFields.PostalAddress)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.DeliveryPostalAddress);
                    }

                        // Delivery Information
                    else if (sort.Name == OtherFields.CaseNumber)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.CaseNumber);
                    }
                    else if (sort.Name == OtherFields.Info)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.InfoOther);
                    }
                    else if (sort.Name == OtherFields.FileName)
                    {
                        query = query.OrderBy(x => x.Account.AccountActivity_Id).ThenByDescending(o => o.Account.AccountFileName);
                    }

                    break;
            }

            return query.Select(x => x.Account);
        }
    }
}
