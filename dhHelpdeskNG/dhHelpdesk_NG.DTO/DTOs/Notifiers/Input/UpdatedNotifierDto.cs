namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Input
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class UpdatedNotifierDto
    {
        public UpdatedNotifierDto(
            int id,
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

        [IsId]
        public int? DomainId { get; set; }

        public string LoginName { get; set; }

        public string FirstName { get; set; }

        public string Initials { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Place { get; set; }

        public string Phone { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public string Code { get; set; }

        public string PostalAddress { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Title { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        public string Unit { get; set; }

        [IsId]
        public int? OrganizationUnitId { get; set; }

        [IsId]
        public int? DivisionId { get; set; }

        [IsId]
        public int? GroupId { get; set; }

        public string Password { get; set; }

        [IsId]
        public int? ManagerId { get; set; }

        public string Other { get; set; }

        public bool Ordered { get; set; }

        public bool IsActive { get; set; }

        public DateTime ChangedDate { get; set; }

        [IsId]
        public int Id { get; private set; }
    }
}
