namespace DH.Helpdesk.BusinessData.Models.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.Common.Constraints;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class Notifier : BusinessModel
    {
        #region Constructors and Destructors

        private Notifier()
        {
        }

        #endregion

        #region Public Properties

        [MaxLength(NotifierConstraint.CellPhoneMaxLength)]
        public string CellPhone { get; internal set; }

        [AllowRead(ModelStates.ForEdit | ModelStates.Updated)]
        public DateTime ChangedDateAndTime { get; internal set; }

        [MaxLength(NotifierConstraint.CityMaxLength)]
        public string City { get; internal set; }

        [MaxLength(NotifierConstraint.UserCodeMaxLength)]
        public string Code { get; internal set; }

        [AllowRead(ModelStates.Created | ModelStates.ForEdit)]
        public DateTime CreatedDateAndTime { get; private set; }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int? DepartmentId { get; internal set; }

        [MaxLength(NotifierConstraint.DisplayNameMaxLength)]
        public string DisplayName { get; internal set; }

        [IsId]
        public int? DivisionId { get; internal set; }

        [IsId]
        public int? DomainId { get; internal set; }

        [MaxLength(NotifierConstraint.EmailMaxLength)]
        public string Email { get; internal set; }

        [MaxLength(NotifierConstraint.FirstNameMaxLength)]
        public string FirstName { get; internal set; }

        [IsId]
        public int? GroupId { get; internal set; }

        //[IsId]
        public int LanguageId { get; internal set; }

        [IsId]
        public int? CategoryId { get; internal set; }

        [MaxLength(NotifierConstraint.InitialsMaxLength)]
        public string Initials { get; internal set; }

        public bool IsActive { get; private set; }

        [MaxLength(NotifierConstraint.LastNameMaxLength)]
        public string LastName { get; internal set; }

        [MaxLength(NotifierConstraint.LoginNameMaxLength)]
        public string LoginName { get; internal set; }

        [IsId]
        public int? ManagerId { get; internal set; }

        public bool Ordered { get; internal set; }

        [IsId]
        public int? OrganizationUnitId { get; internal set; }

        public string CostCentre { get; set; }

        [MaxLength(NotifierConstraint.OtherMaxLength)]
        public string Other { get; internal set; }

        [MaxLength(NotifierConstraint.PhoneMaxLength)]
        public string Phone { get; internal set; }

        [MaxLength(NotifierConstraint.PlaceMaxLenght)]
        public string Place { get; internal set; }

        [MaxLength(NotifierConstraint.PostalAddressMaxLength)]
        public string PostalAddress { get; internal set; }

        [MaxLength(NotifierConstraint.PostalCodeMaxLength)]
        public string PostalCode { get; internal set; }

        public DateTime? SynchronizationDateAndTime { get; private set; }

        [MaxLength(NotifierConstraint.TitleMaxLength)]
        public string Title { get; internal set; }

        [MaxLength(NotifierConstraint.UnitMaxLength)]
        public string Unit { get; internal set; }

        [MaxLength(NotifierConstraint.UserIdMaxLength)]
        public string UserId { get; internal set; }

        #endregion

        #region Public Methods and Operators

        public static Notifier CreateForEdit(
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
            string costCentre,
            int? divisionId,
            int? managerId,
            int? groupId,
            string other,
            bool ordered,
            bool isActive,
            DateTime createdDateAndTime,
            DateTime? changedDateAndTime,
            DateTime? synchronizationDateAndTime,
            int languageId,
            int? categoryId)
        {
            var notifier = new Notifier
            {
                UserId = userId,
                DomainId = domainId,
                LoginName = loginName,
                FirstName = firstName,
                Initials = initials,
                LastName = lastName,
                DisplayName = displayName,
                Place = place,
                Phone = phone,
                CellPhone = cellPhone,
                Email = email,
                Code = code,
                PostalAddress = postalAddress,
                PostalCode = postalCode,
                City = city,
                Title = title,
                DepartmentId = departmentId,
                Unit = unit,
                OrganizationUnitId = organizationUnitId,
                CostCentre = costCentre,
                DivisionId = divisionId,
                ManagerId = managerId,
                GroupId = groupId,
                Other = other,
                Ordered = ordered,
                IsActive = isActive,
                CreatedDateAndTime = createdDateAndTime,
                ChangedDateAndTime = changedDateAndTime ?? DateTime.Now,
                SynchronizationDateAndTime = synchronizationDateAndTime,
                LanguageId = languageId,
                CategoryId = categoryId
            };

            notifier.State = ModelStates.ForEdit;
            return notifier;
        }

        public static Notifier CreateNew(
            int customerId,
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
            string costCentre,
            int? divisionId,
            int? managerId,
            int? groupId,
            string other,
            bool ordered,
            bool isActive,
            DateTime createdDateAndTime,
            int languageid,
            int? categoryId)
        {
            var notifier = new Notifier
            {
                CustomerId = customerId,
                UserId = userId,
                DomainId = domainId,
                LoginName = loginName,
                FirstName = firstName,
                Initials = initials,
                LastName = lastName,
                DisplayName = displayName,
                Place = place,
                Phone = phone,
                CellPhone = cellPhone,
                Email = email,
                Code = code,
                PostalAddress = postalAddress,
                PostalCode = postalCode,
                City = city,
                Title = title,
                DepartmentId = departmentId,
                Unit = unit,
                OrganizationUnitId = organizationUnitId,
                CostCentre = costCentre,
                DivisionId = divisionId,
                ManagerId = managerId,
                GroupId = groupId,
                Other = other,
                Ordered = ordered,
                IsActive = isActive,
                CreatedDateAndTime = createdDateAndTime,
                LanguageId=languageid,
                CategoryId = categoryId
            };

            notifier.State = ModelStates.Created;
            return notifier;
        }

        public static Notifier CreateUpdated(
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
            string costCentre,
            int? divisionId,
            int? managerId,
            int? groupId,
            string other,
            bool ordered,
            bool isActive,
            DateTime changedDateAndTime,
            int languageid,
            int? categoryId)
        {
            var notifier = new Notifier
            {
                Id = id,
                UserId = userId,
                DomainId = domainId,
                LoginName = loginName,
                FirstName = firstName,
                Initials = initials,
                LastName = lastName,
                DisplayName = displayName,
                Place = place,
                Phone = phone,
                CellPhone = cellPhone,
                Email = email,
                Code = code,
                PostalAddress = postalAddress,
                PostalCode = postalCode,
                City = city,
                Title = title,
                DepartmentId = departmentId,
                Unit = unit,
                OrganizationUnitId = organizationUnitId,
                CostCentre = costCentre,
                DivisionId = divisionId,
                ManagerId = managerId,
                GroupId = groupId,
                Other = other,
                Ordered = ordered,
                IsActive = isActive,
                ChangedDateAndTime = changedDateAndTime,
                LanguageId = languageid,
                CategoryId = categoryId
            };

            notifier.State = ModelStates.Updated;
            return notifier;
        }

        #endregion
    }
}