namespace DH.Helpdesk.BusinessData.Models.Notifiers
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class Notifier : BusinessModel
    {
        #region Constructors and Destructors

        private Notifier()
        {
        }

        #endregion

        #region Public Properties

        public string CellPhone { get; internal set; }

        [AllowRead(ModelStates.ForEdit | ModelStates.Updated)]
        public DateTime ChangedDateAndTime { get; internal set; }

        public string City { get; internal set; }

        public string Code { get; internal set; }

        [AllowRead(ModelStates.Created | ModelStates.ForEdit)]
        public DateTime CreatedDateAndTime { get; private set; }

        [IsId]
        public int CustomerId { get; private set; }

        [IsId]
        public int? DepartmentId { get; internal set; }

        public string DisplayName { get; internal set; }

        [IsId]
        public int? DivisionId { get; internal set; }

        [IsId]
        public int? DomainId { get; internal set; }

        public string Email { get; internal set; }

        public string FirstName { get; internal set; }

        [IsId]
        public int? GroupId { get; internal set; }

        [IsId]
        public int Id { get; set; }

        public string Initials { get; internal set; }

        public bool IsActive { get; private set; }

        public string LastName { get; internal set; }

        public string LoginName { get; internal set; }

        [IsId]
        public int? ManagerId { get; internal set; }

        public bool Ordered { get; internal set; }

        [IsId]
        public int? OrganizationUnitId { get; internal set; }

        public string Other { get; internal set; }

        public string Phone { get; internal set; }

        public string Place { get; internal set; }

        public string PostalAddress { get; internal set; }

        public string PostalCode { get; internal set; }

        public DateTime? SynchronizationDateAndTime { get; private set; }

        public string Title { get; internal set; }

        public string Unit { get; internal set; }

        public string UserId { get; private set; }

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
            int? divisionId,
            int? managerId,
            int? groupId,
            string other,
            bool ordered,
            bool isActive,
            DateTime createdDateAndTime,
            DateTime? changedDateAndTime,
            DateTime? synchronizationDateAndTime)
        {
            return new Notifier
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
                       DivisionId = divisionId,
                       ManagerId = managerId,
                       GroupId = groupId,
                       Other = other,
                       Ordered = ordered,
                       IsActive = isActive,
                       CreatedDateAndTime = createdDateAndTime,
                       ChangedDateAndTime = changedDateAndTime ?? DateTime.Now,
                       SynchronizationDateAndTime = synchronizationDateAndTime
                   };
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
            int? divisionId,
            int? managerId,
            int? groupId,
            string other,
            bool ordered,
            bool isActive,
            DateTime createdDateAndTime)
        {
            return new Notifier
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
                       DivisionId = divisionId,
                       ManagerId = managerId,
                       GroupId = groupId,
                       Other = other,
                       Ordered = ordered,
                       IsActive = isActive,
                       CreatedDateAndTime = createdDateAndTime
                   };
        }

        public static Notifier CreateUpdated(
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
            string other,
            bool ordered,
            bool isActive,
            DateTime changedDateAndTime)
        {
            return new Notifier
                   {
                       Id = id,
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
                       DivisionId = divisionId,
                       ManagerId = managerId,
                       GroupId = groupId,
                       Other = other,
                       Ordered = ordered,
                       IsActive = isActive,
                       ChangedDateAndTime = changedDateAndTime
                   };
        }

        #endregion
    }
}