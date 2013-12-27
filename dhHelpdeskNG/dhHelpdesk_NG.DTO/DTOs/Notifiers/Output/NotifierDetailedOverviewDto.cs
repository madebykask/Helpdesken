namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NotifierDetailedOverviewDto
    {
        public NotifierDetailedOverviewDto(
            int id,
            string userId,
            string domain,
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
            string department,
            string unit,
            string organizationUnit,
            string division,
            string manager,
            string group,
            string password,
            string other,
            bool ordered,
            DateTime createdDate,
            DateTime changedDate,
            DateTime? synchronizationDate)
        {
            Id = id;
            UserId = userId;
            Domain = domain;
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
            Department = department;
            Unit = unit;
            OrganizationUnit = organizationUnit;
            Division = division;
            Manager = manager;
            Group = group;
            Password = password;
            Other = other;
            Ordered = ordered;
            CreatedDate = createdDate;
            ChangedDate = changedDate;
            SynchronizationDate = synchronizationDate;
        }

        [IsId]
        public int Id { get; private set; }

        public string UserId { get; private set; }

        public string Domain { get; private set; }

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

        public string Department { get; private set; }

        public string Unit { get; private set; }

        public string OrganizationUnit { get; private set; }

        public string Division { get; private set; }

        public string Manager { get; private set; }

        public string Group { get; private set; }

        public string Password { get; private set; }

        public string Other { get; private set; }

        public bool Ordered { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public DateTime? SynchronizationDate { get; private set; }
    }
}
