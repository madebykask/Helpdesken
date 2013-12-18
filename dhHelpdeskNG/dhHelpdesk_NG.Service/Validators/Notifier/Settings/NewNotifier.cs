namespace dhHelpdesk_NG.Service.Validators.Notifier.Settings
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class NewNotifier
    {
        public NewNotifier(
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
            bool ordered)
        {
            ArgumentsValidator.NotNullAndEmpty(userId, "userId");

            if (domainId.HasValue)
            {
                ArgumentsValidator.IsId(domainId.Value, "domainId");
            }

            if (departmentId.HasValue)
            {
                ArgumentsValidator.IsId(departmentId.Value, "departmentId");
            }

            if (organizationUnitId.HasValue)
            {
                ArgumentsValidator.IsId(organizationUnitId.Value, "organizationUnitId");
            }

            if (divisionId.HasValue)
            {
                ArgumentsValidator.IsId(divisionId.Value, "divisionId");
            }

            if (managerId.HasValue)
            {
                ArgumentsValidator.IsId(managerId.Value, "managerId");
            }

            if (groupId.HasValue)
            {
                ArgumentsValidator.IsId(groupId.Value, "groupId");
            }

            UserId = userId;
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

        public int? ManagerId { get; private set; }

        public int? GroupId { get; private set; }

        public string Password { get; private set; }

        public string Other { get; private set; }

        public bool Ordered { get; private set; }
    }
}
