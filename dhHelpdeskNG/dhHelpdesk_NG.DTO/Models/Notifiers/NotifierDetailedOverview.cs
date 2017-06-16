namespace DH.Helpdesk.BusinessData.Models.Notifiers
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NotifierDetailedOverview
    {
        public NotifierDetailedOverview(
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
            string region,
            string department,
            string unit,
            string organizationUnit,
            string division,
            string manager,
            string group,
            string other,
            bool ordered,
            DateTime createdDate,
            DateTime changedDate,
            DateTime? synchronizationDate,
            int? languageId,
            string language)
        {
            this.Id = id;
            this.UserId = userId;
            this.Domain = domain;
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
            this.Region = region;
            this.Department = department;
            this.Unit = unit;
            this.OrganizationUnit = organizationUnit;
            this.Division = division;
            this.Manager = manager;
            this.Group = group;
            this.Other = other;
            this.Ordered = ordered;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SynchronizationDate = synchronizationDate;
            this.LanguageId = languageId;
            this.Language = language;
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

        public string Region { get; private set; }

        public string Department { get; private set; }

        public string Unit { get; private set; }

        public string OrganizationUnit { get; private set; }

        public string Division { get; private set; }

        public string Manager { get; private set; }

        public string Group { get; private set; }

        public string Other { get; private set; }

        public bool Ordered { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public DateTime? SynchronizationDate { get; private set; }

        public int? LanguageId { get; private set; }

        public string Language { get; private set; }
    }
}
