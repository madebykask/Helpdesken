using System;
using System.Linq;

using DH.Helpdesk.BusinessData.Enums.Notifiers;
using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Domain.Computers;

namespace DH.Helpdesk.Dal.SearchRequestBuilders.Notifiers
{
    internal sealed class NotifiersSearchRequestBuilder
    {
        private IQueryable<ComputerUser> _notifiers; 

        internal NotifiersSearchRequestBuilder(IQueryable<ComputerUser> notifiers)
        {
            _notifiers = notifiers;
        }

        internal IQueryable<ComputerUser> Build()
        {
            return _notifiers;
        } 

        internal void OrderByDefault()
        {
            _notifiers = _notifiers.OrderBy(n => n.UserId).ThenBy(n => n.FirstName).ThenBy(n => n.SurName);
        }

        internal void OrderBy(SortField field)
        {
            switch (field.SortBy)
            {
                case SortBy.Ascending:
                    if (field.Name == GeneralField.UserId)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.UserId);
                    }
                    else if (field.Name == GeneralField.Domain)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.Domain.Name);
                    }
                    else if (field.Name == GeneralField.LoginName)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.LogonName);
                    }
                    else if (field.Name == GeneralField.FirstName)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.FirstName);
                    }
                    else if (field.Name == GeneralField.Initials)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.Initials);
                    }
                    else if (field.Name == GeneralField.LastName)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.SurName);
                    }
                    else if (field.Name == GeneralField.DisplayName)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.DisplayName);
                    }
                    else if (field.Name == GeneralField.Place)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.Location);
                    }
                    else if (field.Name == GeneralField.Phone)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.Phone);
                    }
                    else if (field.Name == GeneralField.CellPhone)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.Cellphone);
                    }
                    else if (field.Name == GeneralField.Email)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.Email);
                    }
                    else if (field.Name == GeneralField.Code)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.UserCode);
                    }
                    else if (field.Name == AddressField.PostalAddress)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.PostalAddress);
                    }
                    else if (field.Name == AddressField.PostalCode)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.Postalcode);
                    }
                    else if (field.Name == AddressField.City)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.City);
                    }
                    else if (field.Name == OrganizationField.Region)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.Department.Region.Name);
                    }
                    else if (field.Name == OrganizationField.Department)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.Department.DepartmentName);
                    }
                    else if (field.Name == OrganizationField.OrganizationUnit)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.OU.Name);
                    }
                    else if (field.Name == StateField.ChangedDate)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.ChangeTime);
                    }
                    else if (field.Name == StateField.SynchronizationDate)
                    {
                        _notifiers = _notifiers.OrderBy(u => u.SyncChangedDate);
                    }                    
                    break;

                case SortBy.Descending:
                    if (field.Name == GeneralField.UserId)
                    {
                        _notifiers = _notifiers.OrderByDescending(n => n.UserId);
                    }
                    else if (field.Name == GeneralField.Domain)
                    {
                        _notifiers = _notifiers.OrderByDescending(n => n.Domain.Name);
                    }
                    else if (field.Name == GeneralField.LoginName)
                    {
                        _notifiers = _notifiers.OrderByDescending(n => n.LogonName);
                    }
                    else if (field.Name == GeneralField.FirstName)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.FirstName);
                    }
                    else if (field.Name == GeneralField.Initials)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.Initials);
                    }
                    else if (field.Name == GeneralField.LastName)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.SurName);
                    }
                    else if (field.Name == GeneralField.DisplayName)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.DisplayName);
                    }
                    else if (field.Name == GeneralField.Place)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.Location);
                    }
                    else if (field.Name == GeneralField.Phone)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.Phone);
                    }
                    else if (field.Name == GeneralField.CellPhone)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.Cellphone);
                    }
                    else if (field.Name == GeneralField.Email)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.Email);
                    }
                    else if (field.Name == GeneralField.Code)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.UserCode);
                    }
                    else if (field.Name == AddressField.PostalAddress)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.PostalAddress);
                    }
                    else if (field.Name == AddressField.PostalCode)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.Postalcode);
                    }
                    else if (field.Name == AddressField.City)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.City);
                    }
                    else if (field.Name == OrganizationField.Region)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.Department.Region.Name);
                    }
                    else if (field.Name == OrganizationField.Department)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.Department.DepartmentName);
                    }
                    else if (field.Name == OrganizationField.OrganizationUnit)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.OU.Name);
                    }
                    else if (field.Name == StateField.ChangedDate)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.ChangeTime);
                    }
                    else if (field.Name == StateField.SynchronizationDate)
                    {
                        _notifiers = _notifiers.OrderByDescending(u => u.SyncChangedDate);
                    }                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void FilterByStatus(NotifierStatus status)
        {
            if (status == NotifierStatus.Active)
            {
                _notifiers = _notifiers.Where(n => n.Status != 0);
            }
            else if (status == NotifierStatus.Inactive)
            {
                _notifiers = _notifiers.Where(n => n.Status == 0);
            }
        }

        internal void FilterByDomainId(int domainId)
        {
            _notifiers = _notifiers.Where(n => n.Domain_Id == domainId);
        }

        internal void FilterByDepartmentId(int departmentId)
        {
            _notifiers = _notifiers.Where(n => n.Department_Id == departmentId);
        }

        internal void FilterByRegionId(int regionId)
        {
            _notifiers = _notifiers.Where(n => n.Department_Id.HasValue && n.Department.Region_Id == regionId);
        }

        internal void FilterByDivisionId(int divisionId)
        {
            _notifiers = _notifiers.Where(n => n.Division_Id == divisionId);
        }

        internal void FilterByOrganisationUnit(int orgUnitId)
        {
            _notifiers = _notifiers.Where(n => n.OU_Id == orgUnitId);
        }

        internal void TakeCount(int takeCount)
        {
            _notifiers = _notifiers.Take(takeCount);
        }

        internal void FilterByPharse(string pharse)
        {
            var pharseInLowerCase = pharse.ToLower();

            _notifiers =
                _notifiers.Where(
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

        internal void FilterByComputerUserCategoryId(int? value)
        {
            _notifiers = _notifiers.Where(o => o.ComputerUsersCategoryID == value);
        }
    }
}
