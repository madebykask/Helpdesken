namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Input
{
    using System;

    public sealed class UpdatedNotifierDto
    {
        public UpdatedNotifierDto(
            int id,
            string userId,
            int? domainId,
            string loginName,
            string firstName,
            string initials,
            string lastName,
            string displayName,
            string place,
            string phone,
            string cellPhone,
            string email,
            string code,
            string postalAddress,
            string postalCode,
            string city,
            string title,
            int? departmentId,
            string unit,
            int? organizationUnitId,
            int? divisionId,
            int? managerId,
            int? groupId,
            string password,
            string other,
            bool ordered,
            bool isActive,
            DateTime changedDate)
        {
            this.Id = id;
            this.UserId = userId;
            this.DomainId = domainId;
            this.LoginName = loginName;
            this.FirstName = firstName;
            this.Initials = initials;
            this.LastName = lastName;
            this.DisplayName = displayName;
            this.Place = place;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Code = code;
            this.PostalAddress = postalAddress;
            this.PostalCode = postalCode;
            this.City = city;
            this.Title = title;
            this.DepartmentId = departmentId;
            this.Unit = unit;
            this.OrganizationUnitId = organizationUnitId;
            this.DivisionId = divisionId;
            this.ManagerId = managerId;
            this.GroupId = groupId;
            this.Password = password;
            this.Other = other;
            this.Ordered = ordered;
            this.IsActive = isActive;
            this.ChangedDate = changedDate;
        }

        public string UserId { get; private set; }

        public int? DomainId { get; private set; }

        public string LoginName { get; private set; }

        public string FirstName { get; private set; }

        public string Initials { get; private set; }

        public string LastName { get; private set; }

        public string DisplayName { get; private set; }

        public string Place { get; private set; }

        public string Phone { get; private set; }

        public string CellPhone { get; private set; }

        public string Email { get; private set; }

        public string Code { get; private set; }

        public string PostalAddress { get; private set; }

        public string PostalCode { get; private set; }

        public string City { get; private set; }

        public string Title { get; private set; }

        public int? DepartmentId { get; private set; }

        public string Unit { get; private set; }

        public int? OrganizationUnitId { get; private set; }

        public int? DivisionId { get; private set; }

        public int? GroupId { get; private set; }

        public int? ManagerId { get; private set; }

        public string Password { get; private set; }

        public string Other { get; private set; }

        public bool Ordered { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public int Id { get; private set; }
    }
}
