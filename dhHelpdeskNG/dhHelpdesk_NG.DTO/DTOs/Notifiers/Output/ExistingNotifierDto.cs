namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ExistingNotifierDto
    {
        public ExistingNotifierDto(
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
            DateTime changedDate)
        {
            DomainId = domainId;
            LoginName = loginName;
            FirstName = firstName;
            Initials = initials;
            LastName = lastName;
            DisplayName = displayName;
            Place = place;
            Phone = phone;
            CellPhone = cellPhone;
            Email = email;
            Code = code;
            PostalAddress = postalAddress;
            PostalCode = postalCode;
            City = city;
            Title = title;
            DepartmentId = departmentId;
            Unit = unit;
            OrganizationUnitId = organizationUnitId;
            DivisionId = divisionId;
            ManagerId = managerId;
            GroupId = groupId;
            Password = password;
            Other = other;
            Ordered = ordered;
            ChangedDate = changedDate;
        }

        [IsId]
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

        [IsId]
        public int? DepartmentId { get; private set; }

        public string Unit { get; private set; }

        [IsId]
        public int? OrganizationUnitId { get; private set; }

        [IsId]
        public int? DivisionId { get; private set; }

        [IsId]
        public int? ManagerId { get; private set; }

        [IsId]
        public int? GroupId { get; private set; }

        public string Password { get; private set; }

        public string Other { get; private set; }

        public bool Ordered { get; private set; }

        public DateTime ChangedDate { get; private set; }
    }
}
