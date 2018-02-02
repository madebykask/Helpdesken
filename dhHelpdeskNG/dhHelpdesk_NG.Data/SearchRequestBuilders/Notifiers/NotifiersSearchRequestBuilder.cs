namespace DH.Helpdesk.Dal.SearchRequestBuilders.Notifiers
{
    using System;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Domain.Computers;

    public sealed class NotifiersSearchRequestBuilder
    {
        private IQueryable<ComputerUser> notifiers; 

        public NotifiersSearchRequestBuilder(IQueryable<ComputerUser> notifiers)
        {
            this.notifiers = notifiers;
        }

        public IQueryable<ComputerUser> Build()
        {
            return this.notifiers;
        } 

        public void OrderByDefault()
        {
            this.notifiers = this.notifiers.OrderBy(n => n.UserId).ThenBy(n => n.FirstName).ThenBy(n => n.SurName);
        }

        public void OrderBy(SortField field)
        {
            switch (field.SortBy)
            {
                case SortBy.Ascending:
                    if (field.Name == GeneralField.UserId)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.UserId);
                    }
                    else if (field.Name == GeneralField.Domain)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.Domain.Name);
                    }
                    else if (field.Name == GeneralField.LoginName)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.LogonName);
                    }
                    else if (field.Name == GeneralField.FirstName)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.FirstName);
                    }
                    else if (field.Name == GeneralField.Initials)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.Initials);
                    }
                    else if (field.Name == GeneralField.LastName)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.SurName);
                    }
                    else if (field.Name == GeneralField.DisplayName)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.DisplayName);
                    }
                    else if (field.Name == GeneralField.Place)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.Location);
                    }
                    else if (field.Name == GeneralField.Phone)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.Phone);
                    }
                    else if (field.Name == GeneralField.CellPhone)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.Cellphone);
                    }
                    else if (field.Name == GeneralField.Email)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.Email);
                    }
                    else if (field.Name == GeneralField.Code)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.UserCode);
                    }
                    else if (field.Name == AddressField.PostalAddress)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.PostalAddress);
                    }
                    else if (field.Name == AddressField.PostalCode)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.Postalcode);
                    }
                    else if (field.Name == AddressField.City)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.City);
                    }
                    else if (field.Name == OrganizationField.Region)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.Department.Region.Name);
                    }
                    else if (field.Name == OrganizationField.Department)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.Department.DepartmentName);
                    }
                    else if (field.Name == OrganizationField.OrganizationUnit)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.OU.Name);
                    }
                    else if (field.Name == StateField.ChangedDate)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.ChangeTime);
                    }
                    else if (field.Name == StateField.SynchronizationDate)
                    {
                        this.notifiers = this.notifiers.OrderBy(u => u.SyncChangedDate);
                    }                    
                    break;

                case SortBy.Descending:
                    if (field.Name == GeneralField.UserId)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(n => n.UserId);
                    }
                    else if (field.Name == GeneralField.Domain)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(n => n.Domain.Name);
                    }
                    else if (field.Name == GeneralField.LoginName)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(n => n.LogonName);
                    }
                    else if (field.Name == GeneralField.FirstName)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.FirstName);
                    }
                    else if (field.Name == GeneralField.Initials)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.Initials);
                    }
                    else if (field.Name == GeneralField.LastName)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.SurName);
                    }
                    else if (field.Name == GeneralField.DisplayName)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.DisplayName);
                    }
                    else if (field.Name == GeneralField.Place)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.Location);
                    }
                    else if (field.Name == GeneralField.Phone)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.Phone);
                    }
                    else if (field.Name == GeneralField.CellPhone)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.Cellphone);
                    }
                    else if (field.Name == GeneralField.Email)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.Email);
                    }
                    else if (field.Name == GeneralField.Code)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.UserCode);
                    }
                    else if (field.Name == AddressField.PostalAddress)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.PostalAddress);
                    }
                    else if (field.Name == AddressField.PostalCode)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.Postalcode);
                    }
                    else if (field.Name == AddressField.City)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.City);
                    }
                    else if (field.Name == OrganizationField.Region)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.Department.Region.Name);
                    }
                    else if (field.Name == OrganizationField.Department)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.Department.DepartmentName);
                    }
                    else if (field.Name == OrganizationField.OrganizationUnit)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.OU.Name);
                    }
                    else if (field.Name == StateField.ChangedDate)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.ChangeTime);
                    }
                    else if (field.Name == StateField.SynchronizationDate)
                    {
                        this.notifiers = this.notifiers.OrderByDescending(u => u.SyncChangedDate);
                    }                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void FilterByStatus(NotifierStatus status)
        {
            if (status == NotifierStatus.Active)
            {
                this.notifiers = this.notifiers.Where(n => n.Status != 0);
            }
            else if (status == NotifierStatus.Inactive)
            {
                this.notifiers = this.notifiers.Where(n => n.Status == 0);
            }
        }

        public void FilterByDomainId(int domainId)
        {
            this.notifiers = this.notifiers.Where(n => n.Domain_Id == domainId);
        }

        public void FilterByDepartmentId(int departmentId)
        {
            this.notifiers = this.notifiers.Where(n => n.Department_Id == departmentId);
        }

        public void FilterByRegionId(int regionId)
        {
            this.notifiers = this.notifiers.Where(n => n.Department_Id.HasValue && n.Department.Region_Id == regionId);
        }

        public void FilterByDivisionId(int divisionId)
        {
            this.notifiers = this.notifiers.Where(n => n.Division_Id == divisionId);
        }

        public void TakeCount(int takeCount)
        {
            this.notifiers = this.notifiers.Take(takeCount);
        }

        public void FilterByPharse(string pharse)
        {
            var pharseInLowerCase = pharse.ToLower();

            this.notifiers =
                this.notifiers.Where(
                    n =>
                    n.UserId.ToLower().Contains(pharseInLowerCase)
                    || n.Domain.Name.ToLower().Contains(pharseInLowerCase)
                    || n.LogonName.ToLower().Contains(pharseInLowerCase)
                    || n.FirstName.ToLower().Contains(pharseInLowerCase)
                    || n.Initials.ToLower().Contains(pharseInLowerCase)
                    || n.SurName.ToLower().Contains(pharseInLowerCase)
                    || n.DisplayName.ToLower().Contains(pharseInLowerCase)
                    || n.Location.ToLower().Contains(pharseInLowerCase) || n.Phone.ToLower().Contains(pharseInLowerCase)
                    || n.Cellphone.ToLower().Contains(pharseInLowerCase)
                    || n.Email.ToLower().Contains(pharseInLowerCase) || n.UserCode.ToLower().Contains(pharseInLowerCase));
        }

		internal void FilterByComputerUserCategoryID(int? value)
		{
			this.notifiers = this.notifiers.Where(o => o.ComputerUsersCategoryID == value);
		}
	}
}
